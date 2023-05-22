using Joc.Library;
using Lidgren.Network;
using ServerSide.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerSide.Commands
{
    interface ICommand
    {
        void Run(ManagerLogger managerLorgger, Server server, NetIncomingMessage inc, PlayerDetails player, List<PlayerDetails> players);
    }
}
