using solar.generics.DataHelper;
using solar.irepo;
using solar.models;
using solar.generics.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace solar.repo
{
    public class AppPermissionRepo : IAppPermissionRepo
    {
        public string tableName => "APPPERMISSION";

        public string getName(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID=@ID", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable productData = DBServer.ExecuteDataTable(command);
                if (productData.Rows.Count == 0)
                    return "";
                return productData.Rows[0]["NAME"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppPermission> getAllDefaultPermissions()
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT s.Mode,s.ALIAS,s.VALUE, 0 [ID],-1 [APPUSERID],s.ID [APPSETTINGID],4 [PERMISSION] FROM AppSettings s");
                DataTable permissionData = DBServer.ExecuteDataTable(command);
                if (permissionData.Rows.Count == 0)
                {
                    return null;
                }
                return permissionData.ConvertList<AppPermission>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppPermission> getByUserId(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format(@"SELECT s.Mode,s.ALIAS,s.VALUE, ISNULL(p.ID,0) ID,
                        ISNULL(p.APPUSERID,@ID) APPUSERID, ISNULL(p.APPSETTINGID,s.ID) APPSETTINGID,
                        ISNULL(p.PERMISSION,0) PERMISSION
                        FROM AppSettings s left join APPPERMISSION p on s.id=p.appsettingid and p.APPUSERID=@ID WHERE s.TYPE='M'", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable permissionData = DBServer.ExecuteDataTable(command);
                if (permissionData.Rows.Count == 0)
                {
                    return null;
                }
                return permissionData.ConvertList<AppPermission>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AppPermission getById(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID=@ID", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable permissionData = DBServer.ExecuteDataTable(command);
                if (permissionData.Rows.Count == 0)
                {
                    return null;
                }
                return permissionData.Rows[0].Convert<AppPermission>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool save(List<AppPermission> data, SqlTransaction transaction)
        {
            try
            {
                List<SqlCommand> command = new List<SqlCommand>();
                foreach (AppPermission permission in data)
                {
                    if (permission.id == 0)
                    {
                        command.Add(permission.InsertInto());
                    }
                    else
                    {
                        command.Add(permission.UpdateInto());
                    }
                }
                command.ExecuteNon(transaction);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool save(AppPermission data)
        {
            try
            {
                SqlCommand command;
                if (data.id == 0)
                {
                    command = data.InsertInto();
                }
                else
                {
                    command = data.UpdateInto();
                }
                DBServer.ExecuteNon(command);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool save(AppPermission[] permissions, SqlTransaction transaction)
        {
            try
            {
                List<SqlCommand> commands = new List<SqlCommand>();
                foreach (var appPermission in permissions)
                {
                    if (appPermission.id == 0)
                    {
                        commands.Add(appPermission.InsertInto());
                    }
                    else
                    {
                        commands.Add(permissions.UpdateInto());
                    }
                }
                DBServer.ExecuteNScalar(commands, transaction);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool delete(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("DELETE FROM {0} WHERE ID=@ID", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DBServer.ExecuteNon(command);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool deleteByUserId(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("UPDATE {0} SET ISDELETED=1 WHERE APPUSERID=@ID", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DBServer.ExecuteNon(command);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Tuple<IList<AppPermission>, int> getByPage(int start, int number, string searchs, string orderby)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {4} {2} {3} OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;SELECT COUNT(ID) TotalRecords FROM {4}",
                    start,
                    number,
                    !String.IsNullOrEmpty(searchs) ? String.Format("WHERE {0}", searchs) : "",
                    (!String.IsNullOrEmpty(orderby) ? String.Format("ORDER BY {0}", orderby) : "ORDER BY ID"), tableName));

                DataSet permissionData = command.ExecuteDataSet();
                if (permissionData.Tables[0].Rows.Count == 0)
                    return null;
                return new Tuple<IList<AppPermission>, int>(permissionData.Tables[0].ConvertList<AppPermission>(), Convert.ToInt32(permissionData.Tables[0].Rows[0]["TotalRecords"]));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
