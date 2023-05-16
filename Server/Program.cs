using System;
using System.Threading;
using LibrariaMea;
using Lidgren.Network;
namespace Server
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var configuratie = new NetPeerConfiguration("JocDeDame") { Port= 14242};
            configuratie.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            var server = new NetServer(configuratie);
            server.Start();
            Console.WriteLine("Serverul a pornit");
            int s = 0;
            while (true)
            {
                NetIncomingMessage incmesage;
                /*incmesage = server.ReadMessage();*/
                
                /*
                Console.WriteLine("While Loop: inainte de  verificare mesaj" + s );
                s++;*/
                while ((incmesage = server.ReadMessage()) != null)
                {
                    
                    /*Console.WriteLine("While Loop: inainte de  verificare mesaj --->>>" + incmesage.ReadByte());*/
                    switch (incmesage.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            Console.WriteLine("Client Connectat: " + incmesage.SenderConnection.RemoteEndPoint);
                            var data = incmesage.ReadByte();
                            if (data == (byte)PacketType.Login)
                            {
                                var loginInformation = new PlayerDetails();
                                incmesage.ReadAllProperties(loginInformation);
                                incmesage.SenderConnection.Approve();
                                
                                var outmsg = server.CreateMessage();
                                outmsg.Write((byte)PacketType.Login);
                                outmsg.Write(true);
                                Console.WriteLine("..Created message " + outmsg.ToString());

                                Thread.Sleep(500);
                                server.SendMessage(outmsg, incmesage.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                                
                            }
                            else
                            {
                                incmesage.SenderConnection.Deny("Didn't sent correct information.");
                            }

                            break;
                        case NetIncomingMessageType.Data:
                            Console.WriteLine("Mesaj primit de la Client" + incmesage.ReadString());
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            var status = (NetConnectionStatus) incmesage.ReadByte();
                            Console.WriteLine("Status Conexiune Schimbata " + status.ToString() + " (" + incmesage.ReadString() + ") " + incmesage.ToString());
                            break;
                        default:
                            Console.WriteLine("Mesaj necunoscut de la client: " + incmesage.MessageType);
                            break;
                    }
                }

            }

        }
    }
}
