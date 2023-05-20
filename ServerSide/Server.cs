using Joc.Library;
using Lidgren.Network;
using ServerSide.Commands;
using ServerSide.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    class Server
    {
        private ManagerLogger _managerLogger;
        private List<PlayerDetails> _players;
        private NetPeerConfiguration _config;
        private NetServer _server;

        public Server(ManagerLogger managerLogger)
        {
            _managerLogger = managerLogger;
            _players = new List<PlayerDetails>();
            _config = new NetPeerConfiguration("JocDeDame") { Port = 14242 };
            _config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            _server = new NetServer(_config);
        }

        public void Run()
        {
            _server.Start();
            Console.WriteLine("Serverul a pornit");
            _managerLogger.AddLogMessage("Server", "Server Started");

            while (true)
            {
                NetIncomingMessage incmesage;
                /*incmesage = server.ReadMessage();*/

                /*
                Console.WriteLine("While Loop: inainte de  verificare mesaj" + s );
                s++;*/
                while ((incmesage = _server.ReadMessage()) != null)
                {
                    switch (incmesage.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            var login = new LoginCommand1();
                            login.Run(_managerLogger, _server, incmesage, null, _players);
                            break;
                        case NetIncomingMessageType.Data:
                            //Se va dezvolta logica pentru mesajele de tip data (informatii legate de player)
                            Data(incmesage);
                            _managerLogger.AddLogMessage("server", "Mesaj primit de la Client" + incmesage.ReadString());
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            var status = (NetConnectionStatus)incmesage.ReadByte();
                            _managerLogger.AddLogMessage("server", "Status Conexiune Schimbata " + status.ToString() + " (" + incmesage.ReadString() + ") " + incmesage.ToString());
                            break;
                        default:
                            _managerLogger.AddLogMessage("server", "Mesaj necunoscut de la client: " + incmesage.MessageType);
                            Console.WriteLine("Mesaj necunoscut de la client: " + incmesage.MessageType);
                            break;
                    }
                }

            }
        }

        public void Stop()
        {
            _managerLogger.AddLogMessage("Server", "Server Se opreste");
            _server.Shutdown("Serverul se opreste");
        }

        private void Data(NetIncomingMessage incmesage)
{
var packetType = (PacketType)incmesage.ReadByte();   // primeste de la client tipul de mesaj pe care vrea sa-l analizeze serverul
            var command = CommandFactory.GetCommand(packetType);
            command.Run(_managerLogger,_server, incmesage, null, _players);

        }
    }
}
