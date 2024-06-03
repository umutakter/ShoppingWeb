using CoreLibrary.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class BaseRepository<T> : IBaseRepository<T> where T : CoreDbModel
    {
        private static readonly ILog log = Logger.GetLogger(typeof(BaseRepository<T>));
        private readonly string connectionString;
        internal DbConnection? connection;
        private SqlConnection GetConnection()//BU FONKSİYONA GEREK KALMAMALI!!!!!
        {
            return new SqlConnection(connectionString);
        }
        public BaseRepository()
        {
            this.connectionString = BaseSettings.ConnectionString;
            connection = new SqlConnection(connectionString);
        }
        public void Dispose()
        {
            try
            {
                if (connection != null)
                    connection.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region GENERIC CRUD OPERATIONS
        public bool Update(T model)
        {
            try
            {
                string columnSET = "", columnID = "";
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();
                Dictionary<string, object> columns = new Dictionary<string, object>();
                int ID = 0;

                string tableName = type.GetField("TABLE_NAME", BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!.ToString()!;
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttributes(true).FirstOrDefault()!.GetType().Name != "IgnoreSQLAttribute")
                    {
                        string propertyName = property.Name;
                        columnSET += $", {propertyName} = @{propertyName}";
                        columns.Add(propertyName, property.GetValue(model)!);
                    }
                    else
                    {
                        columnID = property.Name;
                        ID = (int)property.GetValue(model)!;
                    }
                }
                columnSET = columnSET.Remove(0, 1);
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand($"UPDATE {tableName} SET {columnSET} WHERE {columnID} = {ID}", (SqlConnection)conn))
                    {
                        foreach (var key in columns.Keys)
                        {
                            object value = columns[key];
                            command.Parameters.AddWithValue($"@{key}", value);
                        }
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::Update::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::Update::Error occured.", ex);
            }

        }
        public bool Insert(T model)
        {
            try
            {
                string columnINTO = "", columnVALUES = "";
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();
                Dictionary<string, object> columns = new Dictionary<string, object>();

                string tableName = type.GetField("TABLE_NAME", BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!.ToString()!;
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttributes(true).FirstOrDefault()!.GetType().Name != "IgnoreSQLAttribute")
                    {
                        string propertyName = property.Name;
                        columnINTO += ", " + propertyName;
                        columnVALUES += ", @" + propertyName;
                        columns.Add(propertyName, property.GetValue(model)!);
                    }
                }
                columnINTO = columnINTO.Remove(0, 1);
                columnVALUES = columnVALUES.Remove(0, 1);
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand($"INSERT INTO {tableName} ({columnINTO}) VALUES ({columnVALUES})", (SqlConnection)conn))
                    {
                        foreach (var key in columns.Keys)
                        {
                            object value = columns[key];
                            command.Parameters.AddWithValue($"@{key}", value);
                        }
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::Insert::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::Insert::Error occured.", ex);
            }
        }//todo: insert edildiğinde id dönmeli.
        public List<T> SelectAll()
        {
            try
            {
                Type type = typeof(T);
                string tableName = type.GetField("TABLE_NAME", BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!.ToString()!;
                List<T> resultList = new List<T>();
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName}", (SqlConnection)conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                T model = (T)Activator.CreateInstance(typeof(T))!;
                                foreach (PropertyInfo prop in type.GetProperties())
                                {
                                    if (reader[prop.Name] != DBNull.Value)
                                    {
                                        prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType));
                                    }
                                }
                                resultList.Add(model);
                            }
                        }
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::SelectAll::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::SelectAll::Error occured.", ex);
            }
        }
        #endregion
    }
}
