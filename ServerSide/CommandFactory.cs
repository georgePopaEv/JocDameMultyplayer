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
            switch (packetType) // tipurile de date care se pot primi de la client //
            {
                case PacketType.Login:      //de login , unde clientul vrea sa se logheze
                    return new LoginCommand1();
                    break;
                case PacketType.PlayerPosition:     //de update a pozitiei acestuia
                    return new PlayerPositionCommand(); 
                    break;
                case PacketType.AllPlayers: //datele despre toti playerii
                    return new AllPlayersCommand();
                    break;
                case PacketType.Input:
                    return new InputCommand();
                    break;
                case PacketType.Kick:
                    return new KickPlayerCommand();
                case PacketType.ClickPos:
                    return new InputMouseCommand();
                    break;
                case PacketType.CreateRoom:
                    return new CreateRoomCommand();
                    break;
                case PacketType.ClickPosForMoving:
                    return new MovePieceCommand();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("packettType");
                    break;
            }
            
        }
    }
}
