using ServerSide.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.MyEventArgs
{
    class LogMessageEventArgs : EventArgs
    {
        public LogMessage LogMessage { get; set; } 

        public LogMessageEventArgs(LogMessage logMessage)
        {
            LogMessage = logMessage;
        }

    }
}
