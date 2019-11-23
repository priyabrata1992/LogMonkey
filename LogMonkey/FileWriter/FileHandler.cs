using LogMonkey.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey.FileWriter
{
    internal class FileHandler
    {
        private static string filePath = "";

        public static void SetFilePath(String path)
        {
            filePath = path;
        }

        public static void WriteExceptionToFile(
            String exception,
            String innerException,
            String stackTrace,
            String exceptionMethod,
            String exceptionClass,
            DateTime dateTime,
            LogType type,
            String extraInformation)
        {
            var fileName = filePath + "/" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".csv";
            var contents = exception.Replace('\n', ' ').Replace('\r', ' ').Replace(',', ' ')
                + "," + innerException.Replace('\n', ' ').Replace('\r', ' ').Replace(',', ' ')
                + "," + stackTrace.Replace('\n', ' ').Replace('\r', ' ').Replace(',', ' ')
                + "," + exceptionMethod + "," + exceptionClass + "," 
                + dateTime.ToString() + "," 
                + LogTypeUtils.GetType(type) 
                + "," + extraInformation + "\n";
            try
            {
                if (File.Exists(fileName))
                {
                    File.AppendAllText(fileName, contents);
                }
                else
                {
                    File.AppendAllText(fileName, "Exception,Inner Exception,Stack Trace,Exception Method,Exception Class, Date Time,Type,Extra Information\n");
                    File.AppendAllText(fileName, contents);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
