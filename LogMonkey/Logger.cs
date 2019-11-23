using LogMonkey.ComponentImpl;
using LogMonkey.Constants;
using LogMonkey.Database;
using LogMonkey.FileWriter;
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
            if ((sActiveLoggerConfiguration.PrimaryLoggingMode == LogMode.Database
                || sActiveLoggerConfiguration.FallbackLoggingMode == LogMode.Database)
                && String.IsNullOrEmpty(sActiveLoggerConfiguration.SqlConnectionString))
            {
                throw new NoDatabaseConnectionStringProvidedException();
            }
            else
            {
                DBHandler.SetConnectionString(sActiveLoggerConfiguration.SqlConnectionString);
            }
            if ((sActiveLoggerConfiguration.PrimaryLoggingMode == LogMode.File
                || sActiveLoggerConfiguration.FallbackLoggingMode == LogMode.File)
                && String.IsNullOrEmpty(sActiveLoggerConfiguration.FilePath))
            {
                throw new NoFilePathProvidedException();
            }
            else
            {
                FileHandler.SetFilePath(sActiveLoggerConfiguration.FilePath);
            }
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
            if ((sActiveLoggerConfiguration.PrimaryLoggingMode == LogMode.Database 
                || sActiveLoggerConfiguration.FallbackLoggingMode == LogMode.Database) 
                && String.IsNullOrEmpty(sActiveLoggerConfiguration.SqlConnectionString))
            {
                throw new NoDatabaseConnectionStringProvidedException();
            }
            DBHandler.SetConnectionString(sActiveLoggerConfiguration.SqlConnectionString);

            if ((sActiveLoggerConfiguration.PrimaryLoggingMode == LogMode.File
                || sActiveLoggerConfiguration.FallbackLoggingMode == LogMode.File)
                && String.IsNullOrEmpty(sActiveLoggerConfiguration.FilePath))
            {
                throw new NoFilePathProvidedException();
            }
            else
            {
                FileHandler.SetFilePath(sActiveLoggerConfiguration.FilePath);
            }

            return useAlternateConfiguration;
        }

        public void Write(LogType type, Exception ex, String methodName = null, String className = null, String extraInformation = null)
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
            switch (sActiveLoggerConfiguration.PrimaryLoggingMode)
            {
                case LogMode.Database:
                     {
                        try
                        {
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
                        catch(Exception e)
                        {
                            //Primary logging to db has failed.
                            //write the current exception to secondary mode and then write the original exception to secondary mode
                            switch(sActiveLoggerConfiguration.FallbackLoggingMode)
                            {
                                case LogMode.File:
                                    {

                                        FileHandler.WriteExceptionToFile(
                                            e.ToString(),
                                            sActiveLoggerConfiguration.LogInnerException == true? (e.InnerException!= null ? (e.InnerException.ToString()): "NULL") : "NULL",
                                            e.StackTrace,
                                            "WriteExceptionToDatabase",
                                            "DBHandler",
                                            DateTime.Now,
                                            LogType.WTF,
                                            LogConstants.FallbackModeUsed
                                        );

                                        FileHandler.WriteExceptionToFile(
                                            exceptionString,
                                            innerExceptionString,
                                            ex.StackTrace,
                                            String.IsNullOrEmpty(methodName) ? "NULL" : methodName,
                                            String.IsNullOrEmpty(className) ? "NULL" : className,
                                            DateTime.Now,
                                            type,
                                            String.IsNullOrEmpty(extraInformation) ? "NULL" : extraInformation
                                        );
                                        break;
                                    }
                                case LogMode.None:
                                    break;
                            }
                        }
                    }
                    break;
                case LogMode.File:
                    {
                        try
                        {
                            FileHandler.WriteExceptionToFile(
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
                        catch(Exception e)
                        {
                            //Primary logging to file has failed.
                            //write the current exception to secondary mode and then write the original exception to secondary mode
                            switch (sActiveLoggerConfiguration.FallbackLoggingMode)
                            {
                                case LogMode.Database:
                                    {
                                        DBHandler.WriteExceptionToDatabase(
                                            e.ToString(),
                                            sActiveLoggerConfiguration.LogInnerException == true ? (e.InnerException != null ? (e.InnerException.ToString()) : "NULL") : "NULL",
                                            e.StackTrace,
                                            "WriteExceptionToDatabase",
                                            "DBHandler",
                                            DateTime.Now,
                                            LogType.WTF,
                                            LogConstants.FallbackModeUsed
                                        );

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
                                    break;
                                case LogMode.None:
                                    break;
                            }
                        }
                    }
                    break;
                case LogMode.None:
                    break;
            }
        }
    }
}
