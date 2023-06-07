using Joc.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace ServerSide
{
    public class PlayerAndConnection
    {
        public PlayerDetails PlayerDetails { get; set; }
        public NetConnection Connection { get; set; }

        public PlayerAndConnection(PlayerDetails playerDetails, NetConnection connection)
        {
            PlayerDetails = playerDetails;
            Connection = connection;
        }
    }
}
