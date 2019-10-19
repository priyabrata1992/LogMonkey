using LogMonkey.ComponentImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey
{
    public sealed class Logger
    {
        Logger() { } //private constructor

        
        private static readonly object mLock = new object();
        private static Logger MLogger;

        public static Logger Instance(LoggerConfiguration loggerConfiguration)
        {
            lock (mLock)
            {
                if (MLogger == null)
                {
                    MLogger = new Logger();
                }
                return MLogger;
            }
        }
    }
}
