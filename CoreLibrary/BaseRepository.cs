using CoreLibrary.Logging;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public static class DbHelpers
    {
        public static string GetTableName<T>()
        {
            Type type = typeof(T);
            return type.GetField("TABLE_NAME", BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!.ToString()!;
        }
    }
    public static class DbCommandGenerator
    {
        private static readonly ILog log = Logger.GetLogger(typeof(DbCommandGenerator));

        public static DbCommand GetSelectAllCommand<T>()
        {
            string tableName = DbHelpers.GetTableName<T>();
            var query =  $"SELECT * FROM {tableName}";
            var connection = BaseSettings.Connection;
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            return command;
        }
        public static DbCommand GetSelectByIdCommand<T>(int id)
        {
            string keyColumn = "";
            Type type = typeof(T);
            foreach (PropertyInfo property in type.GetProperties())
                if (property.GetCustomAttributes(true).FirstOrDefault()!.GetType().Name == "IgnoreSQLAttribute")
                    keyColumn = property.Name;

            if (keyColumn == "")
                log.Info($"CoreLibrary:DbCommandGenerator::GetSelectByIdCommand::KEY ALANI BULUNAMADI.");
            string tableName = DbHelpers.GetTableName<T>();

            var query = $"SELECT * FROM {tableName} WHERE {keyColumn} = {id}";
            var connection = BaseSettings.Connection;
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            return command;
        }
        public static DbCommand GetInsertCommand<T>(T model)
        {
            string columnINTO = "", columnVALUES = "";
            Type type = typeof(T);
            Dictionary<string, object> columns = new Dictionary<string, object>();

            foreach (PropertyInfo property in type.GetProperties())
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

            string tableName = DbHelpers.GetTableName<T>();
            var query = $"INSERT INTO {tableName} ({columnINTO}) VALUES ({columnVALUES})";

            var connection = BaseSettings.Connection;
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            foreach (var key in columns.Keys)
            {
                object value = columns[key];
                command.Parameters.AddWithValue($"@{key}", value);
            }
            return command;
        }
    }
    public static class DbExecutor
    {
        private static readonly ILog log = Logger.GetLogger(typeof(DbExecutor));

        public static List<T> SelectExecutor<T>(DbCommand command)
        {
            Type type = typeof(T);
            List<T> resultList = new List<T>();

            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    T model = (T)Activator.CreateInstance(typeof(T))!;
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        if (reader[prop.Name] != DBNull.Value)
                            prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType));
                    }
                    resultList.Add(model);
                }
            }
            catch (SqlException ex)
            {
                log.Error($"CoreLibrary:DbExecutor::SelectExecutor::Error occured.", ex);
                throw new Exception($"CoreLibrary:DbExecutor::SelectExecutor::Error occured.", ex);
            }
            
            return resultList;
        }
        public static bool InsertExecutor<T>(DbCommand command)
        {
            Type type = typeof(T);
            try
            {
                command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                log.Error($"CoreLibrary:DbExecutor::InsertExecutor::Error occured.", ex);
                throw new Exception($"CoreLibrary:DbExecutor::InsertExecutor::Error occured.", ex);
            }
            return true;
        }
    }


    public class BaseRepository<T> : IBaseRepository<T> where T : CoreDbModel
    {
        private static readonly ILog log = Logger.GetLogger(typeof(BaseRepository<T>));
        private readonly string connectionString;
        protected DbConnection? connection;
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
                using (var command = DbCommandGenerator.GetInsertCommand(model))
                {
                    DbExecutor.InsertExecutor<T>(command);
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
                return DbExecutor.SelectExecutor<T>(DbCommandGenerator.GetSelectAllCommand<T>());
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::SelectAll::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::SelectAll::Error occured.", ex);
            }
        }
        public T SelectById(int id)
        {
            try
            {
                return DbExecutor.SelectExecutor<T>(DbCommandGenerator.GetSelectByIdCommand<T>(id)).FirstOrDefault()!;
            }
            catch (Exception ex)
            {
                log.Error($"CoreLibrary:BaseRepository::{nameof(T)}::SelectById::Error occured.", ex);
                throw new Exception($"CoreLibrary:BaseRepository::{nameof(T)}::SelectById::Error occured.", ex);
            }
        }
        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
