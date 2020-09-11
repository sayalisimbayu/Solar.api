using System.Linq;
using solar.models;
using Microsoft.AspNetCore.Authorization;
using solar.generics.Providers;
using Microsoft.AspNetCore.Mvc;
using solar.iservices;
using System.Threading.Tasks;

namespace solar.api.Controllers.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServicesProvider<IUserService> _userProvider;
        public UserController(IServicesProvider<IUserService> userProvider)
        {
            _userProvider = userProvider;
        }

        [HttpGet("{id}/Name"), Authorize]
        public async Task<Feedback> GetName(int id, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getName(tenant, id);
        }
        [HttpGet("{id}"), Authorize]
        public async Task<Feedback> Get(int id, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getById(tenant, id);
        }
        [HttpGet("{id}/UserInfo"), Authorize]
        public async Task<Feedback> UserInfo(int id, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getUserInfoById(tenant, id);
        }
        [HttpGet("{id}/InfoByUser"), Authorize]
        public async Task<Feedback> UserInfoUserId(int id, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getUserInfoByUserId(tenant, id);
        }
        [HttpGet("{id}/UserSetting"), Authorize]
        public async Task<Feedback> UserSetting(int id, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getUserSettingsById(tenant, id);
        }
        [HttpPost("Save")]
        public async Task<Feedback> Save(AppUser data, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).save(tenant, data);
        }
        [HttpPost("SaveUserInfo")]
        public Feedback SaveUser(AppUserInfo data, string tenant = "")
        {
            return _userProvider.GetInstance(tenant).saveUser(tenant, data);
        }
        [HttpPost("Permissions"), Authorize]
        public async Task<Feedback> SavePermission(AppPermission[] data, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).save(tenant, data.ToList());
        }
        [HttpGet("{id}/{notificationId}/Delete"), Authorize]
        public async Task<Feedback> Delete(int id, int notificationId, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).delete(tenant, id, notificationId);
        }
        [HttpPost("Page"), Authorize]
        public async Task<Feedback> GetPage(Paged page, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getPage(tenant, page);
        }
        [HttpPost("MinifiedPage"), Authorize]
        public async Task<Feedback> GetMinifiedPage(Paged page, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getMinifiedPage(tenant, page);
        }
        [HttpGet("{username}/Permissions")]
        public async Task<Feedback> GetPermissions(string username, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getPermissions(tenant, username);
        }

        [HttpGet("{id}/PermissionsById/{active}")]
        public async Task<Feedback> GetPermissionsById(int id, int active = 1, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getPermissionsById(tenant, id, active);
        }
        [HttpPost("saveAppUserAddOnConfig")]
        public Feedback saveAppUserAddOnConfig(AppUserAddOnConfig data, string tenant = "")
        {
            return _userProvider.GetInstance(tenant).saveAppUserAddOnConfig(tenant, data);
        }
        [HttpPost("setThemeForUser")]
        public async Task<Feedback> setThemeForUser(AppUserTheme appUserTheme, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).setThemeForUser(tenant, appUserTheme);
        }
        [HttpGet("{id}/Config")]
        public Feedback getConfig(int id, string tenant = "")
        {
            return _userProvider.GetInstance(tenant).getConfigById(tenant, id);
        }
        [HttpPost("profileImage"), Authorize]
        public async Task<Feedback> SaveProfileImage([FromBody] UserProfileImage Image, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).SaveProfileImage(tenant, Image);
        }
    }
}