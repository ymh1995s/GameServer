using MyServer.Game;
using MyServerCore;
using System.Net;

namespace MyServer
{
    internal class Program
    {
        static Listener _listener = new Listener();


        static void TickRoom(GameRoom room, int tick = 100)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = tick;
            timer.Elapsed += ((s, e) => { room.Update(); });
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        static void Main(string[] args)
        {
            GameRoom room = RoomManager.Instance.Add(1);
            TickRoom(room, 25);

            // DNS (Domain Name System)
            //string host = Dns.GetHostName();
            //IPHostEntry ipHost = Dns.GetHostEntry(host);
            //IPAddress ipAddr = ipHost.AddressList[0];
            //IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); 
            string ipAddressString = "25.30.25.192";
            IPAddress ipAddress = IPAddress.Parse(ipAddressString);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 7777);


            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Server Listening...");

            while (true)
            {
                Thread.Sleep(10000);
            }            
        }
    }
}