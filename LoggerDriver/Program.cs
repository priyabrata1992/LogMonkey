using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMonkey;
using LogMonkey.ComponentImpl;
using System.Data.SqlClient;

namespace LoggerDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection();

            //Build the configuration
            LoggerConfiguration configuration = LoggerConfiguration
               .Builder()
               .SetPrimaryLoggingMode(LogMode.Database)
               //.SetFallbackLoggingMode(LogMode.File)
               .SetDatabaseConnection(connection)
               .SetLogInnerException(true)
               //.SetFilePath("")
               .Build();

            //Build the alternate configurationhh
            LoggerConfiguration alternateConfiguration = LoggerConfiguration
               .Builder()
               .SetPrimaryLoggingMode(LogMode.Database)
               //.SetFallbackLoggingMode(LogMode.File)
               .SetDatabaseConnection(connection)
               .SetLogInnerException(false)
               //.SetFilePath("")
               .Build();

            //Initialization of the logger
            Logger.Initialize(configuration, alternateConfiguration);
            Logger logger = Logger.Instance();

            logger.SwitchConfiguration();

            try {
                throw new DivideByZeroException();
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Exception happended for input "+ "Some input");
            }
            Console.ReadKey();
        }
    }
}
