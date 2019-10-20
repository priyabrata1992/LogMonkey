using LogMonkey.Constants;
using LogMonkey.Exceptions;
using System;
using System.Runtime.Serialization;

namespace LogMonkey.ComponentImpl
{
    [Serializable]
    internal class NoFilePathProvidedException : LoggerException
    {
        public NoFilePathProvidedException():base(LogConstants.NoFilePathProvidedMessage)
        {
        }

        public NoFilePathProvidedException(string message) : base(message)
        {
        }

        public NoFilePathProvidedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}