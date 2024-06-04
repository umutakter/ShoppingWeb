using CoreLibrary.Logging;
using log4net;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace CoreLibrary.DbCore
{
    public static class DbExecutor
    {
        private static readonly ILog log = Logger.GetLogger(typeof(DbExecutor));
        public static List<T> SelectExecutor<T>(DbCommand command)
        {
            Type type = typeof(T);
            List<T> resultList = new List<T>();

            try
            {
                using (var connection = new SqlConnection(BaseSettings.ConnectionString))
                {
                    command.Connection = connection;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        T model = (T?)Activator.CreateInstance(typeof(T)) ?? throw new InvalidOperationException("Cannot create instance of type " + typeof(T).Name);
                        foreach (PropertyInfo prop in type.GetProperties())
                        {
                            if (reader[prop.Name] != DBNull.Value)
                                prop.SetValue(model, Convert.ChangeType(reader[prop.Name], prop.PropertyType));
                        }
                        resultList.Add(model);
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Error($"CoreLibrary:DbExecutor::SelectExecutor::Error occured.", ex);
                throw new Exception($"CoreLibrary:DbExecutor::SelectExecutor::Error occured.", ex);
            }

            return resultList;
        }
        public static int InsertExecutor<T>(DbCommand command)
        {
            try
            {
                using (var connection = new SqlConnection(BaseSettings.ConnectionString))
                {
                    command.Connection = connection;
                    connection.Open();
                    return (int)command.ExecuteScalar()!;
                }
            }
            catch (SqlException ex)
            {
                log.Error($"CoreLibrary:DbExecutor::InsertExecutor::Error occured.", ex);
                throw new Exception($"CoreLibrary:DbExecutor::InsertExecutor::Error occured.", ex);
            }
        }
        public static bool UpdateExecutor(DbCommand command)
        {
            try
            {
                using (var connection = new SqlConnection(BaseSettings.ConnectionString))
                {
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException ex)
            {
                log.Error($"CoreLibrary:DbExecutor::UpdateExecutor::Error occured.", ex);
                throw new Exception($"CoreLibrary:DbExecutor::UpdateExecutor::Error occured.", ex);
            }
        }
        public static bool DeleteExecutor(DbCommand command)
        {
            try
            {
                using (var connection = new SqlConnection(BaseSettings.ConnectionString))
                {
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                log.Error($"CoreLibrary:DbExecutor::DeleteExecutor::Error occured.", ex);
                throw new Exception($"CoreLibrary:DbExecutor::DeleteExecutor::Error occured.", ex);
            }
            return true;
        }
    }
}
