﻿using LibrariaMea;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using Server.Manager;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Server.Commands
{
    class InputCommand : ICommand
    {
        public void Run(NetServer server, NetIncomingMessage inc, PlayerDetails player, List<PlayerDetails> players)
        {
            Console.WriteLine("Received new Input");
            var name = inc.ReadString();                  // primeste numele clientului
            var key = (Keys)inc.ReadByte();           //se citeste keya apasata de catre client
            player = players.FirstOrDefault(p => p.Name == name);
            if (player == null)
            {
                Console.WriteLine("NU am gasit player cu acest nume{0}", name);
                return;
            }
            int x = 0;
            int y = 0;

            switch (key)                // in functie de tasta apasata se ia o anumita decizie de catra server
            {
                case Keys.Down:
                case Keys.S:
                    y++;
                    break;
                case Keys.Up:
                case Keys.W:
                    y--;
                    break;
                case Keys.Right:
                case Keys.D:
                    x++;
                    break;
                case Keys.Left:
                case Keys.A:
                    x--;
                    break;
                default:
                    break;
            }

            if (!ManagerCollision.CheckCollision(new Rectangle(player.XPosition + x, player.YPosition + y, 50, 50), player.Name, players))
            {
                player.XPosition += x;
                player.YPosition += y;
            }
            var command = new PlayerPositionCommand();
            command.Run(server, inc, player, players);
        }
    }
}