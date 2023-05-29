using System.Drawing;

namespace Joc.Library
{
    public class Constants
    {
        public readonly int HEIGHT = 800;
        public readonly int PADDING = 25;
        public readonly int OUTLINE = 2;
        public readonly int WIDTH = 800;
        public readonly int ROWS = 8;
        public readonly int COLS = 8;
        public readonly int SQUARE_SIZE = 800 / 8;
        public readonly Color RED = Color.FromArgb(255, 0, 0);
        public readonly Color WHITE = Color.FromArgb(255, 255, 255);
        public readonly Color BLACK = Color.FromArgb(0, 0, 0);
        public readonly Color BLUE = Color.FromArgb(0, 0, 255);
        public readonly Color GREY = Color.FromArgb(128, 128, 128);

        public Constants()
        {
        }
        
        public Constants(int hEIGHT, int pADDING, int oUTLINE, int wIDTH, int rOWS, int cOLS, int sQUARE_SIZE, Color rED, Color wHITE, Color bLACK, Color bLUE, Color gREY)
        {
            HEIGHT = hEIGHT;
            PADDING = pADDING;
            OUTLINE = oUTLINE;
            WIDTH = wIDTH;
            ROWS = rOWS;
            COLS = cOLS;
            SQUARE_SIZE = sQUARE_SIZE;
            RED = rED;
            WHITE = wHITE;
            BLACK = bLACK;
            BLUE = bLUE;
            GREY = gREY;
        }


    }
}