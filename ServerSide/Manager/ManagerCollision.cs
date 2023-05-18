using System;
using System.Collections.Generic;
using System.Text;
using Joc.Library;

using System.Windows.Forms;
using System.Drawing;

namespace ServerSide.Manager
{
    class ManagerCollision
    {
        public static bool CheckCollision(Rectangle rec, string username,  List<PlayerDetails> players)
        {
            foreach ( var player in players)
            {
                if(player.Name != username)
                {
                    var playerRec = new Rectangle(player.XPosition, player.YPosition, 50, 50);
                    
                    if (playerRec.IntersectsWith(rec))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
