using LogMonkey.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey.Exceptions
{
    [Serializable]
    public class PrimaryLoggingModeMissingException : LoggerException
    {

        public PrimaryLoggingModeMissingException():base(LogConstants.PrimaryLoggingSchemeMissingMessage)
        {
        }

        public PrimaryLoggingModeMissingException(String message) : base(message)
        {
        }

        public PrimaryLoggingModeMissingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
