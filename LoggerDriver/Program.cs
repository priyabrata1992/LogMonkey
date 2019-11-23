using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMonkey;
using LogMonkey.ComponentImpl;
using System.Data.SqlClient;
using LogMonkey.Database;
using LogMonkey.Constants;

namespace LoggerDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dummy SqlConnection, to be replaced by an actual initialized connection for use.
            String sqlConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=workdb;Integrated Security=True";

            //Build the configuration
            LoggerConfiguration configuration = LoggerConfiguration
               .Builder()
               .SetPrimaryLoggingMode(LogMode.Database)
               .SetFallbackLoggingMode(LogMode.File)
               .SetDatabaseConnectionString(sqlConnectionString)
               .SetLogInnerException(true)
               .SetFilePath("D://")
               .Build();

            //Build the alternate configuration
            LoggerConfiguration alternateConfiguration = LoggerConfiguration
               .Builder()
               .SetPrimaryLoggingMode(LogMode.File)
               .SetFallbackLoggingMode(LogMode.Database)
               .SetDatabaseConnectionString(sqlConnectionString)
               .SetLogInnerException(false)
               .SetFilePath("C://")
               .Build();

            //Initialization of the logger
            Logger.Initialize(configuration, alternateConfiguration);

            //Get a singleton instance.
            Logger logger = Logger.Instance();

            //switch logger configurations.
            logger.SwitchConfiguration();

            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                logger.Write(LogType.Information, ex, "test", "Main", "Additional information");
            }

            //Make the console wait.
            Console.ReadKey();
        }
    }
}
