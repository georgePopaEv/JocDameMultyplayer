using LibrariaMea;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Commands
{
    class AllPlayersCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, PlayerDetails player, List<PlayerDetails> players)
        {
            Console.WriteLine("Sending Out full player list");
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
