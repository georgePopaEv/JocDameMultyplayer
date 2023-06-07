using System;
using System.Collections.Generic;
using System.Drawing;
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

            public Board board { get; set; }
            public List<(int, int)> validMoves { get; set; }
            public (int, int) selectedPiece { get; set; }
            public string color;


        public PlayerDetails(string name, int xPosiion, int yPosiion, string color)
            {
                Name = name;
                XPosition = xPosiion;
                YPosition = yPosiion;
                color = Color.FromArgb(0, 0, 0).ToString();
                selectedPiece = (-1, -1);
            }
            public PlayerDetails() { }

            /* public void Move(Keys key)
             {

             }*/
        }
    
}
