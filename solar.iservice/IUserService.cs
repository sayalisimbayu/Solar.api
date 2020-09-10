using solar.api;
using solar.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace solar.iservices
{
    public interface IUserService
    {
        Task<Feedback> getName(string tenant, int id);
        Task<Feedback> getById(string tenant, int id);
        Task<Feedback> getUserInfoById(string tenant, int id);
        Task<Feedback> getUserInfoByUserId(string tenant, int id);
        Task<Feedback> getUserSettingsById(string tenant, int id);
        Task<Feedback> save(string tenant, AppUser data);
        Task<Feedback> save(string tenant, List<AppPermission> data);
        Task<Feedback> delete(string tenant, int id, int notificationId);
        Feedback saveUser(string tenant, AppUserInfo data);
        Task<Feedback> getPage(string tenant, Paged page);
        Task<Feedback> getMinifiedPage(string tenant, Paged page);
        Task<Feedback> getPermissions(string tenant, string username);
        Task<Feedback> getPermissionsById(string tenant, int id, int active);
        Task<Boolean> validateUser(string tenant, AuthModel auth);
        Task<Feedback> forgot(string tenant, string email);
        Task<Feedback> reset(string tenant, ResetModel reset);
        Feedback saveAppUserAddOnConfig(string tenant, AppUserAddOnConfig data);
        Feedback getConfigById(string tenant, int id);
        Task<Feedback> SaveProfileImage(string tenant, UserProfileImage Image);
    }
}
