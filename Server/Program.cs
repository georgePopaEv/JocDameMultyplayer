using System;
using System.Collections.Generic;
using System.Threading;
using LibrariaMea;
using Lidgren.Network;
namespace Server
{
    class Program
    {
        private static List<PlayerDetails> _players;
        static void Main(string[] args)
        {
            _players = new List<PlayerDetails>();
            var configuratie = new NetPeerConfiguration("JocDeDame") { Port= 14242};
            configuratie.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            var server = new NetServer(configuratie);
            server.Start();
            Console.WriteLine("Serverul a pornit");
            
            while (true)
            {
                NetIncomingMessage incmesage;
                /*incmesage = server.ReadMessage();*/
                
                /*
                Console.WriteLine("While Loop: inainte de  verificare mesaj" + s );
                s++;*/
                while ((incmesage = server.ReadMessage()) != null)
                {
                    
                    
                    switch (incmesage.MessageType)   
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            ConnectionApproval(server, incmesage);
                            break;
                        case NetIncomingMessageType.Data:
                            //Se va dezvolta logica pentru mesajele de tip data (informatii legate de player)
                            Console.WriteLine("Mesaj primit de la Client" + incmesage.ReadString()); 
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            var status = (NetConnectionStatus) incmesage.ReadByte();
                            Console.WriteLine("Status Conexiune Schimbata " + status.ToString() + " (" + incmesage.ReadString() + ") " + incmesage.ToString());
                            break;
                        default:
                            Console.WriteLine("Mesaj necunoscut de la client: " + incmesage.MessageType);
                            break;
                    }
                }

            }

        }

        private static void ConnectionApproval(NetServer server, NetIncomingMessage incmesage)
        {
            Console.WriteLine("Client Connectat: " + incmesage.SenderConnection.RemoteEndPoint);
            var data = incmesage.ReadByte();
            if (data == (byte)PacketType.Login)
            {
                var player = CreatePlayer(incmesage); // Se creaza un nou player si se adauga in lista de playeri pe baza a ce trimite la conectare clientul(daca el vrea sa fie pe o anumita pozitie sa trimita date pentru o anumita pozitie)
                incmesage.SenderConnection.Approve();  //Dam approve pentru Conexiune
                var outmsg = server.CreateMessage();    //Serverul creaza un mesaj
                outmsg.Write((byte)PacketType.Login);   // Adauga in mesaj bite-ul pentru Login (faptul ca este mesaj de tip login)
                outmsg.Write(true);                     //Acceptul pentru Client
                outmsg.Write(player.XPosiion);
                outmsg.Write(player.YPosiion);
                Console.WriteLine("..Created message " + outmsg.ToString());
                Thread.Sleep(500);      // O pauza mica pentru Server de juma de secunda
                server.SendMessage(outmsg, incmesage.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0); //Trimiterea mesajului catre client si prelucrarea acestuia in Establish                                
            }
            else
            {
                incmesage.SenderConnection.Deny("Didn't sent correct information."); // In caz de nu se primeste nici un mesaj de la Client de tip Connection Approval
            }
        }

        private static PlayerDetails CreatePlayer(NetIncomingMessage incmesage)
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
            if(player is null)
            {
                Console.WriteLine("este null ");
            }
            else
            {
                Console.WriteLine("nu este null ");

                _players.Add(player);
            }
            
            return player;
        }
    }
}
