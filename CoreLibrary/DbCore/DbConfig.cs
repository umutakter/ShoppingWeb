using System.Data.SqlClient;

namespace CoreLibrary.DbCore
{
    public class DbCoreConfig
    {
        public static void SetConfig(Action<DbConfig> configure)
        {
            DbConfig config = new DbConfig();
            configure(config);
        }
    }
    public class DbConfig
    {
        public void SetDbConnection(Action<DatabaseSettings> dbConfig)
        {
            DatabaseSettings ds = new DatabaseSettings();
            dbConfig(ds);
            BaseDbSettings.ConnectionString = ds.ConnectionString;
        }
    }
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
    }
    public static class BaseDbSettings
    {
        public static string ConnectionString { get; set; }
        public static SqlConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
            set { }
        }
    }
}