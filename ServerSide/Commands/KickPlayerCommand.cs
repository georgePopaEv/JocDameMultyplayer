using Joc.Library;
using Lidgren.Network;
using ServerSide.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Commands
{
    class KickPlayerCommand : ICommand
    {
        public void Run(ManagerLogger managerLorgger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players = null)
        {
            managerLorgger.AddLogMessage("Server", string.Format("Kicking {0}", playerAndConnection.PlayerDetails.Name));
            var outmessage = server.NetServer.CreateMessage();
            outmessage.Write((byte)PacketType.Kick);
            outmessage.Write(playerAndConnection.PlayerDetails.Name);
            server.NetServer.SendToAll(outmessage, NetDeliveryMethod.ReliableOrdered);
            //Kick Player

            playerAndConnection.Connection.Disconnect("Bye bye you are kicked.");
            
        }
    }
}
