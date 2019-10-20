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
        public static String NoDatabaseConnectionProvidedMessage = "Database connection was not provided.";
        public static String NoDatabaseConnectionProvidedForFallbackMessage = "Fallback mode set to database, database connection was not provided.";
        public static String NoFilePathProvidedMessage = "File path was not provided.";
        public static String NoFilePathProvidedForFallbackMessage = "Fallback mode set to file, file path was not provided.";
    }
}
