using LogMonkey.Constants;
using LogMonkey.Exceptions;
using System;
using System.Runtime.Serialization;

namespace LogMonkey.ComponentImpl
{
    [Serializable]
    internal class NoDatabaseConnectionProvidedException : LoggerException
    {
        public NoDatabaseConnectionProvidedException():base(LogConstants.NoDatabaseConnectionProvidedMessage)
        {
        }

        public NoDatabaseConnectionProvidedException(string message) : base(message)
        {
        }

        public NoDatabaseConnectionProvidedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}