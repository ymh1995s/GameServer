using Google.Protobuf;
using Google.Protobuf.Protocol;
using MyServer.Game.Obejct;
using System;
using System.Net.Sockets;
using System.Numerics;
using static System.Net.WebRequestMethods;

namespace MyServer.Game
{
    public class GameRoom : JobSerializer
    {
        public int RoomId { get; set; }

        Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public void Init(int mapId)
        {

        }

        // 누군가 주기적으로 호출해줘야 한다
        public void Update()
        {
            Flush();
        }

        public void EnterGame(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            GameObjectType type = ObjectManager.GetObjectTypeById(gameObject.Id);

            if (type == GameObjectType.Player)
            {
                Player player = gameObject as Player;
                _players.Add(gameObject.Id, player);
                player.Room = this;

                // 본인한테 정보 전송
                {
                    S_EnterGame enterPacket = new S_EnterGame();
                    enterPacket.Player = player.Info;
                    player.Session.Send(enterPacket);

                    S_Spawn spawnPacket = new S_Spawn();
                    foreach (Player p in _players.Values)
                    {
                        if (player != p)
                            spawnPacket.Objects.Add(p.Info);
                    }

                    player.Session.Send(spawnPacket);
                    Console.WriteLine( $"Enter Packet Send {player.Id}" );
                }
            }
            // 타인한테 정보 전송
            {
                S_Spawn spawnPacket = new S_Spawn();
                spawnPacket.Objects.Add(gameObject.Info);
                foreach (Player p in _players.Values)
                {
                    if (p.Id != gameObject.Id)
                        p.Session.Send(spawnPacket);
                }
                Console.WriteLine($"Spawn Packet Broadcast");
            }
        }

        public void LeaveGame(int objectId)
        {
            GameObjectType type = ObjectManager.GetObjectTypeById(objectId);

            if (type == GameObjectType.Player)
            {
                Player player = null;
                if (_players.Remove(objectId, out player) == false)
                    return;

                player.Room = null;

                // 본인한테 정보 전송
                {
                    S_LeaveGame leavePacket = new S_LeaveGame();
                    player.Session.Send(leavePacket);
                    Console.WriteLine($"Enter Packet Send {player.Id}");
                }
            }

            // 타인한테 정보 전송
            {
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.ObjectIds.Add(objectId);
                foreach (Player p in _players.Values)
                {
                    if (p.Id != objectId)
                        p.Session.Send(despawnPacket);
                }
            }
            Console.WriteLine($"DeSpawn Packet Broadcast");
        }


        public void HandleMove(Player player, C_Move movePacket)
        {
            if (player == null)
                return;

            // TODO : 검증
            PositionInfo movePosInfo = movePacket.PosInfo;
            ObjectInfo info = player.Info;

            //서버의 위치 기억
            info.PosInfo = movePacket.PosInfo;

            // 다른 플레이어한테도 알려준다
            S_Move resMovePacket = new S_Move();
            resMovePacket.IsLeft = movePacket.IsLeft;
            resMovePacket.ObjectId = player.Info.ObjectId;
            resMovePacket.PosInfo = movePacket.PosInfo;

            Console.WriteLine($"Move Packet Broadcast");

            Broadcast(resMovePacket);
        }

        public void HandleArrow(C_Arrow arrowPacket)
        {
            // 다른 플레이어한테도 알려준다
            S_Arrow resArrowPacket = new S_Arrow();
            resArrowPacket.Owner = arrowPacket.Owner;
            resArrowPacket.XVec = arrowPacket.XVec;
            resArrowPacket.YVec = arrowPacket.YVec;

            foreach (Player p in _players.Values)
            {
                p.Session.Send(resArrowPacket);
            }

            Console.WriteLine($"Attack Packet Broadcast");
        }

        public void HandleDie(C_Die diePacket)
        {
            // 다른 플레이어한테도 알려준다
            S_Die resDiePacket = new S_Die();
            resDiePacket.ObjectId = diePacket.ObjectId;
            resDiePacket.AttackerId = diePacket.AttackerId;

            foreach (Player p in _players.Values)
            {
                p.Session.Send(resDiePacket);
            }

            Console.WriteLine($"Die Packet Broadcast");
        }

        public Player FindPlayer(Func<GameObject, bool> condition)
        {
            foreach (Player player in _players.Values)
            {
                if (condition.Invoke(player))
                    return player;
            }
            return null;
        }

        public void Broadcast(IMessage packet)
        {
            foreach (Player p in _players.Values)
                p.Session.Send(packet);
        }
    }
}
