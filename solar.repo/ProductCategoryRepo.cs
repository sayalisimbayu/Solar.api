using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using solar.generics.DataHelper;
using solar.irepo;
using solar.models;

namespace solar.repo
{
    public class ProductCategoryRepo: IProductCategoryRepo
    {
        public string tableName => "PRODUCTCATEGORIES";

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

        public ProductCategory getById(int id)
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
                return categoryData.Rows[0].Convert<ProductCategory>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<IList<ProductCategory>, int> getByPage(int start, int number, string searchs, string orderby)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT PC.*,C.NAME [CATEGORYNAME] FROM {4} PC LEFT JOIN CATEGORY C ON PC.CATEGORYID=C.ID {2} {3} OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;SELECT COUNT(ID) TotalRecords FROM {4}",
                    start * number,
                    number,
                    !String.IsNullOrEmpty(searchs) ? String.Format("WHERE {0}  AND PC.ISDELETED=0 ", searchs) : " WHERE PC.ISDELETED=0 ",
                    (!String.IsNullOrEmpty(orderby) ? String.Format("ORDER BY {0}", orderby) : "ORDER BY PC.ID"), tableName));

                DataSet categoryData = command.ExecuteDataSet();
                if (categoryData.Tables[0].Rows.Count == 0)
                    return null;
                return new Tuple<IList<ProductCategory>, int>(categoryData.Tables[0].ConvertList<ProductCategory>(), Convert.ToInt32(categoryData.Tables[1].Rows[0]["TotalRecords"]));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProductCategory[] getByProductId(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE PRODUCTID=@ID AND ISDELETED=0", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable categoryData = DBServer.ExecuteDataTable(command);
                if (categoryData.Rows.Count == 0)
                {
                    return null;
                }
                return categoryData.ConvertToArray<ProductCategory>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProductCategory[] getByCategoryId(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE CATEGORYID=@ID AND ISDELETED=0", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable categoryData = DBServer.ExecuteDataTable(command);
                if (categoryData.Rows.Count == 0)
                {
                    return null;
                }
                return categoryData.ConvertToArray<ProductCategory>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProductCategory[] getByProductIds(int[] ids)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE PRODUCTID in (@ID) AND ISDELETED=0", tableName));
                command.Parameters.AddWithValue("@ID", ids);
                DataTable categoryData = DBServer.ExecuteDataTable(command);
                if (categoryData.Rows.Count == 0)
                {
                    return null;
                }
                return categoryData.ConvertToArray<ProductCategory>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProductCategory[] getByCategoryIds(int[] ids)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE CATEGORYID in (@ID) AND ISDELETED=0", tableName));
                command.Parameters.AddWithValue("@ID", ids);
                DataTable categoryData = DBServer.ExecuteDataTable(command);
                if (categoryData.Rows.Count == 0)
                {
                    return null;
                }
                return categoryData.ConvertToArray<ProductCategory>();
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

        public bool save(ProductCategory data)
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
