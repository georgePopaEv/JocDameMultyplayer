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
    class UpdatedBoardCommand : ICommand
    {
        public void Run(ManagerLogger managerLorgger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players = null)
        {
            managerLorgger.AddLogMessage("server", "send updated Board");
            var outmessage = server.NetServer.CreateMessage();           //se creaza mesaj de catre server
            outmessage.Write((byte)PacketType.UpdatedBoard);      //se stampileaza cu tagul AllPLayers


            for (int i = 0; i < server.Board.ROWS; i++)
            {
                for (int j = 0; j < server.Board.COLS; j++)
                {
                    string serializeditem = JsonConvert.SerializeObject(server.Board.board[i, j]);
                    Console.WriteLine(serializeditem);
                    outmessage.Write(serializeditem);
                }
            }
            //string serializedBoard = JsonConvert.SerializeObject(server.Board.board);
            //outmessage.Write(serializedBoard);            
            server.NetServer.SendToAll(outmessage, NetDeliveryMethod.ReliableOrdered);  //se trimite catre toti 
        }
    }
}
