using Joc.Library;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using ServerSide.Manager;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

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
            string filePath = "D:\\JocDameM\\JocDameMultyplayer\\ServerSide\\Test\\textCuloare.txt";
            using (StreamWriter writer = new StreamWriter(filePath))
                writer.WriteLine(culoare);
                managerLorgger.AddLogMessage("server", string.Format("{0} a trimis row = {1}, col = {2}", culoare, row, col));

            playerAndConnection = players.FirstOrDefault(p => p.PlayerDetails.Name == name);
            if (playerAndConnection == null)
            {
                managerLorgger.AddLogMessage("server", string.Format("NU am gasit player cu acest nume{0}", name));
                return;
            }
            //server.Select(row, col);
            if (server.turn == playerAndConnection.PlayerDetails.color) //&& playerAndConnection.PlayerDetails.selectedPiece == (-1,-1)
                {
                managerLorgger.AddLogMessage("[DEBUG]", "TEST selected piece");
                playerAndConnection.PlayerDetails.selectedPiece = (row, col);           //se memoreaza piesa selectata
                    server.listaValid_moves = server.GetValidMoves(row, col , playerAndConnection.PlayerDetails.color); // se calculeaza valorile posibile  
                    playerAndConnection.PlayerDetails.selectedPiece = (row, col);
                    playerAndConnection.PlayerDetails.validMoves = server.listaValid_moves;  // ii asignam lista de posibile mutari si clientului pentru a avea in baza sa de date
                    Console.WriteLine("r=" + playerAndConnection.PlayerDetails.selectedPiece.Item1 + " c =" + playerAndConnection.PlayerDetails.selectedPiece.Item2);
                    var command1 = new ValidMovesCommand();
                    command1.Run(managerLorgger, server, inc, playerAndConnection, players);
                }
            /*else
            {
                if (server.turn == playerAndConnection.PlayerDetails.color)
                {
                    managerLorgger.AddLogMessage("server", "Mutare");
                    Console.WriteLine("MUUUUUTARE");
                    server.move(playerAndConnection.PlayerDetails.selectedPiece, (row, col));
                    var command1 = new UpdatedBoardCommand();
                    command1.Run(managerLorgger, server, inc, playerAndConnection, players);
                }
                
            }*/
            

            /*var command = new UpdatedBoardCommand();
            command.Run(managerLorgger, server, inc, playerAndConnection, players);*/
        }
    }
}
