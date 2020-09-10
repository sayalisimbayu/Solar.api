using solar.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace solar.irepo
{
    public interface IAppPermissionRepo
    {
        string getName(int id);
        List<AppPermission> getAllDefaultPermissions();
        List<AppPermission> getByUserId(int id, bool active = false);
        AppPermission getById(int id);
        bool save(AppPermission data);
        bool save(List<AppPermission> data, SqlTransaction transaction);
        bool deleteByUserId(int id);
        bool delete(int id);
        Tuple<IList<AppPermission>, int> getByPage(int start, int number, string searchs, string orderby);
    }
}
