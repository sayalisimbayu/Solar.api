using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace System
{
    public static class ObjectHelper
    {
        public static T Convert<T>(this DataRow row) where T : new()
        {
            T obj = new T();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (String.Compare(prop.Name.ToLower(), row.Table.Columns[i].ColumnName.ToLower()) == 0)
                    {
                        prop.SetValue(obj, row[row.Table.Columns[i].ColumnName]);
                    }
                }
            }
            return obj;
        }
        public static List<T> ConvertList<T>(this DataTable table) where T : new()
        {
            List<T> obj = new List<T>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                T element = new T();
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        if (prop.Name.ToLower()== table.Columns[j].ColumnName.ToLower())
                        {
                            prop.SetValue(element, table.Rows[i][table.Columns[j].ColumnName]);
                        }
                    }
                }
                obj.Add(element);
            }
            return obj;
        }

        public static T[] ConvertToArray<T>(this DataTable table) where T : new()
        {
            return table.ConvertList<T>().ToArray();
        }
    }
}
