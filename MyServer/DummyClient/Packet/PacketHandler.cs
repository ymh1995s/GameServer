﻿using DummyClient;
using MyServerCore;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
    public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
    {
        //S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
        //ServerSession serverSession = session as ServerSession;
    }

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        //S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
        //ServerSession serverSession = session as ServerSession;
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        //S_PlayerList pkt = packet as S_PlayerList;
        //ServerSession serverSession = session as ServerSession;
    }

    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        //S_BroadcastMove pkt = packet as S_BroadcastMove;
        //ServerSession serverSession = session as ServerSession;
    }
}