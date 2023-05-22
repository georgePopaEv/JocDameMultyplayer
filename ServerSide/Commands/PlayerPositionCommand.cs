using Joc.Library;
using Lidgren.Network;
using ServerSide.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerSide.Commands
{
    class PlayerPositionCommand : ICommand
    {
        public void Run(ManagerLogger managerLorgger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players = null)
        {
            var outmessage = server.NetServer.CreateMessage();           //se creaza mesaj pentru client 
            outmessage.Write((byte)PacketType.PlayerPosition);       // se scrie in mesaj faptul ca este un mesaj de tip PLayerPosition
            outmessage.WriteAllProperties(playerAndConnection.PlayerDetails);              // Se scrie in mesaj detaliile despre noul player creat
            server.NetServer.SendToAll(outmessage, NetDeliveryMethod.ReliableOrdered);  //se doreste sa se trimita catre toti conectati la server
        }
    }
}
