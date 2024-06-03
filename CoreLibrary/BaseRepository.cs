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
    public class BaseRepository<T> : IBaseRepository<T>
    {
        private readonly string connectionString;
        internal DbConnection? connection;
        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
        public BaseRepository()
        {
            this.connectionString = BaseSettings.ConnectionString;
            connection = new SqlConnection(connectionString);
        }

        

        public bool Update(T model)
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
            connection.Open();
            using (SqlCommand command = new SqlCommand($"UPDATE {tableName} SET {columnSET} WHERE {columnID} = {ID}", (SqlConnection)connection))
            {
                foreach (var key in columns.Keys)
                {
                    object value = columns[key];
                    command.Parameters.AddWithValue($"@{key}", value);
                }
                command.ExecuteNonQuery();
            }
            
            return true;

        }
        public bool Insert(T model)
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
            connection.Open();
            using (SqlCommand command = new SqlCommand($"INSERT INTO {tableName} ({columnINTO}) VALUES ({columnVALUES})", (SqlConnection)connection))
            {
                foreach (var key in columns.Keys)
                {
                    object value = columns[key];
                    command.Parameters.AddWithValue($"@{key}", value);
                }
                command.ExecuteNonQuery();
            }
            return true;
        }//todo: insert edildiğinde id dönmeli.
        public List<T> SelectAll()
        {
            Type type = typeof(T);
            string tableName = type.GetField("TABLE_NAME", BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!.ToString()!;
            List<T> resultList = new List<T>();
            using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName}", (SqlConnection)connection))
            {
                try
                {
                    connection.Open();
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
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata oluştu: " + ex.Message);
                }
            }
            return resultList;
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
    }
}
