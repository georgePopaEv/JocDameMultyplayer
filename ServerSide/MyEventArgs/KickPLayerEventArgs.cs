using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.MyEventArgs
{
    class KickPLayerEventArgs :EventArgs
    {
        public string Username { get; set; }
        public KickPLayerEventArgs(string username)
        {
            Username = username;
        }
    }
}
