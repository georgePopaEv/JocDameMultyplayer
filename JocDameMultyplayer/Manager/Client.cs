using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Joc.Library;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;

namespace JocDameMultyplayer
{
    public class Client
    {
        private NetClient _client;
        public List<PlayerDetails> Players { get; set; }
        public string Username { get; set; }
        public bool Active { get; set; }
        public Board Board;
        public Client()
        {
            Players = new List<PlayerDetails>();
            Board = new Board();
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

        public void Update()
        {
            NetIncomingMessage incmessage;
            while ((incmessage = _client.ReadMessage()) != null)
            {
                switch (incmessage.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        Data(incmessage);
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
        private void Data(NetIncomingMessage incmessage)
        {
            var packageType = (PacketType)incmessage.ReadByte(); //se citeste ce fel de packet este
            switch (packageType)
            {
                case PacketType.PlayerPosition:
                    ReadPlayer(incmessage);
                    break;

                case PacketType.AllPlayers:
                    ReceiveAllPlayers(incmessage);
                    break;

                case PacketType.Kick:
                    ReceiveKick(incmessage);
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
    }
}
