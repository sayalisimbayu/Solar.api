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
    public class GeneralRepo : IGeneralRepo
    {
        public string tableName => "APPSETTINGS";

        public List<AppSetting> getAppSettings()
        {
            try
            {
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM {0}", tableName));

                DataSet userData = command.ExecuteDataSet();
                if (userData.Tables[0].Rows.Count == 0)
                    return null;
                return userData.Tables[0].ConvertList<AppSetting>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
