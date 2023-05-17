using System;
using Lidgren.Network;

namespace LibrariaMea
{
    public class PlayerDetails
    {
        public string Name { get; set; }
        // public NetConnection Connection { get; set; }
        public int XPosiion { get; set; }
        public int YPosiion { get; set; } 

        public PlayerDetails(string name, int xPosiion, int yPosiion)
        {
            Name = name;
            XPosiion = xPosiion;
            YPosiion = yPosiion;
        }
        public PlayerDetails() { }

       /* public void Move(Keys key)
        {

        }*/
    }
}
