using System;

namespace LogMonkey.Constants
{
    public enum LogType
    {
        Error,
        Warning,
        Information,
        WTF
    }

    internal class LogTypeUtils
    {
        internal static String GetType(LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    return "ERROR";
                case LogType.Information:
                    return "INFORMATION";
                case LogType.Warning:
                    return "WARNING";
                case LogType.WTF:
                    return "WTF";
                default:
                    return "ERROR";
            }
        }
    }
}
