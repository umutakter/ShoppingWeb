using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Logging
{
    public static class Logger
    {
        public static string LogConfigPath;
        static Logger()
        {
            try
            {
                var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
                var logConfigFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\CoreLibrary\Logging\log4net.config"));
                LogConfigPath = logConfigFile.FullName;
                XmlConfigurator.Configure(logRepository, logConfigFile);
                Console.WriteLine("log4net configuration loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error configuring log4net: {ex.Message}");
            }
        }

        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }
    }
}
