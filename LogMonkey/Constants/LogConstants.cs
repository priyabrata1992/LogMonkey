using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey.Constants
{
    class LogConstants
    {
        public static String PrimaryLoggingSchemeMissingMessage = "Primary logging scheme cannot be LogMode.None";
        public static String NoDatabaseConnectionStringProvidedMessage = "Database connection string was not provided.";
        public static String NoDatabaseConnectionStringProvidedForFallbackMessage = "Fallback mode set to database, database connection string was not provided.";
        public static String NoFilePathProvidedMessage = "File path was not provided.";
        public static String NoFilePathProvidedForFallbackMessage = "Fallback mode set to file, file path was not provided.";
        public static String LoggerNotInitializedMessage = "Logger has not been initialized. Please initialize with LoggerConfiguration before getting instance.";
    }
}
