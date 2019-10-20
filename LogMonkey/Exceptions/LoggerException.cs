using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey.Exceptions
{
    public abstract class LoggerException:Exception
    {
        public LoggerException(String message) : base(message)
        {
        }

        public LoggerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
