using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawFirm.CommonUtilitis.Logging
{
    public class ErrorLogger
    {
        public static void Initialize()
        {
            XmlConfigurator.Configure();
        }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogError(string message, Exception exception, bool rethrowException)
        {
            if (message != null && message.Length > 4000)
            {
                message = message.Substring(0, 4000);//4000 maximum size of Message field in DB
            }
            log.Error(message, exception);

            if (rethrowException)
            {
                throw exception;
            }
        }


        public static void LogDebug(string message)
        {
            log.Debug(message);
        }
    }

}
