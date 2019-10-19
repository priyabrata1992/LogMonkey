using LogMonkey.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonkey.ComponentImpl
{
    public class LoggerConfiguration
    {

        private static LoggerConfiguration mConfiguration;
        private static readonly Object mLock = new Object();

        private LoggerConfiguration()
        {
        }


        protected static LoggerConfiguration Instance()
        {
            lock (mLock)
            {
                if (mConfiguration == null)
                {
                    mConfiguration = new LoggerConfiguration();
                }

                return mConfiguration;
            }
        }

    }

}

