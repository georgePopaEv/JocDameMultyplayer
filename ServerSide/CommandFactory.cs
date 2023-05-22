using ServerSide.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerSide
{
    class CommandFactory
    {
        public static ICommand GetCommand(PacketType packetType)
        {
            switch (packetType)
            {
                case PacketType.Login:
                    return new LoginCommand1();
                    break;
                case PacketType.PlayerPosition:
                    return new PlayerPositionCommand();
                    break;
                case PacketType.AllPlayers:
                    return new AllPlayersCommand();
                    break;
                case PacketType.Input:
                    return new InputCommand();
                    break;
                case PacketType.Kick:
                    return new KickPlayerCommand();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("packettType");
                    break;
            }
            
        }
    }
}
