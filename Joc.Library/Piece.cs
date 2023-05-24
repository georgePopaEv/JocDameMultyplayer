using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Joc.Library
{
    public class Piece : Constants
    {

        public int row { get; set; }
        public int col { get; set; }
        public Color color { get; set; }
        public bool king { get; set; }
        public bool shield { get; set; }
        private int posX { get; set; }
        private int posY { get; set; }

        public Piece(int row, int col, Color color)
        {
            this.row = row;
            this.col = col;
            this.color = color;
            this.king = false;
            this.shield = false;
            this.posX = 0;
            this.posY = 0;
            calc_pos();
        }

        public void calc_pos()
        {
            this.posX = SQUARE_SIZE * this.col + SQUARE_SIZE / 2;
            this.posY = SQUARE_SIZE * this.row + SQUARE_SIZE / 2;

        }

        public void make_King()
        {
            this.king = true;
        }
        public void make_Shield()
        {
            this.shield= true;
        }

        public void move(int row, int col)
        {
            this.row = row;
            this.col = col;
            calc_pos();
            make_Shield();
        }

        public (int, int) get_Rect()
        {
            return (posX, posY);
        }

        public override string ToString()
        {
            return color.ToString();
        }
    }
}
