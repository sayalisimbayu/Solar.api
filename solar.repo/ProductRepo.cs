using solar.generics.DataHelper;
using solar.irepo;
using solar.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace solar.repo
{
    public class ProductRepo : IProductRepo
    {
        public string tableName => "PRODUCTS";

        public string getName(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID=@ID AND ISDELETED=0", tableName));
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
        public Products getById(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE ID=@ID AND ISDELETED=0", tableName));
                command.Parameters.AddWithValue("@ID", id);
                DataTable productData = DBServer.ExecuteDataTable(command);
                if (productData.Rows.Count == 0)
                {
                    return null;
                }
                return productData.Rows[0].Convert<Products>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool save(Products data)
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
        public Tuple<IList<Products>, int> getByPage(int start, int number, string searchs, string orderby)
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {4} {2} {3} OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;SELECT COUNT(ID) TotalRecords FROM {4} WHERE ISDELETED=0",
                    start*number,
                    number,
                    !String.IsNullOrEmpty(searchs) ? String.Format("WHERE {0}  AND ISDELETED=0 ", searchs) : " WHERE ISDELETED=0 ",
                    (!String.IsNullOrEmpty(orderby) ? String.Format("ORDER BY {0}", orderby) : "ORDER BY ID"), tableName));

                DataSet productData = command.ExecuteDataSet();
                if (productData.Tables[0].Rows.Count == 0)
                    return null;
                return new Tuple<IList<Products>, int>(productData.Tables[0].ConvertList<Products>(), Convert.ToInt32(productData.Tables[1].Rows[0]["TotalRecords"]));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
