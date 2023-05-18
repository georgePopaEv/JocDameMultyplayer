using System;
using System.Collections.Generic;
using System.Text;
using LibrariaMea;
using Lidgren.Network;
using System.Threading;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Server.Commands;

namespace Server
{
    
    class Servers
    {
        private List<PlayerDetails> _players;
        private NetPeerConfiguration _config;
        private NetServer _server;

        public Servers()
        {
            _players = new List<PlayerDetails>();
            _config =  new NetPeerConfiguration("JocDeDame") { Port = 14242 };
            _config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            _server = new NetServer(_config);
        }

        public void Run()
        {
            _server.Start();
            Console.WriteLine("Serverul a pornit");

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
                            login.Run(_server, incmesage, null, _players);
                            break;
                        case NetIncomingMessageType.Data:
                            //Se va dezvolta logica pentru mesajele de tip data (informatii legate de player)
                            Data(incmesage);
                            Console.WriteLine("Mesaj primit de la Client" + incmesage.ReadString());
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            var status = (NetConnectionStatus)incmesage.ReadByte();
                            Console.WriteLine("Status Conexiune Schimbata " + status.ToString() + " (" + incmesage.ReadString() + ") " + incmesage.ToString());
                            break;
                        default:
                            Console.WriteLine("Mesaj necunoscut de la client: " + incmesage.MessageType);
                            break;
                    }
                }

            }
        }

        private void Data(NetIncomingMessage incmesage)
        {
            var packetType = (PacketType)incmesage.ReadByte();   // primeste de la client tipul de mesaj pe care vrea sa-l analizeze serverul
            var command = CommandFactory.GetCommand(packetType);
            command.Run(_server, incmesage, null, _players);

        }

        
    }
}
