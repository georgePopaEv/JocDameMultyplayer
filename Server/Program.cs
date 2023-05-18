using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using LibrariaMea;
using Lidgren.Network;

using Server.Forms;

namespace Server
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.Run(new Main());
            var server = new Servers();
            server.Run();
        }
    }
}
