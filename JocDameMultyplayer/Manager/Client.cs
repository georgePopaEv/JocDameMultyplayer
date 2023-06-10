using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Joc.Library;
using JocDameMultyplayer.MyEventArgsClientSide;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace JocDameMultyplayer
{
    public class Client
    {
        private NetClient _client;
        public List<PlayerDetails> Players { get; set; }
        public List<Room> _rooms;


        public event EventHandler<RoomUpdatedEventArgs> RoomUpdatedEvent;
        public string Username { get; set; }
        public Color color;
        public bool Active { get; set; }
        public Board board;
        public string serializedBoard;
        public string roomIdEntered;
        //public Dictionary<(int, int), List<Piece>> validMoves = new Dictionary<(int, int), List<Piece>>();
        public List<(int, int)> validMoves = new List<(int, int)>();
        public Client()
        {
            Players = new List<PlayerDetails>();
            _rooms = new List<Room>();
            
        }

        public bool Start()
        {
            var random = new Random();
            
            _client = new NetClient(new NetPeerConfiguration("JocDeDame")); //se creaza client-ul connectat la server
            _client.Start();  // se porneste clientul
            //Username = "name_" + random.Next(0, 100); // Se creaza un player cu numele x
            var outmsg = _client.CreateMessage();   //se creaza un mesaj din partea clientului
            outmsg.Write((byte)PacketType.Login);   //se scrie in mesaj ce tip de packet de doreste, iar in acest caz este un packet de type Login
            outmsg.Write(Username);
            color = Color.FromArgb(0, 0, 0);
            board = new Board("empty");
            //outmsg.WriteAllProperties(PlayerDetails);      // Dupa ce ii trimitem server-
            _client.Connect("localhost", 14242, outmsg);    // se incearca connectarea la server si se trimite si mesajul
            return EstablishInfo();     //se returneaza True daca s-a reusit connectarea la server

        }

        private bool EstablishInfo()
        {
            var time = DateTime.Now;
            NetIncomingMessage incmessage;   /// se declara inc message care se ocupa de primirea mesajelor de la server
            while (true)
            {
                if(DateTime.Now.Subtract(time).Seconds > 5)
                {
                    return false;
                }

                while((incmessage = _client.ReadMessage()) != null) // Cat timp mesajul de la server este diferit de null atunci il analizam
                {
                    switch (incmessage.MessageType)     // verifica ce fel de mesaj de la server primim prin acest switch
                    {
                        case NetIncomingMessageType.Data:           //Daca mesajul este incadrat de tip data atunci
                            Console.WriteLine("Primit mesaj de la Server");         
                            var data_from_server = incmessage.ReadByte();       //citim mesajul de la server
                            if(data_from_server == (byte)PacketType.Login)      // Identificam daca mesajul este tot de tip login pentru a se realiza conexiunea
                            {
                                Active = incmessage.ReadBoolean(); // daca este de tip login atunci citim urmatorul packet care ar trebui sa fie approv-ul de la server
                                if (Active)   // daca este pozitiv atunci pornim clientul
                                {
                                    ReceiveAllPlayers(incmessage);
                                    ReadBoard(incmessage);
                                    ReceiveAllRooms(incmessage);
                                    

                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            break;

                    }
                }
            }
            throw new NotImplementedException();
        }

        public void checkTheRoom(string roomIdSelected, string v)
        {
            var outmessage = _client.CreateMessage();
            outmessage.Write((byte)PacketType.CheckTheRoom);
            outmessage.Write(Username);
            outmessage.Write(roomIdSelected);
            outmessage.Write(v); // culoarea cu care vreau sa ma conectez

            _client.SendMessage(outmessage, NetDeliveryMethod.ReliableOrdered); //trimite la server
        }

        public void connectToRoom(string roomIdSelected)
        {
            var outmessage = _client.CreateMessage();
            outmessage.Write((byte)PacketType.ConnectToRoom);
            outmessage.Write(Username);
            outmessage.Write(roomIdSelected);
            
            _client.SendMessage(outmessage, NetDeliveryMethod.ReliableOrdered);



        }

        public void Update()    //este pus in game1 pentur a se apela la fiecare Frame
        {
            NetIncomingMessage incmessage;
            while ((incmessage = _client.ReadMessage()) != null)
            {
                switch (incmessage.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        Data(incmessage); // in functie de datele primite trimitem si inapoi ceea ce se doreste
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)incmessage.ReadByte())
                        {
                            case NetConnectionStatus.Disconnected:
                                Active = false;
                                break;

                        }
                        break;
                }
            }
        }

        public void DisconnectFromServer()
        {
            _client.Disconnect("Disconect");
        }

        public void DisconnectFromServer(string text)
        {
            _client.Disconnect(text);
        }

        private void Data(NetIncomingMessage incmessage)
        {
            var packageType = (PacketType)incmessage.ReadByte(); //se citeste ce fel de packet este
            switch (packageType)
            {
                case PacketType.PlayerPosition:
                    ReadPlayer(incmessage);
                    //ReadBoard(incmessage);
                    break;
                case PacketType.AllPlayers:
                    ReceiveAllPlayers(incmessage);
                    break;
                case PacketType.Kick:
                    ReceiveKick(incmessage);
                    break;
                case PacketType.UpdatedBoard:
                    ReadBoard(incmessage);
                    break;
                case PacketType.ValidMoves:
                    ReadValidMoves(incmessage);
                    break;
                case PacketType.CreateRoom:
                    ReceiveAllRoomsUpdated(incmessage);
                    break;
                case PacketType.MovePieceCommand:
                    validMoves = new List<(int, int)>();
                    ReadBoard(incmessage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void ReceiveKick(NetIncomingMessage inc)
        {
            var username = inc.ReadString();
            var player = Players.FirstOrDefault(p => p.Name == username);
            if (player != null)
            {
                Players.Remove(player);
            }
            if (username == Username)
            {
                _client.Disconnect("kick");
            }
        }

        private void ReadPlayer(NetIncomingMessage inc)
        {
            var player = new PlayerDetails();
            inc.ReadAllProperties(player);
            if (Players.Any(p => p.Name == player.Name))
            {
                var oldplayer = Players.FirstOrDefault(p => p.Name == player.Name);
                oldplayer.XPosition = player.XPosition;
                oldplayer.YPosition = player.YPosition;
            }
            else
            {
                Players.Add(player);
            }

        }

        private void ReadRoom(NetIncomingMessage inc)
        {
            var room = new Room();
            inc.ReadAllProperties(room);
            if (_rooms.Any(p => p.id == room.id))
            {
                var oldRoom = _rooms.FirstOrDefault(p => p.id == room.id);
                oldRoom._players = room._players;
                oldRoom.board = board;
            }
            else
            {
                _rooms.Add(room);
            }

        }

        private void ReceiveAllRooms(NetIncomingMessage message)
        {
            var count = message.ReadInt32();
            
            for (int n = 0; n < count; n++)
            {
                ReadRoom(message);
            }
        }

        private void ReceiveAllRoomsUpdated(NetIncomingMessage message)
        {
            var count = message.ReadInt32();
            if (count > _rooms.Count)
            {
                for (int n = 0; n < count; n++)
                {
                    ReadRoom(message);
                }
                RoomUpdatedEvent?.Invoke(this, new RoomUpdatedEventArgs(_rooms));
            }
        }

        private void ReadBoard(NetIncomingMessage inc)
        {


            string pathFilePiece = "D:\\JocDameM\\JocDameMultyplayer\\JocDameMultyplayer\\TestText\\testTest.txt";
            string pathFile = "D:\\JocDameM\\JocDameMultyplayer\\JocDameMultyplayer\\TestText\\testTest0.txt";
            StreamWriter writerp = new StreamWriter(pathFilePiece);
            StreamWriter writer0 = new StreamWriter(pathFile);

                for (int i = 0; i < board.ROWS; i++)
                {
                    for (int j = 0; j < board.COLS; j++)
                    {
                        string serializeditem = inc.ReadString();
                        
                        if (serializeditem == "0")
                        {
                            board.board[i, j] = int.Parse(serializeditem);
                            writer0.WriteLine("r = " + i + " c = " + j + " == "+serializeditem);
                        //board.changePiece(i, j, int.Parse(serializeditem));
                    }
                        else
                        {
                            //writere.WriteLine("Se citeste : ... " + serializeditem);
                            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(serializeditem);
                            int row = int.Parse(dictionary["row"].ToString());
                            int col = int.Parse(dictionary["col"].ToString());
                            string[] color = dictionary["color"].ToString().Split(", ");
                            Piece p = new Piece(row, col, Color.FromArgb(int.Parse(color[0]), int.Parse(color[1]), int.Parse(color[2])));
                            serializedBoard = " " + row + "  " + col + " culoare = " + color[0] + " " + color[1] + " " + color[2];
                            writerp.WriteLine(serializeditem);
                            //board.changePiece(i, j, p);
                            board.board[i, j] = p;
                            
                        }
                    }
                }
            writerp.WriteLine("S-a modificat mapa");
            writer0.WriteLine("S-a modificat mapa");
            writerp.Close();
            writer0.Close();
            
            //validMoves = new List<(int, int)>();
        }
        public void ReadValidMoves(NetIncomingMessage inc)
        {
            var serializedstring = inc.ReadString();
            //validMoves 
            var deseserealize = JsonConvert.DeserializeObject<List<(int, int)>>(serializedstring);
            validMoves = deseserealize;
            //  [{ "Item1":1,"Item2":4},{ "Item1":1,"Item2":6}]
            //public Dictionary<(int, int), List<Piece>> validMoves = new Dictionary<(int, int), List<Piece>>();

        }
        private void ReceiveAllPlayers(NetIncomingMessage message)
        {
            var count = message.ReadInt32();
            for ( int n = 0; n<count; n++)
            {
                ReadPlayer(message);
            }
        }

        public void SendInput(Keys key)
        {
            var outmessage = _client.CreateMessage();  //se creaza mesajul care se vrea sa se trimita la server
            outmessage.Write((byte)PacketType.Input);  // se incarca tipul de mesaj care se vrea sa se trimita, de data asta este de tip Input (adica date legate de input)
            outmessage.Write(Username);    // ii transmitem serverului numele nostru de client
            outmessage.Write((byte)key);            // Atasam si key-ia apasata de client 
            _client.SendMessage(outmessage, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendClickPosition(int row, int col)
        {
            var outmessage = _client.CreateMessage();
            outmessage.Write((byte)PacketType.ClickPos);
            outmessage.Write(Username);
            outmessage.Write(color.ToString());
            outmessage.Write(row);
            outmessage.Write(col);
            _client.SendMessage(outmessage, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendClickPositionToMove(int row, int col)
        {
            var outmessage = _client.CreateMessage();
            outmessage.Write((byte)PacketType.ClickPosForMoving);
            outmessage.Write(Username);
            outmessage.Write(color.ToString());/// culoarea player-ului care face mutarea
            outmessage.Write(row);
            outmessage.Write(col);
            _client.SendMessage(outmessage, NetDeliveryMethod.ReliableOrdered);
        }

        public void sendRoomCreated(string RoomID, string RoomName)
        {
            var outmessage = _client.CreateMessage();
            outmessage.Write((byte)PacketType.CreateRoom);
            outmessage.Write(Username);
            outmessage.Write(RoomID);
            outmessage.Write(RoomName);
            _client.SendMessage(outmessage, NetDeliveryMethod.ReliableOrdered); //trimite mesaj server-ului
        }
    }
}
