﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using Server.Data;
using Server.Game;
using ServerCore;

namespace Server
{
	class Program
	{
		static Listener _listener = new Listener();

		static void FlushRoom()
		{
			JobTimer.Instance.Push(FlushRoom, 250);
		}

		static void Main(string[] args)
		{

			ConfigManager.LoadConfig();
			DataManager.LoadData();
			var d = DataManager.StatDict;

			RoomManager.Instance.Add(1);

			// DNS (Domain Name System)
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAddr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening...");

			//FlushRoom();
			//JobTimer.Instance.Push(FlushRoom);

			// TODO
			while (true)
			{
				//JobTimer.Instance.Flush();
				GameRoom room = RoomManager.Instance.Find(1);
				room.Push(room.Update);
				//Thread.Sleep(100);
			}
		}
	}
}
