using LogMonkey.Constants;
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
        //Instance, to be used for acheiving singleton patten
        private static LoggerConfiguration mConfiguration;

        //Lock object, for thread safety
        private static readonly Object mLock = new Object();

        //Primary mode of logging to be used by the logger.
        private LogMode mPrimaryLoggingMode = LogMode.None;
        public LogMode PrimaryLoggingMode { get => mPrimaryLoggingMode; }
        
        //Secondary/fallback mode of logging to be used by the logger, if primary mode fails.
        private LogMode mFallbackLoggingMode = LogMode.None;
        public LogMode MFallbackLoggingMode { get => mFallbackLoggingMode; }

        //Whether to send a notification mailer when the primary mode fails, and fallback is executed.
        private bool sendMailerNotificationOnFallback;
        public bool SendMailerNotificationOnFallback { get => sendMailerNotificationOnFallback; }

        //SqlConnection instance to be used for writing logs to database.
        private SqlConnection sqlConnection;
        public SqlConnection SqlConnection { get => sqlConnection; }

        //File path where logs are to be written.
        private String filePath;
        public string FilePath { get => filePath; }

        //Empty private constructor - we don't want multiple instances of this class.
        protected LoggerConfiguration()
        {
        }

        //Return a single re-usable instance.
        private static LoggerConfiguration Instance()
        {
            lock (mLock)
            {
                if (mConfiguration == null)
                {
                    mConfiguration = new LoggerConfiguration();
                }

                return mConfiguration;
            }
        }

        //Helper for the builder pattern
        public static LoggerConfiguration Builder()
        {
            return Instance();
        }

        //Setter for the fallback mode.
        public LoggerConfiguration SetFallbackLoggingMode(LogMode mode, bool sendMailerNotification = false)
        {
            this.sendMailerNotificationOnFallback = sendMailerNotification;
            this.mFallbackLoggingMode = mode;
            return this;
        }

        //Setter for the Primary mode.
        public LoggerConfiguration SetPrimaryLoggingMode(LogMode mode)
        {
            this.mPrimaryLoggingMode = mode;
            return this;
        }

        //Setting for the database connection.
        public LoggerConfiguration SetDatabaseConnection(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
            return this;
        }

        //Setter for the file path.
        public LoggerConfiguration SetFilePath(String filePath)
        {
            this.filePath = filePath;
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
                        CheckIfSqlConnectionIsSet(LogConstants.NoDatabaseConnectionProvidedMessage);
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
            if (sqlConnection == null)
            {
                throw new NoDatabaseConnectionProvidedException(exceptionMessage);
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
                        CheckIfSqlConnectionIsSet(LogConstants.NoDatabaseConnectionProvidedForFallbackMessage);
                        break;
                    case LogMode.File:
                        CheckIfFilePathIsSet(LogConstants.NoFilePathProvidedForFallbackMessage);
                        break;
                }
            }
        }
    }
}

