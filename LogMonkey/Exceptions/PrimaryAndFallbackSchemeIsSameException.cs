using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey.Exceptions
{
    class PrimaryAndFallbackSchemeIsSameException : LoggerException
    {
        public PrimaryAndFallbackSchemeIsSameException(string message) : base(message)
        {
        }

        public PrimaryAndFallbackSchemeIsSameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
