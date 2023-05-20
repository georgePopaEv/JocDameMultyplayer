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
        public void Run(ManagerLogger managerLorgger, NetServer server, NetIncomingMessage inc, PlayerDetails player, List<PlayerDetails> players)
        {
            managerLorgger.AddLogMessage("server", "send full player list");
            var outmessage = server.CreateMessage();           //se creaza mesaj de catre server
            outmessage.Write((byte)PacketType.AllPlayers);      //se stampileaza cu tagul AllPLayers
            outmessage.Write(players.Count);                   // Se pune prima data cati playeri sunt 
            foreach (var p in players)                //pentru fiecare player 
            {
                outmessage.WriteAllProperties(p);      //se pune in mesaj detalii despre fiecare player
            }
            server.SendToAll(outmessage, NetDeliveryMethod.ReliableOrdered);  //se trimite catre toti 
        }
    }
}
