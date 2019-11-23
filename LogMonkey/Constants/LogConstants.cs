using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey.Constants
{
    class LogConstants
    {
        internal static String PrimaryLoggingSchemeMissingMessage = "Primary logging scheme cannot be LogMode.None";
        internal static String NoDatabaseConnectionStringProvidedMessage = "Database connection string was not provided.";
        internal static String NoDatabaseConnectionStringProvidedForFallbackMessage = "Fallback mode set to database, database connection string was not provided.";
        internal static String NoFilePathProvidedMessage = "File path was not provided.";
        internal static String NoFilePathProvidedForFallbackMessage = "Fallback mode set to file, file path was not provided.";
        internal static String LoggerNotInitializedMessage = "Logger has not been initialized. Please initialize with LoggerConfiguration before getting instance.";
        internal static String PrimaryAndFallbackModeCannotBeSame = "Primary and fallback schemes cannot be the same for a single configuration.";
        internal static String FallbackModeUsed = "Your primary logging scheme has failed - fallback has been used.";
    }
}
