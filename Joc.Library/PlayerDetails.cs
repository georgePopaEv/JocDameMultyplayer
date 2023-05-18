using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joc.Library
{ 
        public class PlayerDetails
        {
            public string Name { get; set; }
            // public NetConnection Connection { get; set; }
            public int XPosition { get; set; }
            public int YPosition { get; set; }

            public PlayerDetails(string name, int xPosiion, int yPosiion)
            {
                Name = name;
                XPosition = xPosiion;
                YPosition = yPosiion;
            }
            public PlayerDetails() { }

            /* public void Move(Keys key)
             {

             }*/
        }
    
}
