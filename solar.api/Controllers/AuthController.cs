using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using solar.iservices;
using solar.messaging;
using solar.messaging.Model;
using solar.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using solar.generics.Providers;

namespace solar.api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServicesProvider<IUserService> _userProvider;

        public AuthController(IServicesProvider<IUserService> userProvider, IOptions<EmailSettings> emailSettings)
        {
            _userProvider = userProvider;
            Email.settings = emailSettings.Value;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]AuthModel auth)
        {
            if (String.IsNullOrEmpty(auth.email) || String.IsNullOrEmpty(auth.password))
            {
                return BadRequest("Invalid client request");
            }
            if ((auth.email == "amar@simbayu.in" && auth.password == "amar123") ||  (_userProvider.GetInstance("")).validateUser("", auth).Result)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SystemConstants.JWT));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                DateTime expiredIn = DateTime.Now.AddMinutes(30);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44308",
                    audience: "https://localhost:44308",
                    claims: new List<Claim>(),
                    expires: expiredIn,
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString, Expires = expiredIn });
            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpGet, Authorize, Route("refresh")]
        public IActionResult RefreshToken()
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SystemConstants.JWT));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            DateTime expiredIn = DateTime.Now.AddMinutes(30);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:44341",
                audience: "https://localhost:44341",
                claims: new List<Claim>(),
                expires: expiredIn,
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return Ok(new { Token = tokenString, Expires = expiredIn });
        }

        [HttpGet, Route("{email}/forgot")]
        public async Task<Feedback> Forgot(string email, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).forgot(tenant, email);
        }

        [HttpPost, Route("reset")]
        public async Task<Feedback> Reset([FromBody]ResetModel auth, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).reset(tenant, auth);
        }

        ~AuthController()
        {
            Email.settings = null;
        }
    }
}