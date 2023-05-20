using Joc.Library;
using Lidgren.Network;
using ServerSide.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServerSide.Commands
{
    class LoginCommand1 : ICommand
    {
        public void Run(ManagerLogger managerLorgger, NetServer server, NetIncomingMessage incmesage, PlayerDetails player, List<PlayerDetails> players)
        {
            
            managerLorgger.AddLogMessage("server", "Client Connectat: " + incmesage.SenderConnection.RemoteEndPoint);
            var data = incmesage.ReadByte();
            if (data == (byte)PacketType.Login)
            {
                managerLorgger.AddLogMessage("server", "..connection accepted");
                player = CreatePlayer(incmesage, players); // Se creaza un nou player si se adauga in lista de playeri pe baza a ce trimite la conectare clientul(daca el vrea sa fie pe o anumita pozitie sa trimita date pentru o anumita pozitie)
                incmesage.SenderConnection.Approve();  //Dam approve pentru Conexiune
                var outmsg = server.CreateMessage();    //Serverul creaza un mesaj  // Mai cream un mesaj 
                outmsg.Write((byte)PacketType.Login);   // Adauga in mesaj bite-ul pentru Login (faptul ca este mesaj de tip login)
                outmsg.Write(true);                     //Acceptul pentru Client
                outmsg.Write(players.Count);
                Thread.Sleep(500);      // O pauza mica pentru Server de juma de secunda
                for (int n = 0; n < players.Count; n++)
                {
                    outmsg.WriteAllProperties(players[n]);
                }
                server.SendMessage(outmsg, incmesage.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0); //Trimiterea mesajului catre client si prelucrarea acestuia in Establish                                
                var command = new PlayerPositionCommand();
                command.Run(managerLorgger, server, incmesage, player, players);
                

                //SendFullPlayerList();
                //SendNewPlayer(player, incmesage);   // se trimite inapoi mesaj ca este un nou player adaugat in server , se va trata in client folder 
            }
            else
            {
                incmesage.SenderConnection.Deny("Didn't sent correct information."); // In caz de nu se primeste nici un mesaj de la Client de tip Connection Approval
            }
        }

        private PlayerDetails CreatePlayer(NetIncomingMessage incmesage, List<PlayerDetails> players)
        {
            var random = new Random();
            var player = new PlayerDetails // se construieste player-ul  //se creaza un nou player pentru ca s-a primit o noua cerere de conectare->prin urmare este un alt player
            {
                // pe baza conexiunii trimise la server
                Name = incmesage.ReadString(),           // pe baza numelui trimis la server
                XPosition = random.Next(0, 750),         //un x random
                YPosition = random.Next(0, 420)      //un y random 
            };
            //se adauga in lista noastra de playeri de care se ocupa serverul
            players.Add(player);


            return player;
        }
    }
}
