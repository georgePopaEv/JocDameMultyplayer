using System;
using System.Collections.Generic;
using System.Threading;
using LibrariaMea;
using Lidgren.Network;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();
            server.Run();
        }
    }
}
