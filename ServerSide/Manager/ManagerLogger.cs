using ServerSide.MyEventArgs;
using ServerSide.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Manager
{

    // Clasa aceasta este responsabila cu lista de mesaje dintre server si client pe partea Server-ului
    //Este ca un manager
    class ManagerLogger
    {
        private List<LogMessage> _logMessages;                  //Iar un manager are nevoie de o lista de mesage unde sa le stocheze

        public event EventHandler<LogMessageEventArgs> NewLogMessageEvent; // un obiect LogMessage într-un argument de eveniment și a-l transmite celor care ascultă evenimentul NewLogMessageEvent.

        public ManagerLogger()          // la initializarea unui Manager , ii cream lista goala
        {
            _logMessages = new List<LogMessage>();
        }

        // Adaugam in lista noastra un mesaj primit ca argument ( un mesaj este de tip LogMessage adica are 2 campuri Id si Text)
        public void AddLogMessage(LogMessage logMessage)
        {
            _logMessages.Add(logMessage);

            if (NewLogMessageEvent != null) // atata timp cat evenimentul nostru nu este se trigeruieste un eveniment 
            {
                NewLogMessageEvent(this, new LogMessageEventArgs(logMessage));
            }
        }

        public void AddLogMessage(string id, string message)
        {
            AddLogMessage(new LogMessage { Id = id, Message = message });
        }
    }
}
