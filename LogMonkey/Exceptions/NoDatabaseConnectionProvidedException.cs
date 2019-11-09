using LogMonkey.Constants;
using LogMonkey.Exceptions;
using System;
using System.Runtime.Serialization;

namespace LogMonkey.ComponentImpl
{
    [Serializable]
    internal class NoDatabaseConnectionStringProvidedException : LoggerException
    {
        public NoDatabaseConnectionStringProvidedException():base(LogConstants.NoDatabaseConnectionStringProvidedMessage)
        {
        }

        public NoDatabaseConnectionStringProvidedException(string message) : base(message)
        {
        }

        public NoDatabaseConnectionStringProvidedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}