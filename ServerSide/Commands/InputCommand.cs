using Joc.Library;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using ServerSide.Manager;
using System.Text;
using System.Drawing;
using System.Windows.Forms;


namespace ServerSide.Commands
{
    class InputCommand : ICommand
    {
        public void Run(ManagerLogger managerLorgger, NetServer server, NetIncomingMessage inc, PlayerDetails player, List<PlayerDetails> players)
        {
            managerLorgger.AddLogMessage("server", "Received new Input");
            var name = inc.ReadString();                  // primeste numele clientului
            var key = (Keys)inc.ReadByte();           //se citeste keya apasata de catre client
            player = players.FirstOrDefault(p => p.Name == name);
            if (player == null)
            {
                managerLorgger.AddLogMessage("server", string.Format("NU am gasit player cu acest nume{0}", name));
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
            command.Run(managerLorgger, server, inc, player, players);
        }
    }
}
