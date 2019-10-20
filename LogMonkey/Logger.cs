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
        private static LoggerConfiguration mLoggerConfiguration;

        public static Logger Instance(LoggerConfiguration loggerConfiguration)
        {
            lock (mLock)
            {
                mLoggerConfiguration = loggerConfiguration;
                if (MLogger == null)
                {
                    MLogger = new Logger();
                }
                return MLogger;
            }
        }

        public bool E(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine("Exception::" + ex.ToString());
                if (mLoggerConfiguration.LogInnerException)
                {
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("InnerException::" + ex.InnerException.ToString());
                    }
                    else
                    {
                        Console.WriteLine("InnerException::" + "NULL");
                    }
                }
            }
            return true;
        }

        public bool E(Exception ex, params object[] extraInformation)
        {
            this.E(ex);
            foreach (var e in extraInformation)
            {
                Console.WriteLine(e.ToString());
            }
            return true;
        }

    }
}
