namespace CoreLibrary
{
    public class DbCoreConfig
    {
        public static void SetConfig(Action<Config> configure)
        {
            Config config = new Config();
            configure(config);
        }
    }
    public class Config
    {
        public void SetDbConnection(Action<DatabaseSettings> dbConfig)
        {
            DatabaseSettings ds = new DatabaseSettings();
            dbConfig(ds);
            BaseSettings.ConnectionString = ds.ConnectionString;
        }
    }
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
    }
    public static class BaseSettings
    {
        public static string ConnectionString { get; set; }
    }
}