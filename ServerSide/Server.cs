using Joc.Library;
using Lidgren.Network;
using Newtonsoft.Json;
using ServerSide.Commands;
using ServerSide.Manager;
using ServerSide.MyEventArgs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    class Server
    {
        public event EventHandler<NewPLayerEventArgs> NewPlayer;
        public event EventHandler<KickPLayerEventArgs> KickPlayerFromListBox;
        private ManagerLogger _managerLogger;
        private List<PlayerAndConnection> _players;
        private NetPeerConfiguration _config;

        private Piece selected = null;
        public Board Board;
        public string turn = Color.FromArgb(0, 0, 0).ToString();
        public Dictionary<(int, int), List<Piece>> validMoves = new Dictionary<(int, int), List<Piece>>();
        public List<(int, int)> listaValid_moves = new List<(int, int)>();


        public NetServer NetServer { get;  private set;}

        public Server(ManagerLogger managerLogger)
        {
            _managerLogger = managerLogger;
            _players = new List<PlayerAndConnection>();
            _config = new NetPeerConfiguration("JocDeDame") { Port = 14242 };
            
            _config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            NetServer = new NetServer(_config);
            Board = new Board();
        }

        public void Run()
        {
            NetServer.Start();
            Console.WriteLine("Serverul a pornit");
            _managerLogger.AddLogMessage("Server", "Server Started");
            

            while (true)
            {
                NetIncomingMessage incmesage;
                /*incmesage = server.ReadMessage();*/

                /*
                Console.WriteLine("While Loop: inainte de  verificare mesaj" + s );
                s++;*/
                while ((incmesage = NetServer.ReadMessage()) != null)
                {
                    switch (incmesage.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            
                            var login = new LoginCommand1();
                            login.Run(_managerLogger, this, incmesage, null, _players);
                            break;
                        case NetIncomingMessageType.Data:
                            //Se va dezvolta logica pentru mesajele de tip data (informatii legate de player)
                            Data(incmesage);
                            _managerLogger.AddLogMessage("server", "Mesaj primit de la Client" + incmesage.ReadString());
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            var status = (NetConnectionStatus)incmesage.ReadByte();
                            var username = incmesage.ReadString();
                            if (status.ToString() == "Disconnected")
                            {
                                KickPlayerFromListBox(this, new KickPLayerEventArgs(username));
                            }
                            _managerLogger.AddLogMessage("server", "Status Conexiune Schimbata " + status.ToString() + " (" + username + ")===" + username.Length + "= " + incmesage.ToString());
                            
                            break;
                        default:
                            _managerLogger.AddLogMessage("server", "Mesaj necunoscut de la client: " + incmesage.MessageType);
                            Console.WriteLine("Mesaj necunoscut de la client: " + incmesage.MessageType);
                            break;
                    }
                }

            }
        }

        public void Stop()
        {
            _managerLogger.AddLogMessage("Server", "Server Se opreste");
            NetServer.Shutdown("Serverul se opreste");
        }

        private void Data(NetIncomingMessage incmesage)
        {
            var packetType = (PacketType)incmesage.ReadByte();   // primeste de la client tipul de mesaj pe care vrea sa-l analizeze serverul
            var command = CommandFactory.GetCommand(packetType);
            command.Run(_managerLogger, this, incmesage, null, _players);

        }

        

        public void SendNewPlayerEvent(string username)
        {
            if (NewPlayer != null)
                NewPlayer(this, new NewPLayerEventArgs(username));
        }

        public void KickPlayer(int playerIndex)
        {
            var command = CommandFactory.GetCommand(PacketType.Kick);
            command.Run(_managerLogger, this, null, _players[playerIndex], _players);
        }
        public List<(int, int)> valid_Moves(int row, int col)
        {
            Piece piece = (Piece)Board.get_piece(row, col);
            if (piece != null && piece.color.ToString() == turn)
            {
                selected = piece;
                validMoves = Board.GetValidMoves(piece);
            }
            return listaValid_moves ;
        }

        public List<(int, int)> GetValidMoves(int row, int col)
        {
            List<(int, int)> validMoves = new List<(int, int)>();

            // Verificăm dacă poziția curentă conține o piesă validă de culoarea specificată
            if (Board.board[row, col] is Piece)
            {
                Piece p = (Piece)Board.board[row, col];
                // Verificăm posibilitatea de mutare în sus
                if (p.color == Color.FromArgb(0, 0, 0))
                {
                    if (IsMoveValid(row + 1, col - 1))
                        validMoves.Add((row + 1, col - 1));

                    if (IsMoveValid(row + 1, col + 1))
                        validMoves.Add((row + 1, col + 1));
                }

                // Verificăm posibilitatea de mutare în jos
                if (p.color == Color.FromArgb(255, 0, 0))
                {
                    if (IsMoveValid(row - 1, col - 1))
                        validMoves.Add((row - 1, col - 1));

                    if (IsMoveValid(row - 1, col + 1))
                        validMoves.Add((row - 1, col + 1));
                }
            }

            return validMoves;
        }

        // Funcție auxiliară pentru verificarea unei mutări valide
        private bool IsMoveValid(int row, int col)
        {
            // Verificați dacă poziția (row, col) se află în limitele tablei de joc
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {
                // Verificați dacă poziția (row, col) este liberă (nu conține o piesă)
                if (Board.board[row, col] is int)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Select(int row, int col)
        {
            if (selected != null)
            {
                bool result = Move(row, col);
                if (!result)
                {
                    selected = null;
                    Select(row, col);
                }
            }
            else
            {
                Piece piece = (Piece)Board.get_piece(row, col);
                if (piece != null && piece.color.ToString() == turn)
                {
                    selected = piece;
                    validMoves = Board.GetValidMoves(piece);
                    return true;
                }
            }

            return false;
        }
        public bool Move(int row, int col)
        {
            Piece piece = (Piece)Board.get_piece(row, col);
            if (selected != null && piece == null && validMoves.ContainsKey((row, col)))
            {
                Board.move(selected, row, col);
                List<Piece> skipped = validMoves[(row, col)];
                if (skipped != null)
                {
                    Board.remove(skipped);
                }
                //ChangeTurn();
            }
            else
            {
                return false;
            }
            return true;
        }

        

    }
}
