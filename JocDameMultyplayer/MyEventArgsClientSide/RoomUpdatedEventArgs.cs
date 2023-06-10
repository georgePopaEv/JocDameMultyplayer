using Joc.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JocDameMultyplayer.MyEventArgsClientSide
{
    public class RoomUpdatedEventArgs :EventArgs
    {
        public List<Room> listRooms { get; set; }
        public RoomUpdatedEventArgs(List<Room> rooms)
        {
            listRooms = rooms;
        }
    }
}
