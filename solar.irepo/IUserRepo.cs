using solar.api;
using solar.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace solar.irepo
{
    public interface IUserRepo : IMasterRepo
    {
        string getName(int id);
        AppUser getById(int id);
        AppUser getByUserName(string username);
        AppUserInfo getUserSettingsById(int id);
        AppUserInfo getUserInfoById(int id);
        AppUserInfo getUserInfoByUserId(int id);
        bool save(ref AppUser data, SqlTransaction transaction);

        bool saveUser(ref AppUserInfo data, SqlTransaction transaction);
        bool delete(int id);
        AppUserAddOnConfig getConfigById(int id);
        bool saveAppUserAddOnConfig(ref AppUserAddOnConfig data, SqlTransaction transaction);
        Tuple<IList<AppUser>, int> getByPage(Paged page);

    }
}
