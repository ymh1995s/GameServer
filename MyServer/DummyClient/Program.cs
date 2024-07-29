
using MyServerCore;
using System.Net;

namespace DummyClient
{
    internal class Program
    {

        static void Main(string[] args)
        {
            string ipAddressString = "25.30.25.192";
            IPAddress ipAddress = IPAddress.Parse(ipAddressString);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 7777);

            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return SessionManager.Instance.Generate(); },10);

            while (true)
            {
            
            }
        }
    }
}