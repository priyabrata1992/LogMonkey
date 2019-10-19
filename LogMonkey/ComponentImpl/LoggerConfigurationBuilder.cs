using System;
using LogMonkey.Components;

namespace LogMonkey.ComponentImpl
{
    public class LoggerConfigurationBuilder:LoggerConfiguration
    {
        public static LoggerConfiguration Builder()
        {
            return LoggerConfiguration.Instance();
        }
    }

}

