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
        private static LoggerConfiguration sLoggerConfiguration;
        private static LoggerConfiguration sAlternateConfiguration;
        private static LoggerConfiguration sActiveLoggerConfiguration;
        private static bool useAlternateConfiguration;

        //Initialize Logger.
        public static void Initialize(LoggerConfiguration loggerConfiguration, LoggerConfiguration alternateConfiguration = null)
        {
            sLoggerConfiguration = loggerConfiguration;
            sAlternateConfiguration = alternateConfiguration;
            sActiveLoggerConfiguration = sLoggerConfiguration;
            useAlternateConfiguration = false;
        }

        //Get a singleton instance of logger.
        public static Logger Instance()
        {
            if (sActiveLoggerConfiguration == null)
            {
                throw new LoggerNotInitializedException();
            }
            lock (mLock)
            {
                if (MLogger == null)
                {
                    MLogger = new Logger();
                }
                return MLogger;
            }
        }

        private void ToggleAlternateConfiguration()
        {
            useAlternateConfiguration = !useAlternateConfiguration;
        }

        public bool SwitchConfiguration()
        {
            ToggleAlternateConfiguration();
            sActiveLoggerConfiguration = (useAlternateConfiguration) ? sAlternateConfiguration : sLoggerConfiguration;
            return useAlternateConfiguration;
        }

        public bool Error(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine("Exception::" + ex.ToString());
                if (sActiveLoggerConfiguration.LogInnerException)
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

        public bool Error(Exception ex, params object[] extraInformation)
        {
            this.Error(ex);
            foreach (var e in extraInformation)
            {
                Console.WriteLine(e.ToString());
            }
            return true;
        }

        public bool Warning()
        {
            return true;
        }

        public bool Information()
        {
            return true;
        }

    }
}
