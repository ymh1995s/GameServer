using MyServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Google.Protobuf;

public class NetworkManager
{
	ServerSession _session = new ServerSession();

	public void Send(IMessage packet)
	{
		_session.Send(packet);
	}

	public void Init()
	{
		// DNS (Domain Name System)
		//      string host = Dns.GetHostName();
		//IPHostEntry ipHost = Dns.GetHostEntry(host);
		//IPAddress ipAddr = ipHost.AddressList[4];
		string ipAddressString = "25.30.25.192";
		//string ipAddressString = "192.168.219.102";
		IPAddress ipAddress = IPAddress.Parse(ipAddressString);
        IPEndPoint endPoint = new IPEndPoint(ipAddress, 7777);


		Connector connector = new Connector();

		connector.Connect(endPoint,
			() => { return _session; },
			1);
	}

	public void Update()
	{
		List<PacketMessage> list = PacketQueue.Instance.PopAll();
		foreach (PacketMessage packet in list)
		{
			Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.Id);
			if (handler != null)
				handler.Invoke(_session, packet.Message);
		}	
	}

}
