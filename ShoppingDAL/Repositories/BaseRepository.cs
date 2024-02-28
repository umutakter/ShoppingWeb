using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingDAL.Repositories
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
            this.connectionString = "Server=DESKTOP-VPPU1BG;Database=ShoppingDb;Integrated Security=True;";
        }

        public bool Insert(T model)
        {
            string columnINTO = "", columnVALUES = "";
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, object> columns = new Dictionary<string, object>();

            string tableName  = type.GetField("TABLE_NAME", BindingFlags.Public | BindingFlags.Static).GetValue(null).ToString();
            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttributes(true).FirstOrDefault().GetType().Name != "IgnoreSQLAttribute")
                {
                    string propertyName = property.Name;
                    columnINTO += ", " + propertyName;
                    columnVALUES += ", @" + propertyName;
                    columns.Add(propertyName, property.GetValue(model));
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
        }
    }
}
