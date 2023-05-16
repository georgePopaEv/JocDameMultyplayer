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

        public bool Start()
        {
            var player = new PlayerDetails() { Name = "Razvan" }; // Se creaza un player cu numele x
            _client = new NetClient(new NetPeerConfiguration("JocDeDame")); //se creaza client-ul connectat la server
            _client.Start();  // se porneste clientul
            var outmsg = _client.CreateMessage();   //se creaza un mesaj din partea clientului
            Console.WriteLine(outmsg);              //Se Afiseaza in consola mesajul tocmai creat de client
            outmsg.Write((byte)PacketType.Login);   //se scrie in mesaj ce tip de packet de doreste, iar in acest caz este un packet de type Login
            outmsg.WriteAllProperties(player);      // Dupa ce ii trimitem server-
            Console.WriteLine(outmsg.ToString());   // SE afiseaza pe consola ce mesaj s-a creat
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
                                var accepted = incmessage.ReadBoolean(); // daca este de tip login atunci citim urmatorul packet care ar trebui sa fie approv-ul de la server
                                if (accepted)   // daca este pozitiv atunci pornim clientul
                                {
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
