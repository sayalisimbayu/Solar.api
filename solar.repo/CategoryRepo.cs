using solar.generics.DataHelper;
using solar.generics.Providers;
using solar.irepo;
using solar.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace solar.repo
{
    public class CategoryRepo : ICategoryRepo
    {
        public string tableName => "CATEGORY";

        public bool delete(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("UPDATE {0} SET ISDELETED = 1 WHERE ID=@ID", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DBServer.ExecuteNon(command);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Category getById(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID=@ID AND ISDELETED=0", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable categoryData = DBServer.ExecuteDataTable(command);
                if (categoryData.Rows.Count == 0)
                {
                    return null;
                }
                return categoryData.Rows[0].Convert<Category>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<IList<Category>, int> getByPage(Paged page)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT C.*,COUNT(P.ID) PRODUCTCOUNT FROM {5} C LEFT JOIN PRODUCTCATEGORIES P ON C.ID=P.CATEGORYID {2} {3} {4} OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;SELECT COUNT(ID) TotalRecords FROM {5} C {2}",
                    page.pageNumber* page.pageSize,
                    page.pageSize,
                    (!String.IsNullOrEmpty(page.search) ? String.Format("WHERE {0} AND C.ISDELETED=0 ", page.search) : " WHERE C.ISDELETED=0 "),
                    " GROUP BY C.ID,C.NAME,C.ISDELETED ",
                    (!String.IsNullOrEmpty(page.orderby) ? String.Format("ORDER BY {0}", page.orderby) : "ORDER BY C.ID"), tableName));

                DataSet categoryData = command.ExecuteDataSet();
                if (categoryData.Tables[0].Rows.Count == 0)
                    return null;
                return new Tuple<IList<Category>, int>(categoryData.Tables[0].ConvertList<Category>(), Convert.ToInt32(categoryData.Tables[1].Rows[0]["TotalRecords"]));
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
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID=@ID AND ISDELETED=0", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable categoryData = DBServer.ExecuteDataTable(command);
                if (categoryData.Rows.Count == 0)
                    return "";
                return categoryData.Rows[0]["NAME"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool save(Category data)
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
    }
}
