using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace solar.generics.DataHelper
{
    public static class DBServer
    {
        public static IConfiguration configuration;
        private static string sqlConnectionString = ConfigurationExtensions.GetConnectionString(configuration, "simbayuConn");
        private static SqlConnection connection = new SqlConnection(sqlConnectionString);
        public static void setConnectionString()
        {
            sqlConnectionString = ConfigurationExtensions.GetConnectionString(configuration, "simbayuConn");
            connection = new SqlConnection(sqlConnectionString);
        }
        public static DataSet ExecuteDataSet(this SqlCommand command)
        {
            command.Connection = connection;
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            DataSet table = new DataSet();
            using (SqlDataAdapter dr = new SqlDataAdapter())
            {
                dr.SelectCommand = command;
                dr.Fill(table);
                return table;
            }
        }
        public static DataTable ExecuteDataTable(this SqlCommand command)
        {
            command.Connection = connection;
            DataTable table = new DataTable();

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            else if (connection.State != ConnectionState.Closed)
            {
                using (SqlConnection localConn = new SqlConnection(sqlConnectionString))
                {
                    command.Connection = localConn;
                    localConn.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        table.Load(dr);
                        return table;
                    }
                }
            }
            using (SqlDataReader dr = command.ExecuteReader())
            {
                table.Load(dr);
                return table;
            }
        }
        public static int ExecuteNon(this SqlCommand command, SqlTransaction transaction = null)
        {
            command.Connection = connection;
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            if (transaction != null)
                command.Transaction = transaction;
            return command.ExecuteNonQuery();
        }

        public static int ExecuteNon(this List<SqlCommand> commandList, SqlTransaction transaction)
        {
            foreach (SqlCommand command in commandList)
            {
                command.Connection = connection;
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                if (transaction != null)
                    command.Transaction = transaction;
                command.ExecuteNonQuery();
            }
            return 1;
        }

        public static dynamic ExecuteNScalar(this SqlCommand command, SqlTransaction transaction = null)
        {
            command.Connection = connection;
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            if (transaction != null)
                command.Transaction = transaction;
            return command.ExecuteScalar();
        }
        public static dynamic ExecuteNScalar(this List<SqlCommand> commands, SqlTransaction transaction = null)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            foreach (SqlCommand command in commands)
            {
                command.Connection = connection;
                if (transaction != null)
                    command.Transaction = transaction;
                command.ExecuteScalar();
            }
            return 0;
        }

        public static SqlTransaction BeginTransaction()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.OpenAsync();
            return connection.BeginTransaction();
        }

        public static void ConnectionClose()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

    }
}
