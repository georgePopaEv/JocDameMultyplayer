using Joc.Library;
using Lidgren.Network;
using ServerSide.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerSide.Commands
{
    class AllPlayersCommand : ICommand
    {
        public void Run(ManagerLogger managerLorgger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players = null)
        {
            managerLorgger.AddLogMessage("server", "send full player list");
            var outmessage = server.NetServer.CreateMessage();           //se creaza mesaj de catre server
            outmessage.Write((byte)PacketType.AllPlayers);      //se stampileaza cu tagul AllPLayers
            outmessage.Write(players.Count);                   // Se pune prima data cati playeri sunt 
            foreach (var p in players)                //pentru fiecare player 
            {
                outmessage.WriteAllProperties(p.PlayerDetails);      //se pune in mesaj detalii despre fiecare player
            }
            server.NetServer.SendToAll(outmessage, NetDeliveryMethod.ReliableOrdered);  //se trimite catre toti 
        }
    }
}
