using solar.api;
using solar.generics.DataHelper;
using solar.irepo;
using solar.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace solar.repo
{
    public class UserRepo : IUserRepo
    {
        public string tableName => "APPUSER";

        public string configTableName => "APPUSERADDONCONFIG";

        public bool delete(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("UPDATE {0} SET ISDELETED=1 WHERE ID=@ID", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DBServer.ExecuteNon(command);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppUser getById(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID=@ID AND ISDELETED=0", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable userData = DBServer.ExecuteDataTable(command);
                if (userData.Rows.Count == 0)
                {
                    return null;
                }
                return userData.Rows[0].Convert<AppUser>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AppUserInfo getUserInfoById(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM APPUSERINFO WHERE ID=@ID");
                command.Parameters.AddWithValue("@ID", id);
                DataTable userData = DBServer.ExecuteDataTable(command);
                if (userData.Rows.Count == 0)
                {
                    return null;
                }
                return userData.Rows[0].Convert<AppUserInfo>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AppUserInfo getUserSettingsById(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM USERSETTINGS WHERE ID=@ID");
                command.Parameters.AddWithValue("@ID", id);
                DataTable userData = DBServer.ExecuteDataTable(command);
                if (userData.Rows.Count == 0)
                {
                    return null;
                }
                return userData.Rows[0].Convert<AppUserInfo>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppUser getByUserName(string username)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE USERNAME=@USERNAME AND ISDELETED=0", tableName));
                command.Parameters.AddWithValue("@USERNAME", username);
                DataTable userData = DBServer.ExecuteDataTable(command);
                if (userData.Rows.Count == 0)
                {
                    return null;
                }
                return userData.Rows[0].Convert<AppUser>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<IList<AppUser>, int> getByPage(int start, int number, string searchs, string orderby)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {4} {2} {3} OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;SELECT COUNT(ID) TotalRecords FROM {4}",
                    start * number,
                    number,
                    !String.IsNullOrEmpty(searchs) ? String.Format("WHERE {0}  AND ISDELETED=0", searchs) : " WHERE ISDELETED=0 ",
                    (!String.IsNullOrEmpty(orderby) ? String.Format("ORDER BY {0}", orderby) : "ORDER BY ID"), tableName));

                DataSet userData = command.ExecuteDataSet();
                if (userData.Tables[0].Rows.Count == 0)
                    return null;
                return new Tuple<IList<AppUser>, int>(userData.Tables[0].ConvertList<AppUser>(), Convert.ToInt32(userData.Tables[1].Rows[0]["TotalRecords"]));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getName(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID=@ID", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable userData = DBServer.ExecuteDataTable(command);
                if (userData.Rows.Count == 0)
                    return "";
                return userData.Rows[0]["USERNAME"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool save(ref AppUser data, SqlTransaction transaction)
        {
            try
            {
                SqlCommand command;
                if (data.id == 0)
                {
                    command = data.InsertInto();
                    data.id = command.ExecuteNScalar(transaction);
                }
                else
                {
                    command = data.UpdateInto();
                    command.ExecuteNon(transaction);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool saveUser(ref AppUserInfo data, SqlTransaction transaction)
        {
            try
            {
                SqlCommand command;
                if (data.ID == 0)
                {
                    command = data.InsertInto();
                    data.ID = command.ExecuteNScalar(transaction);
                }
                else
                {
                    command = data.UpdateInto();
                    command.ExecuteNon(transaction);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool saveAppUserAddOnConfig(ref AppUserAddOnConfig data, SqlTransaction transaction)
        {
            try
            {
                SqlCommand command;
                if (data.ID == 0)
                {
                    command = data.InsertInto();
                    data.ID = command.ExecuteNScalar(transaction);
                }
                else
                {
                    command = data.UpdateInto();
                    command.ExecuteNon(transaction);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppUserAddOnConfig getConfigById(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID=@ID", configTableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable userData = DBServer.ExecuteDataTable(command);
                if (userData.Rows.Count == 0)
                {
                    return null;
                }
                return userData.Rows[0].Convert<AppUserAddOnConfig>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
