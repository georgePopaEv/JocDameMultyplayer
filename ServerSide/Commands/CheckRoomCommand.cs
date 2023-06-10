using Joc.Library;
using Lidgren.Network;
using Newtonsoft.Json;
using ServerSide.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Commands
{
    class CheckRoomCommand : ICommand //se trimite dupa ce s-a realizat mutarea in mapa de pe server 
    {
        public void Run(ManagerLogger managerLorgger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players = null)
        {

            var outmsg = server.NetServer.CreateMessage();
            // outmsg.Write((byte)PacketType.MovePieceCommand);


            var username = inc.ReadString(); //se citeste numele playerului care vrea sa faca mutarea
            var roomIdSelected = inc.ReadString();  // camera selectata
            var colorSelected = inc.ReadString();   //culoarea pe care ar dori-0

            //se cauta camera , daca aceasta exista dupa id
            var room = server._rooms.FirstOrDefault(p => p.id == roomIdSelected);

            if (room is null)
            {
                managerLorgger.AddLogMessage("server", "Camera nu a fost gasita");
                outmsg.Write(false); // camera nu a fost gasita deci, nu-l lasa sa intre in joc
            }
            else  //daca camera a fost gasita atunci verificam daca e primul sau nu
            {
                //il cautam pe player si il adaugam in lista
                var player = players.FirstOrDefault(p => p.PlayerDetails.Name == username);
                if (room._players.Count == 0) //daca este primul atunci 
                {//il adaugam pe player la lista 
                    player.PlayerDetails.color = colorSelected; // player-ul ia culoarea rosie
                    outmsg.Write("Red or Black available");
                    outmsg.Write(colorSelected.Contains("R=255") ? "R=255" : "R=0");
                    room._players.Add(player);
                }
                else if (room._players.Count == 1)
                {
                    var player1 = room._players[0].PlayerDetails;
                    if (player1.color.Contains("R=255") && colorSelected.Contains("R=0")) //daca are deja culoarea rosie luata atunci va fi adaugat ca negru
                    {
                        player.PlayerDetails.color = colorSelected;
                        outmsg.Write("Black availabe and full Room");
                        outmsg.Write("R=0");
                        room._players.Add(player);
                    }
                    else if (player1.color.Contains("R=255") && colorSelected.Contains("R=255"))
                    {
                        player.PlayerDetails.color = Color.FromArgb(0,0,0).ToString(); //IL SETAM LA NEGRU  
                        outmsg.Write("Red is taken forced Black and full Room");
                        outmsg.Write("R=0");
                        room._players.Add(player);
                    }
                    else if(player1.color.Contains("R=0") && colorSelected.Contains("R=255"))
                    {
                        player.PlayerDetails.color = colorSelected;
                        outmsg.Write("Red availabe and full Room");
                        outmsg.Write("R=255");
                        room._players.Add(player);
                    }
                    else if (player1.color.Contains("R=0") && colorSelected.Contains("R=0"))
                    {
                        player.PlayerDetails.color = colorSelected;
                        outmsg.Write("Black is taken forced Red and full Room");
                        outmsg.Write("R=255");
                        room._players.Add(player);
                    }
                }
                else
                {
                    outmsg.Write("Fully");
                }

                foreach (var client in room._players)
                {
                    //server.NetServer.SendMessage(outmsg, client.Connection, NetDeliveryMethod.ReliableOrdered);
                }
            }
            

            //server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered); //sa se trimita harta catre toti participantii la joc (2 in cazul nostru) pentru fiecare room
                                                                                   //server.NetServer.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0); //Trimiterea mesajului catre client si prelucrarea acestuia in Establish                                

        }
    }
}
