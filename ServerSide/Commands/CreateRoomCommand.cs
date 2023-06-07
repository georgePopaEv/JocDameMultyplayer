using Joc.Library;
using Lidgren.Network;
using ServerSide.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Commands
{
    class CreateRoomCommand : ICommand
    {
        public void Run(ManagerLogger managerLorgger, Server server, NetIncomingMessage inc, PlayerAndConnection playerAndConnection, List<PlayerAndConnection> players = null)
        {
            
            var username = inc.ReadString();
            var roomID = inc.ReadString();
            var roomName = inc.ReadString();
            var room = server._rooms.FirstOrDefault(p => p.id == roomID);
            if (room is null)
            {
                room = createRoom(roomID, roomName);
                server._rooms.Add(room);
            }
            //else -ul il negam direct din scrierea in consola daca exista asa ceva
            managerLorgger.AddLogMessage("server", "Room Created, trimitem la client lista de Camere");
        }

        public Room createRoom(string roomID, string roomName)
        {
            var room1 = new Room
            {
                id = roomID,
                Name = roomName,
                board = new Board(),
                _players = new List<PlayerAndConnection>()

            };
            return room1;
        }
    }
}
