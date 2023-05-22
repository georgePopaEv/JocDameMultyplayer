using Joc.Library;
using Lidgren.Network;
using ServerSide.Commands;
using ServerSide.Manager;
using ServerSide.MyEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    class Server
    {
        public event EventHandler<NewPLayerEventArgs> NewPlayer;
        private ManagerLogger _managerLogger;
        private List<PlayerDetails> _players;
        private NetPeerConfiguration _config;
        public NetServer NetServer { get;  private set;}

        public Server(ManagerLogger managerLogger)
        {
            _managerLogger = managerLogger;
            _players = new List<PlayerDetails>();
            _config = new NetPeerConfiguration("JocDeDame") { Port = 14242 };
            _config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            NetServer = new NetServer(_config);
        }

        public void Run()
        {
            NetServer.Start();
            Console.WriteLine("Serverul a pornit");
            _managerLogger.AddLogMessage("Server", "Server Started");

            while (true)
            {
                NetIncomingMessage incmesage;
                /*incmesage = server.ReadMessage();*/

                /*
                Console.WriteLine("While Loop: inainte de  verificare mesaj" + s );
                s++;*/
                while ((incmesage = NetServer.ReadMessage()) != null)
                {
                    switch (incmesage.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            var login = new LoginCommand1();
                            login.Run(_managerLogger, this, incmesage, null, _players);
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
            NetServer.Shutdown("Serverul se opreste");
        }

        private void Data(NetIncomingMessage incmesage)
        {
            var packetType = (PacketType)incmesage.ReadByte();   // primeste de la client tipul de mesaj pe care vrea sa-l analizeze serverul
            var command = CommandFactory.GetCommand(packetType);
            command.Run(_managerLogger, this, incmesage, null, _players);

        }

        

        public void SendNewPlayerEvent(string username)
        {
            if (NewPlayer != null)
                NewPlayer(this, new NewPLayerEventArgs(username));
        }

        public void KickPlayer(int playerIndex)
        {
            var command = CommandFactory.GetCommand(PacketType.Kick);
            command.Run(_managerLogger, this, null, _players[playerIndex], _players);
        }
    }
}
