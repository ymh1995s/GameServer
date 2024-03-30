using Google.Protobuf.Protocol;
using Google.Protobuf;
using MyServer;
using MyServerCore;
using MyServer.Game.Obejct;
using MyServer.Game;

class PacketHandler
{
    static Random random = new Random();
    public static void C_MoveHandler(PacketSession session, IMessage packet)
    {
        C_Move movePacket = packet as C_Move;
        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if (room == null)
            return;

        room.Push(room.HandleMove, player, movePacket);
    }

    public static void C_ArrowHandler(PacketSession session, IMessage packet)
    {
        C_Arrow arrowPacket = packet as C_Arrow;

        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if (room == null)
            return;

        room.Push(room.HandleArrow, arrowPacket);
    }

    public static void C_DieHandler(PacketSession session, IMessage packet)
    {
        C_Die diePacket = packet as C_Die;

        ClientSession clientSession = session as ClientSession;

        GameObject go = clientSession.MyPlayer;
        if (go == null)
            return;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if (room == null)
            return;

        //player.PosInfo.PosX = 10;와 혼동 없이 싱크를 맞추기에 주의한다.
        player.Info.PosInfo.PosX = (float)(random.NextDouble() * 20.0 - 10.0);
        player.Info.PosInfo.PosY = (float)(random.NextDouble() * 20.0 - 10.0);
        Console.WriteLine(player.Info.PosInfo.PosX);
        Console.WriteLine(player.Info.PosInfo.PosY);

        room.Push(room.HandleDie, diePacket);
        room.Push(room.LeaveGame, player.Info.ObjectId);
        Thread.Sleep(500);
        room.Push(room.EnterGame, player);
    }

}