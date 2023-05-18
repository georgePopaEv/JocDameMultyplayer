﻿using Joc.Library;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerSide.Commands
{
    interface ICommand
    {
        void Run(NetServer server, NetIncomingMessage inc, PlayerDetails player, List<PlayerDetails> players);
    }
}