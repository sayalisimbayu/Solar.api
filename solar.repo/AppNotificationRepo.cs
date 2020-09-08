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
    public class AppNotificationRepo : IAppNotificationRepo
    {
        public string tableName => "APPNOTIFICATION";

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

        public AppNotification getById(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID=@ID", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable notificationData = DBServer.ExecuteDataTable(command);
                if (notificationData.Rows.Count == 0)
                {
                    return null;
                }
                return notificationData.Rows[0].Convert<AppNotification>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<IList<AppNotification>, int> getByPage(Paged page)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {4} {2} {3} OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;SELECT COUNT(ID) TotalRecords FROM {4}",
                    page.pageNumber * page.pageSize,
                    page.pageSize,
                    !String.IsNullOrEmpty(page.search) ? String.Format("WHERE {0}", page.search) : "",
                    (!String.IsNullOrEmpty(page.orderby) ? String.Format("ORDER BY {0}", page.orderby) : "ORDER BY ID"), tableName));

                DataSet notificationData = command.ExecuteDataSet();
                if (notificationData.Tables[0].Rows.Count == 0)
                    return null;
                return new Tuple<IList<AppNotification>, int>(notificationData.Tables[0].ConvertList<AppNotification>(), Convert.ToInt32(notificationData.Tables[1].Rows[0][0]));
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
                DataTable notificationData = DBServer.ExecuteDataTable(command);
                if (notificationData.Rows.Count == 0)
                    return "";
                return notificationData.Rows[0]["NAME"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool save(ref AppNotification data)
        {
            try
            {
                SqlCommand command;
                if (data == null)
                {
                    throw new Exception("Null notification not allowed.");
                }
                if (String.IsNullOrEmpty(data.ntype)) {
                    data.ntype = "user";
                }
                if (data.userId == 0) {

                }
                data.updatedDate = DateTime.Now;
                if (data.id == 0)
                {
                    data.createdDate=DateTime.Now;
                    command = data.InsertInto();
                    data.id = DBServer.ExecuteNScalar(command);
                }
                else
                {
                    command = data.UpdateInto();
                    DBServer.ExecuteNon(command);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
