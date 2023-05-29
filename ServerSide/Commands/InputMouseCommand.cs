using Joc.Library;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using ServerSide.Manager;
using System.Text;
using System.Drawing;
using System.Windows.Forms;


namespace ServerSide.Commands
{
    class InputMouseCommand : ICommand
    {
        public void Run(ManagerLogger managerLorgger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players = null)
        {
            managerLorgger.AddLogMessage("server", "Received Mouse Click");
            var name = inc.ReadString();                  // primeste numele clientului

            
            var culoare = inc.ReadString();
            var row = inc.ReadInt32();
            var col = inc.ReadInt32();
            managerLorgger.AddLogMessage("server", string.Format("{0} a trimis row = {1}, col = {2}", culoare, row, col));

            playerAndConnection = players.FirstOrDefault(p => p.PlayerDetails.Name == name);
            if (playerAndConnection == null)
            {
                managerLorgger.AddLogMessage("server", string.Format("NU am gasit player cu acest nume{0}", name));
                return;
            }
            //server.Select(row, col);
            server.listaValid_moves = server.GetValidMoves(row,col);

            var command1 = new ValidMovesCommand();
            command1.Run(managerLorgger, server, inc, playerAndConnection, players);

            /*var command = new UpdatedBoardCommand();
            command.Run(managerLorgger, server, inc, playerAndConnection, players);*/
        }
    }
}
