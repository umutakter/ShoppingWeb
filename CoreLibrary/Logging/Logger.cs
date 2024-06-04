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
            var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
            var logConfigFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\CoreLibrary\Logging\log4net.config"));
            LogConfigPath = logConfigFile.FullName;
            XmlConfigurator.Configure(logRepository, logConfigFile);
        }

        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }
    }
}
