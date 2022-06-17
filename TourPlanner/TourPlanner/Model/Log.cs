using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;


namespace TourPlanner.Model
{
    public class Log
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void LogError(String message)
        {
            log.Error(message);
        }
        public static void LogInfo(String message)
        {
            log.Info(message);
        }
        public static void LogFatal(String message)
        {
            log.Fatal(message);
        }
        public static void LogDebug(String message)
        {
            log.Debug(message);
        }

    }
}
