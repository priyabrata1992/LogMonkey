using LogMonkey.Constants;
using LogMonkey.Exceptions;
using System;
using System.Runtime.Serialization;

namespace LogMonkey
{
    [Serializable]
    internal class LoggerNotInitializedException : LoggerException
    {
        public LoggerNotInitializedException():base(LogConstants.LoggerNotInitializedMessage)
        {
        }

        public LoggerNotInitializedException(string message) : base(message)
        {
        }

        public LoggerNotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}