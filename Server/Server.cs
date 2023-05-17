using System;
using System.Collections.Generic;
using System.Text;
using LibrariaMea;
using Lidgren.Network;
using System.Threading;
namespace Server
{
    
    class Server
    {
        private List<PlayerDetails> _players;
        private NetPeerConfiguration _config;
        private NetServer _server;

        public Server()
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
                            ConnectionApproval(incmesage);
                            break;
                        case NetIncomingMessageType.Data:
                            //Se va dezvolta logica pentru mesajele de tip data (informatii legate de player)
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

        private void ConnectionApproval(NetIncomingMessage incmesage)
        {
            Console.WriteLine("Client Connectat: " + incmesage.SenderConnection.RemoteEndPoint);
            var data = incmesage.ReadByte();
            if (data == (byte)PacketType.Login)
            {
                var player = CreatePlayer(incmesage); // Se creaza un nou player si se adauga in lista de playeri pe baza a ce trimite la conectare clientul(daca el vrea sa fie pe o anumita pozitie sa trimita date pentru o anumita pozitie)
                
                incmesage.SenderConnection.Approve();  //Dam approve pentru Conexiune
                var outmsg = _server.CreateMessage();    //Serverul creaza un mesaj  // Mai cream un mesaj 
                outmsg.Write((byte)PacketType.Login);   // Adauga in mesaj bite-ul pentru Login (faptul ca este mesaj de tip login)
                outmsg.Write(true);                     //Acceptul pentru Client
                outmsg.Write((int)player.XPosiion);
                outmsg.Write((int)player.YPosiion);
                Thread.Sleep(500);      // O pauza mica pentru Server de juma de secunda
                outmsg.Write(_players.Count);
                foreach (var player1 in _players)
                {
                    if(player.Name != player1.Name)
                        outmsg.WriteAllProperties(player1);
                }
                _server.SendMessage(outmsg, incmesage.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0); //Trimiterea mesajului catre client si prelucrarea acestuia in Establish                                
                SendNewPlayer(player, incmesage);

                SendFullPlayerList();
                //SendNewPlayer(player, incmesage);   // se trimite inapoi mesaj ca este un nou player adaugat in server , se va trata in client folder 
            }
            else
            {
                incmesage.SenderConnection.Deny("Didn't sent correct information."); // In caz de nu se primeste nici un mesaj de la Client de tip Connection Approval
            }
        }

        private PlayerDetails CreatePlayer(NetIncomingMessage incmesage)
        {
            var random = new Random();
            var player = new PlayerDetails // se construieste player-ul  //se creaza un nou player pentru ca s-a primit o noua cerere de conectare->prin urmare este un alt player
            {
                // pe baza conexiunii trimise la server
                Name = incmesage.ReadString(),           // pe baza numelui trimis la server
                XPosiion = random.Next(0, 750),         //un x random
                YPosiion = random.Next(0, 420)      //un y random 
            };
            //se adauga in lista noastra de playeri de care se ocupa serverul
            _players.Add(player);
            

            return player;
        }

        private void SendNewPlayer(PlayerDetails player, NetIncomingMessage inc)
        {
            var outmessage = _server.CreateMessage();           //se creaza mesaj pentru client 
            outmessage.Write((byte)PacketType.NewPlayer);       // se scrie in mesaj faptul ca este un mesaj de tip Newplayer
            outmessage.WriteAllProperties(player);              // Se scrie in mesaj detaliile despre noul player creat
            _server.SendToAll(outmessage, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);  //se doreste sa se trimita catre toti conectati la server
        }

        private void SendFullPlayerList()
        {
            Console.WriteLine("Sending Out full player list");
            var outmessage = _server.CreateMessage();           //se creaza mesaj de catre server
            outmessage.Write((byte)PacketType.AllPlayers);      //se stampileaza cu tagul AllPLayers
            outmessage.Write(_players.Count);                   // Se pune prima data cati playeri sunt 
            foreach (var player in _players)                //pentru fiecare player 
            {
                outmessage.WriteAllProperties(player);      //se pune in mesaj detalii despre fiecare player
            }
            _server.SendToAll(outmessage, NetDeliveryMethod.ReliableOrdered);  //se trimite catre toti 

        }
    }
}
