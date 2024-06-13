using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Authorization.SecretKeySettings
{
    public class AuthCoreConfig
    {
        public static void SetConfig(Action<AuthConfig> configure)
        {
            AuthConfig config = new AuthConfig();
            configure(config);
        }
    }
    public class AuthConfig
    {
        public void SetSecretKey(Action<AuthSettings> dbConfig)
        {
            AuthSettings ds = new AuthSettings();
            dbConfig(ds);
            BaseAuthSettings.SecretKey = ds.SecretKeys;
        }
    }
    public class AuthSettings
    {
        public string SecretKeys { get; set; }
    }
    public static class BaseAuthSettings
    {
        public static string SecretKey { get; set; }
    }
}
