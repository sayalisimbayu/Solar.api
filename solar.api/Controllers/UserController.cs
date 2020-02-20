using System.Linq;
using solar.models;
using Microsoft.AspNetCore.Authorization;
using solar.api.Providers;
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
        [HttpPost("Permission"), Authorize]
        public async Task<Feedback> SavePermission(AppUser data, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).save(tenant, data.permissions.ToList());
        }
        [HttpGet("{id}/{notificationId}/Delete"), Authorize]
        public async Task<Feedback> Delete(int id, int notificationId, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).delete(tenant, id, notificationId);
        }
        [HttpGet("{start}/{number}/Page"), Authorize]
        public async Task<Feedback> GetPage(int start, int number, string searchs = "", string orderby = "", string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getPage(tenant, start, number, searchs, orderby);
        }

        [HttpGet("{username}/Permissions")]
        public async Task<Feedback> GetPermissions(string username, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getPermissions(tenant, username);
        }

        [HttpGet("{id}/PermissionsById")]
        public async Task<Feedback> GetPermissionsById(int id, string tenant = "")
        {
            return await _userProvider.GetInstance(tenant).getPermissionsById(tenant, id);
        }
        [HttpPost("saveAppUserAddOnConfig")]
        public Feedback saveAppUserAddOnConfig(AppUserAddOnConfig data, string tenant = "")
        {
            return _userProvider.GetInstance(tenant).saveAppUserAddOnConfig(tenant, data);
        }
        [HttpGet("{id}/Config")]
        public Feedback getConfig(int id, string tenant = "")
        {
            return _userProvider.GetInstance(tenant).getConfigById(tenant, id);
        }
    }
}