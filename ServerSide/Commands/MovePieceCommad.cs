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
    class MovePieceCommand : ICommand //se trimite dupa ce s-a realizat mutarea in mapa de pe server 
    {
        public void Run(ManagerLogger managerLorgger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players = null)
        {

            var outmsg = server.NetServer.CreateMessage();
            outmsg.Write((byte)PacketType.MovePieceCommand);


            var username = inc.ReadString(); //se citeste numele playerului care vrea sa faca mutarea
            var color_player = inc.ReadString(); // se citeste culoarea care vrea sa faca mutarea
            var nrow = inc.ReadInt32();
            var ncol = inc.ReadInt32();
            var player = players.FirstOrDefault(p => p.PlayerDetails.Name == username);
            managerLorgger.AddLogMessage("server", "S-a primit comanda de mutat piesa r=" + player.PlayerDetails.selectedPiece.Item1 + " and c = " + player.PlayerDetails.selectedPiece.Item2);
            if (color_player == server.turn && player.PlayerDetails.validMoves.Contains((nrow, ncol))) //&& player.PlayerDetails.selectedPiece != (-1,-1) 
            {
                managerLorgger.AddLogMessage("server", "r=" + player.PlayerDetails.selectedPiece.Item1 + "c=" + player.PlayerDetails.selectedPiece.Item2 + "pe r=" + nrow + " c=" + ncol);
                //server.Board.move(piece: (Piece)server.Board.board[player.PlayerDetails.selectedPiece.Item1, player.PlayerDetails.selectedPiece.Item2], nrow, ncol);
                Piece pieceSelected = new Piece(player.PlayerDetails.selectedPiece.Item1, player.PlayerDetails.selectedPiece.Item2, color_player.Contains("R=0") ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 0, 0));
                //server.Board.move(server.Board.board, pieceSelected, nrow, ncol);
                server.move(pieceSelected, nrow, ncol);
                //managerLorgger.AddLogMessage("server", "Se muta piesa r=" + player.PlayerDetails.selectedPiece.Item1 + " and c = " + player.PlayerDetails.selectedPiece.Item2);
                player.PlayerDetails.selectedPiece = (-1, -1);
                server.listaValid_moves = new List<(int, int)>();
            }

            //sa se efectueze mutarea piesei

            //TODO



            //string serializeditem = JsonConvert.SerializeObject(server.listaValid_moves);
            managerLorgger.AddLogMessage("server", "Se trimite mutarea catre clienti pentru a aparea tuturor");
            string filePath = "D:\\JocDameM\\JocDameMultyplayer\\ServerSide\\Test\\text.txt";
            using (StreamWriter writer = new StreamWriter(filePath))

                //StreamWriter writer = new StreamWriter(filePath);
                for (int i = 0; i < server.Board.ROWS; i++)
                {
                    for (int j = 0; j < server.Board.COLS; j++)
                    {
                        string serializeditem = JsonConvert.SerializeObject(server.Board.board[i, j]);
                        outmsg.Write(serializeditem);
                        try
                        {
                            // Deschideți fișierul în modul de scriere (daca nu există, va fi creat)

                            {
                                writer.WriteLine(serializeditem);
                                // Adăugați orice alte instrucțiuni de scriere dorite
                            }

                            Console.WriteLine("S-a scris cu succes în fișier.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("A apărut o eroare: " + ex.Message);
                        }
                        //managerLorgger.AddLogMessage("server", serializeditem);


                    }
                }
            //writer.Close();


            server.NetServer.SendToAll(outmsg, NetDeliveryMethod.ReliableOrdered); //sa se trimita harta catre toti participantii la joc (2 in cazul nostru) pentru fiecare room
                                                                                   //server.NetServer.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0); //Trimiterea mesajului catre client si prelucrarea acestuia in Establish                                

        }
    }
}
