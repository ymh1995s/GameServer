using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace MyServerCore
{
    public class Listener
    {
        Socket _listenSocket;        
        Func<Session> _sessionFactory; // Accept 완료 후 행동 / Return Type가 있는 Delegate / Session Type return.

        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory, int register = 10, int backlog = 100)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory += sessionFactory;

            _listenSocket.Bind(endPoint);

            // backlog : 최대 대기수
            _listenSocket.Listen(backlog);

            for (int i = 0; i < register; i++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
                RegisterAccept(args);
            }
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            //남아 있는 이전 연결을 초기화
            args.AcceptSocket = null;

            bool pending = _listenSocket.AcceptAsync(args);
            //true : 보류 / false : 동기적으로 완료(바로 완료된 경우)
            if (pending == false)
                OnAcceptCompleted(null, args);
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                //Accept 완료 후 행동
                Session session = _sessionFactory.Invoke();
                session.Start(args.AcceptSocket);
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);
            }
            else
            {
                Console.WriteLine(args.SocketError.ToString());
            }

            //등록 완료 후 다음 등록 대기
            RegisterAccept(args);
        }
    }
}
