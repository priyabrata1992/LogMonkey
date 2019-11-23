using LogMonkey.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey.Database
{
    internal class DBHandler
    {
        internal static String connectionString;

        internal static void SetConnectionString(String pConnectionString)
        {
            connectionString = pConnectionString;
        }

        internal static void WriteExceptionToDatabase(
            String exception, 
            String innerException,
            String stackTrace,
            String exceptionMethod,
            String exceptionClass, 
            DateTime dateTime, 
            LogType type, 
            String extraInformation)
        {
            String _type = LogTypeUtils.GetType(type);
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"INSERT INTO LMK.LOG_HISTORY VALUES (
                                 @exception, 
                                 @innerException, 
                                 @stackTrace,
                                 @exceptionMethod, 
                                 @exceptionClass, 
                                 @exceptionDate, 
                                 @exceptionTime, 
                                 @exceptionType, 
                                 @extraInformation
                            )";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@exception", SqlDbType.NVarChar, -1).Value = exception;
                        command.Parameters.Add("@innerException", SqlDbType.NVarChar, -1).Value = innerException;
                        command.Parameters.Add("@stackTrace", SqlDbType.NVarChar, -1).Value = stackTrace;
                        command.Parameters.Add("@exceptionMethod", SqlDbType.NVarChar, 100).Value = exceptionMethod;
                        command.Parameters.Add("@exceptionClass", SqlDbType.NVarChar, 100).Value = exceptionClass;
                        command.Parameters.Add("@exceptionDate", SqlDbType.Date).Value = dateTime.Date;
                        command.Parameters.Add("@exceptionTime", SqlDbType.Time).Value = dateTime.TimeOfDay;
                        command.Parameters.Add("@exceptionType", SqlDbType.NVarChar, 30).Value = _type;
                        command.Parameters.Add("@extraInformation", SqlDbType.NVarChar, 3000).Value = extraInformation;
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
