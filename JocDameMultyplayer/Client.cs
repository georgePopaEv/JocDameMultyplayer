using System;
using System.Collections.Generic;
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
            var loginInformarion = new LoginInformation() { Name = "JocDeDame" };
            _client = new NetClient(new NetPeerConfiguration("JocDeDame"));
            _client.Start();
            var outmsg = _client.CreateMessage();
            Console.WriteLine(outmsg);
            outmsg.Write((byte)PacketType.Login);
            outmsg.WriteAllProperties(loginInformarion);
            Console.WriteLine(outmsg.ToString());
            _client.Connect("localhost", 14242, outmsg);
            return EstablishInfo();

        }

        private bool EstablishInfo()
        {
            var time = DateTime.Now;
            NetIncomingMessage incmessage;
            while (true)
            {
                /*if(DateTime.Now.Subtract(time).Seconds > 5)
                {
                    return false;

                }*/
                Console.WriteLine("Teste");

                while((incmessage = _client.ReadMessage()) != null)
                {
                    switch (incmessage.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            Console.WriteLine("Primit mesaj de la Server");
                            var data_from_server = incmessage.ReadByte();
                            if(data_from_server == (byte)PacketType.Login)
                            {
                                var accepted = incmessage.ReadBoolean(); // pentru a primi acceptul de la server
                                if (accepted)
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
