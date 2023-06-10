using ServerSide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joc.Library
{
    public class Room
    {
        public string id { get; set; }
        public string Name { get; set; }

        public Board board { get; set; }

        public List<PlayerAndConnection> _players { get; set; }

        public Room(string Id, string NameRoom)
        {
            board = new Board(); // se creaza mapa pentru fiecare room
            id = Id;
            Name = NameRoom;
            _players = new List<PlayerAndConnection>();
        }

        public Room()
        {
            _players = new List<PlayerAndConnection>();
        }
    }
}
