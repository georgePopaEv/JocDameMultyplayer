using Lidgren.Network;
using Newtonsoft.Json;
using ServerSide.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Commands
{
    class ValidMovesCommand : ICommand
    {
        public void Run(ManagerLogger managerLorgger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players = null)
        {
            managerLorgger.AddLogMessage("server", "Se trimit mutarile valide");
            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.ValidMoves);
            string serializeditem = JsonConvert.SerializeObject(server.listaValid_moves);
            Console.WriteLine(serializeditem);
            outmsg.Write(serializeditem);
            managerLorgger.AddLogMessage("server", serializeditem.ToString());
            server.NetServer.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0); //Trimiterea mesajului catre client si prelucrarea acestuia in Establish                                
            
        }
    }
}
