using solar.generics.DataHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace System
{
    public static class SqlRepository
    {
        public static SqlCommand InsertInto<T>(this T model, string TableName = "")
        {
            SqlCommand insertCommand = new SqlCommand();
            StringBuilder column = new StringBuilder();
            StringBuilder values = new StringBuilder();
            bool first = true;
            foreach (var e in model.GetType().GetProperties())
            {
                if (!Attribute.IsDefined(e, typeof(NoBind)))
                {
                    if (e.Name.ToLower() != "id")
                    {


                        if (e.PropertyType.BaseType.Name == "Array")
                        {
                            if (!first)
                            {
                                column.Append(",");
                                values.Append(",");
                            }

                            first = false;
                            if (((Array)e.GetValue(model)).GetValue(0) != null)
                            {
                                column.Append(e.Name);
                                values.Append(String.Format("@{0}", e.Name));

                                insertCommand.Parameters.AddWithValue(String.Format("@{0}", e.Name),
                                    ((Array)e.GetValue(model)).GetValue(0));
                            }
                        }
                        else
                        {
                            if ((e.PropertyType == typeof(DateTime) && ((DateTime)e.GetValue(model)) != new DateTime()))
                            {
                                if (!first)
                                {
                                    column.Append(",");
                                    values.Append(",");
                                }

                                first = false;

                                column.Append(e.Name);
                                values.Append(String.Format("@{0}", e.Name));

                                insertCommand.Parameters.AddWithValue(String.Format("@{0}", e.Name), e.GetValue(model));
                            }
                            else if (e.GetValue(model) != null && e.PropertyType != typeof(DateTime))
                            {
                                if (!first)
                                {
                                    column.Append(",");
                                    values.Append(",");
                                }

                                first = false;

                                column.Append(e.Name);
                                values.Append(String.Format("@{0}", e.Name));

                                insertCommand.Parameters.AddWithValue(String.Format("@{0}", e.Name), e.GetValue(model));
                            }
                        }

                    }
                }
            }
            insertCommand.CommandText = String.Format("INSERT INTO {0} ({1}) VALUES ({2}); SELECT CAST(scope_identity() AS int);", TableName.Length > 0 ? TableName : model.GetType().Name, column.ToString(), values.ToString());
            return insertCommand;

        }
        public static SqlCommand UpdateInto<T>(this T model, string TableName = "")
        {
            SqlCommand updateCommand = new SqlCommand();
            StringBuilder values = new StringBuilder();
            StringBuilder where = new StringBuilder();
            bool first = true;
            foreach (var e in model.GetType().GetProperties())
            {
                if (!Attribute.IsDefined(e, typeof(NoBind)))
                {
                    if (e.GetValue(model) != null)
                    {
                        if (!first)
                        {
                            values.Append(",");
                        }
                        if (e.Name.ToLower() != "id")
                        {

                            first = false;
                            values.Append(String.Format("{0}=@{0}", e.Name));
                            updateCommand.Parameters.AddWithValue(String.Format("@{0}", e.Name), e.GetValue(model));

                        }
                        else
                        {
                            where.Append(String.Format("{0}=@{0}", e.Name));
                            updateCommand.Parameters.AddWithValue(String.Format("@{0}", e.Name), e.GetValue(model));
                        }
                    }
                }
            }
            updateCommand.CommandText = String.Format("UPDATE {0} SET {1} WHERE {2}", TableName.Length > 0 ? TableName : model.GetType().Name, values.ToString(), where.ToString());
            return updateCommand;
        }
    }
}
