using System;
using System.Collections.Generic;
using System.Text;
using LibrariaMea;

using Microsoft.Xna.Framework;

namespace Server.Manager
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
                    //if (playerRec.Intersects(rec))
                    //{
                        return true;
                    //}
                }
                
            }
            return false;
        }
    }
}
