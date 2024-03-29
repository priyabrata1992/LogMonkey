﻿using LogMonkey.Constants;
using LogMonkey.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey.ComponentImpl
{
    public class LoggerConfiguration
    {
        //Primary mode of logging to be used by the logger.
        private LogMode mPrimaryLoggingMode = LogMode.None;
        public LogMode PrimaryLoggingMode { get => mPrimaryLoggingMode; }
        
        //Secondary/fallback mode of logging to be used by the logger, if primary mode fails.
        private LogMode mFallbackLoggingMode = LogMode.None;
        public LogMode FallbackLoggingMode { get => mFallbackLoggingMode; }

        //Whether to send a notification mailer when the primary mode fails, and fallback is executed.
        private bool sendMailerNotificationOnFallback;
        public bool SendMailerNotificationOnFallback { get => sendMailerNotificationOnFallback; }

        //SqlConnection instance to be used for writing logs to database.
        private String sqlConnectionString;
        public String SqlConnectionString { get => sqlConnectionString; }

        //File path where logs are to be written.
        private String filePath;
        public string FilePath { get => filePath; }

        //Flag to decide if InnerExceptions should be logged or not.
        private bool logInnerException;
        public bool LogInnerException { get => logInnerException; }

        //Empty private constructor - we don't want multiple instances of this class.
        protected LoggerConfiguration()
        {
        }

        //Returns an instance of LoggerConfiguration
        private static LoggerConfiguration Instance()
        {
            return new LoggerConfiguration();
        }

        //Helper for the builder pattern
        public static LoggerConfiguration Builder()
        {
            return Instance();
        }

        //Setter for the fallback mode.
        public LoggerConfiguration SetFallbackLoggingMode(LogMode mode)
        {
            if (mode == mPrimaryLoggingMode)
            {
                throw new PrimaryAndFallbackSchemeIsSameException(LogConstants.PrimaryAndFallbackModeCannotBeSame);
            }
            //this.sendMailerNotificationOnFallback = sendMailerNotification;
            this.mFallbackLoggingMode = mode;
            return this;
        }

        //Setter for the Primary mode.
        public LoggerConfiguration SetPrimaryLoggingMode(LogMode mode)
        {
            if (mode == mFallbackLoggingMode)
            {
                throw new PrimaryAndFallbackSchemeIsSameException(LogConstants.PrimaryAndFallbackModeCannotBeSame);
            }
            this.mPrimaryLoggingMode = mode;
            return this;
        }

        //Setting for the database connection.
        public LoggerConfiguration SetDatabaseConnectionString(String sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
            return this;
        }

        //Setter for the file path.
        public LoggerConfiguration SetFilePath(String filePath)
        {
            this.filePath = filePath;
            return this;
        }

        //Enable logging of inner exception.
        public LoggerConfiguration SetLogInnerException(bool logInnerException)
        {
            this.logInnerException = logInnerException;
            return this;
        }

        //Validate the configuration & return the final instance.
        public LoggerConfiguration Build()
        {
            CheckIfPrimaryLoggingModeIsSet();
            CheckIfFallbackLoggingModeIsSet();
            return this;
        }

        //Check if primary logging mode is present in the configuration.
        private void CheckIfPrimaryLoggingModeIsSet()
        {
            if (mPrimaryLoggingMode == LogMode.None)
            {
                throw new PrimaryLoggingModeMissingException();
            }
            else
            {
                switch (mPrimaryLoggingMode)
                {
                    case LogMode.Database:
                        CheckIfSqlConnectionIsSet(LogConstants.NoDatabaseConnectionStringProvidedMessage);
                        break;
                    case LogMode.File:
                        CheckIfFilePathIsSet(LogConstants.NoFilePathProvidedMessage);
                        break;
                }
            }
        }

        //Helper to check if SqlConnection object is present.
        private void CheckIfSqlConnectionIsSet(String exceptionMessage)
        {
            if (sqlConnectionString == null)
            {
                throw new NoDatabaseConnectionStringProvidedException(exceptionMessage);
            }
        }

        //Helper to check if file path is presnet.
        private void CheckIfFilePathIsSet(String exceptionMessage)
        {
            if (String.IsNullOrEmpty(this.filePath))
            {
                throw new NoFilePathProvidedException(exceptionMessage);
            }
        }

        //Check if fallback mode configuration is set.
        private void CheckIfFallbackLoggingModeIsSet()
        {
            if (mFallbackLoggingMode != LogMode.None)
            {
                switch (mFallbackLoggingMode)
                {
                    case LogMode.Database:
                        CheckIfSqlConnectionIsSet(LogConstants.NoDatabaseConnectionStringProvidedForFallbackMessage);
                        break;
                    case LogMode.File:
                        CheckIfFilePathIsSet(LogConstants.NoFilePathProvidedForFallbackMessage);
                        break;
                }
            }
        }
    }
}