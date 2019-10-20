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
            LoggerConfiguration configuration = LoggerConfiguration
               .Builder()
               .SetPrimaryLoggingMode(LogMode.Database)
               .SetFallbackLoggingMode(LogMode.File)
               .SetDatabaseConnection(connection)
               .SetFilePath("")
               .Build();
        }
    }
}
