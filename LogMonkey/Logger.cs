using LogMonkey.ComponentImpl;
using LogMonkey.Constants;
using LogMonkey.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey
{
    public sealed class Logger
    {
        Logger() { } //private constructor

        
        private static readonly object mLock = new object();
        private static Logger MLogger;
        private static LoggerConfiguration sLoggerConfiguration;
        private static LoggerConfiguration sAlternateConfiguration;
        private static LoggerConfiguration sActiveLoggerConfiguration;
        private static bool useAlternateConfiguration;

        //Initialize Logger.
        public static void Initialize(LoggerConfiguration loggerConfiguration, LoggerConfiguration alternateConfiguration = null)
        {
            sLoggerConfiguration = loggerConfiguration;
            sAlternateConfiguration = alternateConfiguration;
            sActiveLoggerConfiguration = sLoggerConfiguration;
            useAlternateConfiguration = false;
            DBHandler.SetConnectionString(sActiveLoggerConfiguration.SqlConnectionString);
        }

        //Get a singleton instance of logger.
        public static Logger Instance()
        {
            if (sActiveLoggerConfiguration == null)
            {
                throw new LoggerNotInitializedException();
            }
            lock (mLock)
            {
                if (MLogger == null)
                {
                    MLogger = new Logger();
                }
                return MLogger;
            }
        }

        private void ToggleAlternateConfiguration()
        {
            useAlternateConfiguration = !useAlternateConfiguration;
        }

        public bool SwitchConfiguration()
        {
            ToggleAlternateConfiguration();
            sActiveLoggerConfiguration = (useAlternateConfiguration) ? sAlternateConfiguration : sLoggerConfiguration;
            DBHandler.SetConnectionString(sActiveLoggerConfiguration.SqlConnectionString);
            return useAlternateConfiguration;
        }

        public void Write(Exception ex, String methodName = null, String className = null, String extraInformation = null, LogType type = LogType.Error)
        {
            String exceptionString = "NULL";
            String innerExceptionString = "NULL";

            if (ex != null)
            {
                exceptionString = ex.ToString();
                if (sActiveLoggerConfiguration.LogInnerException)
                {
                    if (ex.InnerException != null)
                    {
                        innerExceptionString = ex.InnerException.ToString();
                    }
                    else
                    {
                        innerExceptionString = "NULL";
                    }
                }
            }
            DBHandler.WriteExceptionToDatabase(
                exceptionString,
                innerExceptionString,
                ex.StackTrace,
                String.IsNullOrEmpty(methodName) ? "NULL" : methodName,
                String.IsNullOrEmpty(className) ? "NULL" : className,
                DateTime.Now,
                type,
                String.IsNullOrEmpty(extraInformation) ? "NULL" : extraInformation
            );
        }
    }
}
