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
        public List<Room> _rooms;

        private Piece selected = null;
        public Board Board;
        public string turn = Color.FromArgb(0, 0, 0).ToString();
        //public Dictionary<(int, int), List<(int,int)>> validMoves = new Dictionary<(int, int), List<(int, int)>>();
        public List<(int, int)> listaValid_moves = new List<(int, int)>();


        public NetServer NetServer { get;  private set;}

        public Server(ManagerLogger managerLogger)
        {
            _managerLogger = managerLogger;

            _players = new List<PlayerAndConnection>();

            _rooms = new List<Room>();
            _config = new NetPeerConfiguration("JocDeDame") { Port = 14242 };
            
            _config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            NetServer = new NetServer(_config);
            Board = new Board();
        }

        public void Run()
        {
            NetServer.Start();  //Se porneste server-ul 
            _managerLogger.AddLogMessage("Server", "Server Started");  // mesaj in log pentru pornirea server-ului
            while (true) 
            {
                NetIncomingMessage incmesage;  // se declara mesajul de tip inc
                while ((incmesage = NetServer.ReadMessage()) != null)   // cat timp avem un mesaj in inbox-ul server-ului
                {
                    switch (incmesage.MessageType)  // in functie de tipul mesajului scriem ...
                    {
                        case NetIncomingMessageType.ConnectionApproval:  // daca este de tip Connection , initializam conexiunea cu clientul
                            
                            var login = new LoginCommand1();                // declaram o comanda de Login
                            login.Run(_managerLogger, this, incmesage, null, _players);  // ii dam run comenzii de login , unde trimitem date de conectare inapoi la client
                            break;
                        case NetIncomingMessageType.Data:   // //Se va dezvolta logica pentru mesajele de tip data (informatii legate de player) // de ceea ce face player-ul (creaza camere, se misca si tot asa)
                            Data(incmesage);
                            _managerLogger.AddLogMessage("server", "Mesaj primit de la Client" + incmesage.ReadString());
                            break;
                        case NetIncomingMessageType.StatusChanged:  // atunci cand exista un status schimbat de la client
                            var status = (NetConnectionStatus)incmesage.ReadByte();     //se citeste statusul
                            var username = incmesage.ReadString();                  //se citeste username-ul care-si schimba statusul
                            if (status.ToString() == "Disconnected")   // daca este deconectata atunci se apeleaza eventul de kick
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
            var command = CommandFactory.GetCommand(packetType); // se creaza o camanda in functie de packetul pe care -l primeste
            command.Run(_managerLogger, this, incmesage, null, _players);   //se ruleaza respectiva comanda

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
        

        public void move(Piece pieceselected, int nrow, int ncol)
        {
            // Console.WriteLine("r=" + selected.Item1 + "c=" + selected.Item2);
            // object p = Board.board[selected.Item1, selected.Item2];

            //Board.board[pieceselected.col, pieceselected.row] = pieceselected;
            Board.board[pieceselected.row, pieceselected.col] = null ;
            Board.board[pieceselected.row, pieceselected.col] = 0 ;
            
            pieceselected.move(nrow, ncol);
            Board.board[pieceselected.row, pieceselected.col] = pieceselected;


            //dupe ce se efectueaza mutarea
            ChangeTurn();
        }

        private void ChangeTurn()
        {
            if (turn.Equals(Color.FromArgb(0, 0, 0).ToString()))
            {
                turn = Color.FromArgb(255, 0, 0).ToString();
            }
            else
            {
                turn = Color.FromArgb(0, 0, 0).ToString();
            }
        }

        public  List<(int, int)> GetValidMoves(int row, int col, string color)
        {
            //List<(int, int)>> valid_moves = new Dictionary<(int, int), List<(int, int)>>();
            List<(int, int)> validMoves = new List<(int, int)>();

            // Verificăm dacă poziția curentă conține o piesă validă de culoarea specificată
            if (Board.board[row, col] is Piece)
            {
                Piece p = (Piece)Board.board[row, col];
                // Verificăm posibilitatea de mutare în sus
                if (p.color == Color.FromArgb(0, 0, 0) && color == Color.FromArgb(0, 0, 0).ToString())
                {
                    if (IsMoveValid(row + 1, col - 1))
                       // valid_moves[(row,col)].Add((row + 1, col - 1));
                        validMoves.Add((row + 1, col - 1));

                    if (IsMoveValid(row + 1, col + 1))
                        //valid_moves[(row, col)].Add((row + 1, col + 1));
                        validMoves.Add((row + 1, col + 1));
                }

                // Verificăm posibilitatea de mutare în jos
                if (p.color == Color.FromArgb(255, 0, 0) && color == Color.FromArgb(255, 0, 0).ToString())
                {
                    if (IsMoveValid(row - 1, col - 1))
                        //valid_moves[(row, col)].Add((row - 1, col - 1));
                        validMoves.Add((row - 1, col - 1));

                    if (IsMoveValid(row - 1, col + 1))
                        //valid_moves[(row, col)].Add((row - 1, col + 1));
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
                /*bool result = Move(row, col);
                if (!result)
                {
                    selected = null;
                    Select(row, col);
                }*/
            }
            else
            {
                Piece piece = (Piece)Board.get_piece(row, col);
                if (piece != null && piece.color.ToString() == turn)
                {
                    selected = piece;
                    //validMoves = Board.GetValidMoves(piece);
                    return true;
                }
            }

            return false;
        }
       /* public bool Move(int row, int col)
        {
            Piece piece = (Piece)Board.get_piece(row, col);
            *//*if (selected != null && piece == null && validMoves.ContainsKey((row, col)))
            {
                Board.move(selected, row, col);
                List<Piece> skipped = validMoves[(row, col)];
                if (skipped != null)
                {
                    Board.remove(skipped);
                }
                
            }
            else
            {
                return false;
            }
            return true;*//*
        }
*/
        

    }
}
