using ServerSide.Manager;
using ServerSide.MyEventArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide.Forms
{
    public partial class MainForm : Form
    {
        private Task _task;         //se creaza un task pentru a rula server-ul
        private Server _server;     // se instantiaza un server in acest form
        private ManagerLogger _managerLogger;
        private CancellationTokenSource _cancellationTokenSource;
        public MainForm()
        {
            _managerLogger = new ManagerLogger();
            _managerLogger.NewLogMessageEvent += NewLogMessageEvent;
            /*Cu această înregistrare, atunci când se apelează metoda AddLogMessage a obiectului _managerLogger și 
             * evenimentul NewLogMessageEvent este declanșat, handler-ul _managerLogger_NewLogMessageEvent va fi apelat 
             * și va primi ca argumente obiectul sender (în cazul de față _managerLogger) și un obiect LogMessageEventArgs 
             * care conține mesajul de log. Acest handler poate efectua acțiuni specifice cu mesajul de log, cum ar fi afișarea 
             * lui în interfața utilizatorului sau stocarea*/
            _server = new Server(_managerLogger);     //se atribuie o instanta de tip Server
            InitializeComponent();      
            
        }

        private void NewLogMessageEvent(object sender, MyEventArgs.LogMessageEventArgs e)
        {

            if (InvokeRequired)
            {
                Invoke(new EventHandler<LogMessageEventArgs>(NewLogMessageEvent), sender, e);
                return;
            }
            dgwServerStatusLog.Rows.Add(new[] { e.LogMessage.Id, e.LogMessage.Message});
        }

        //Click pe Start Server button 
        private void btnStart_Click(object sender, EventArgs e) 
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            _cancellationTokenSource = new CancellationTokenSource();
            _task = new Task(_server.Run,_cancellationTokenSource.Token);      //se creaza un task 
            _task.Start();                      //se porneste task-ul 
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            if (_task != null && _cancellationTokenSource != null)
            {
                _server.Stop();
                _cancellationTokenSource.Cancel();
            }
            
        }
    }
}
