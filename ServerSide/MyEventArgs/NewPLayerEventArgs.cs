using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.MyEventArgs
{
    class NewPLayerEventArgs : EventArgs
    {
        public string Username { get; set; }

        public NewPLayerEventArgs(string username)
        {
            Username = username;
        }
    }
}
