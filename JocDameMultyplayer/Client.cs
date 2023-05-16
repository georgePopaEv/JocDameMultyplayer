using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using LibrariaMea;
using Lidgren.Network;

namespace JocDameMultyplayer
{
    class Client
    {
        private NetClient _client;
        public PlayerDetails PlayerDetails { get; set; }
        public bool Active { get; set; }

        public bool Start()
        {
            var random = new Random();
            _client = new NetClient(new NetPeerConfiguration("JocDeDame")); //se creaza client-ul connectat la server
            _client.Start();  // se porneste clientul

            PlayerDetails = new PlayerDetails("name_" + random.Next(0, 100),0,0); // Se creaza un player cu numele x

            var outmsg = _client.CreateMessage();   //se creaza un mesaj din partea clientului
            outmsg.Write((byte)PacketType.Login);   //se scrie in mesaj ce tip de packet de doreste, iar in acest caz este un packet de type Login
            outmsg.Write(PlayerDetails.Name); 
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
                                    PlayerDetails.XPosiion = incmessage.ReadInt32(); 
                                    PlayerDetails.YPosiion = incmessage.ReadInt32(); 
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
    }
}
