using CoreLibrary.Logging;
using log4net;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace CoreLibrary.DbCore
{
    public static class DbCommandGenerator
    {
        private static readonly ILog log = Logger.GetLogger(typeof(DbCommandGenerator));
        public static DbCommand GetSelectAllCommand<T>()
        {
            string tableName = DbHelpers.GetTableName<T>();
            var query = $"SELECT * FROM {tableName}";
            SqlCommand command = new SqlCommand(query);
            return command;
        }
        public static DbCommand GetSelectByIdCommand<T>(int id)
        {
            string keyColumn = "";
            Type type = typeof(T);
            foreach (PropertyInfo property in type.GetProperties())
            {
                var attribute = property.GetCustomAttributes(true).FirstOrDefault();
                if (attribute != null && attribute.GetType().Name == "CoreKeyAttribute")
                    keyColumn = property.Name;
            }

            if (keyColumn == "")
                log.Info($"CoreLibrary:DbCommandGenerator::GetSelectByIdCommand::KEY ALANI BULUNAMADI.");
            string tableName = DbHelpers.GetTableName<T>();

            var query = $"SELECT * FROM {tableName} WHERE {keyColumn} = {id}";
            SqlCommand command = new SqlCommand(query);
            return command;
        }
        public static DbCommand GetInsertCommand<T>(T model)
        {
            string columnINTO = "", columnVALUES = "";
            Type type = typeof(T);
            Dictionary<string, object> columns = new Dictionary<string, object>();

            foreach (PropertyInfo property in type.GetProperties())
            {
                var attribute = property.GetCustomAttributes(true).FirstOrDefault();
                if (attribute == null || attribute.GetType().Name != "CoreKeyAttribute")
                {
                    string propertyName = property.Name;
                    columnINTO += ", " + propertyName;
                    columnVALUES += ", @" + propertyName;
                    columns.Add(propertyName, property.GetValue(model)!);
                }
            }
            columnINTO = columnINTO.TrimStart(',').Trim();
            columnVALUES = columnVALUES.TrimStart(',').Trim();

            string tableName = DbHelpers.GetTableName<T>();
            var query = $"INSERT INTO {tableName} ({columnINTO}) OUTPUT INSERTED.ID VALUES ({columnVALUES})";

            var command = new SqlCommand(query);
            foreach (var key in columns.Keys)
            {
                object value = columns[key];
                command.Parameters.AddWithValue($"@{key}", value);
            }
            return command;
        }
        public static DbCommand GetUpdateCommand<T>(T model)
        {
            string columnSET = "", columnID = "";
            Type type = typeof(T);
            Dictionary<string, object> columns = new Dictionary<string, object>();
            int id = 0;
            foreach (PropertyInfo property in type.GetProperties())
            {
                var attribute = property.GetCustomAttributes(true).FirstOrDefault();
                if (attribute != null && attribute.GetType().Name == "CoreKeyAttribute")
                {
                    columnID = property.Name;
                    id = (int)property.GetValue(model)!;
                }
                else
                {
                    string propertyName = property.Name;
                    columnSET += $", {propertyName} = @{propertyName}";
                    columns.Add(propertyName, property.GetValue(model)!);
                }
            }
            columnSET = columnSET.Remove(0, 1);
            string tableName = DbHelpers.GetTableName<T>();
            var query = $"UPDATE {tableName} SET {columnSET} WHERE {columnID} = {id}";
            SqlCommand command = new SqlCommand(query);

            foreach (var key in columns.Keys)
            {
                object value = columns[key];
                command.Parameters.AddWithValue($"@{key}", value);
            }
            return command;
        }
        public static DbCommand GetDeleteByIdCommand<T>(int id)
        {
            string keyColumn = "";
            Type type = typeof(T);
            foreach (PropertyInfo property in type.GetProperties())
            {
                var attribute = property.GetCustomAttributes(true).FirstOrDefault();
                if (attribute != null && attribute.GetType().Name == "CoreKeyAttribute")
                {
                    keyColumn = property.Name;
                    break;
                }
            }

            if (string.IsNullOrEmpty(keyColumn))
                log.Info($"CoreLibrary:DbCommandGenerator::GetDeleteByIdCommand::KEY ALANI BULUNAMADI.");

            string tableName = DbHelpers.GetTableName<T>();
            var query = $"DELETE FROM {tableName} WHERE {keyColumn} = @id";
            var command = new SqlCommand(query);
            command.Parameters.AddWithValue("@id", id);
            return command;
        }
        public static DbCommand GetSelectAllExceptKeyCommand<T>(T model)
        {
            Type type = typeof(T);
            Dictionary<string, object> columns = new Dictionary<string, object>();
            string keyColumn = "";
            string whereClause = "";

            foreach (PropertyInfo property in type.GetProperties())
            {
                var attribute = property.GetCustomAttributes(true).FirstOrDefault();
                string propertyName = property.Name;
                if (attribute != null && attribute.GetType().Name == "CoreKeyAttribute")
                {
                    keyColumn = propertyName;
                }
                else
                {
                    columns.Add(propertyName, property.GetValue(model)!);
                    whereClause += $" AND {propertyName} = @{propertyName}";
                }
            }

            if (string.IsNullOrEmpty(keyColumn))
            {
                throw new InvalidOperationException("Key column not found.");
            }

            whereClause = whereClause.TrimStart(" AND".ToCharArray());

            string tableName = DbHelpers.GetTableName<T>();
            var query = $"SELECT * FROM {tableName} WHERE {whereClause}";

            var command = new SqlCommand(query);
            foreach (var key in columns.Keys)
            {
                object value = columns[key];
                command.Parameters.AddWithValue($"@{key}", value);
            }
            return command;
        }
    }
}
