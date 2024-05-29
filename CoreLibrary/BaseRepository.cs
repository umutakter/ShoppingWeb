using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class BaseRepository<T> where T : class
    {
        private readonly string connectionString;
        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
        public BaseRepository()
        {
            this.connectionString = BaseSettings.ConnectionString;
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
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"UPDATE {tableName} SET {columnSET} WHERE {columnID} = {ID}", connection))
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

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"INSERT INTO {tableName} ({columnINTO}) VALUES ({columnVALUES})", connection))
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
        }//todo: insert edildiğinde id dönmeli.
        public List<T> SelectAll<T>() where T : new()
        {
            Type type = typeof(T);
            string tableName = type.GetField("TABLE_NAME", BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!.ToString()!;
            List<T> resultList = new List<T>();
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                T model = new T();
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
            }
            return resultList;
        }
    }
}
