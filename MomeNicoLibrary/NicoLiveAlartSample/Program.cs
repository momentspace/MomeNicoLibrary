using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using MomeNicoLibrary.NicoCommunity;
using MomeNicoLibrary.NicoLive;
using MomeNicoLibrary.Utility;
using MomeNicoLibrary.NicoLive.Information;
using System.Timers;

namespace NicoLiveAlertSample
{
	class Program
	{
		static void Main(string[] args)
		{
			Timer timer = new Timer();
			timer.Interval = 5000;
			timer.Elapsed += new ElapsedEventHandler(test);
			//timer.Start();

			NicoLiveAlert Alert = new NicoLiveAlert();
			const string mail = "nico@bloodpledge.info";
			const string password = "dpXLD6yH9knRHf7yIi6cyGR2WxDW7wmayyoKEtUeaEoxqA8GDIjg8j+5lAOz/6DZg4fgAdnEo3yUTH4C" +
				"HyrTDu0pP8yj5evqZpWPhFJcZnXELsE/qE2xwNNqcC+BtPnBOHxRjFb9bru/BQrvyEiWQ6ADJOdw58BI" +
				"yt4hdGZp4oY=";
			string ticket = Alert.Login(mail, RSALib.Decrypt(password, RSAKey.SecretKey));
			Alert.AlertEvent += new NicoLiveAlert.AlertEventHandler(callback);
//			Alert.AddObserver(callback);
			Alert.Alert(ticket);
			
		}

		static private object obj = new object();
		public static void callback(AlertInformation ai)
		{
			Information info = new Information();

			NicoCommunity nc = new NicoCommunity();
			NicoCommunity.CommunityInfo ci = nc.GetCommunityInfo(ai.BroadcastId);

			StringBuilder sb = new StringBuilder();
			sb.Append(ai.BroadcastId).Append(",")
				.Append(ai.CasterId).Append(",")
				.Append(ai.ChannelId).Append("\t: ")
				.Append(ci.title);
			Console.WriteLine(sb);
			if (ci.title.IndexOf("雑談") != -1)
			{
				info.ci = ci;
				info.ai = ai;
				lock (obj)
				{
					queue.Enqueue(info);
				}
			}
		}

		static private Queue<Information> queue = new Queue<Information>();

		public static void test(object o, ElapsedEventArgs e)
		{
			int size;
			Information info = new Information();
			AlertInformation ai;
			NicoCommunity.CommunityInfo ci;
			lock (obj)
			{
				size = queue.Count();
				if (size != 0)
				{
					info = queue.Dequeue();
				}
			}

			ai = info.ai;
			ci = info.ci;

			if (size != 0)
			{
				StringBuilder sb = new StringBuilder();
				TcpClient tcp = new TcpClient("localhost", 9821);
				Stream s = tcp.GetStream();
				StreamWriter sw = new StreamWriter(s);
				StreamReader sr = new StreamReader(s);
				//sw.Write(sb);
				//sw.Flush();
				char[] c = new char[512];
				//sr.Read(c, 0, c.Length);
				//Console.WriteLine(c);
				//tcp.Close();

				//tcp = new TcpClient("localhost", 9821);
				//s = tcp.GetStream();
				//sw = new StreamWriter(s);
				//sr = new StreamReader(s);
				sb = new StringBuilder();
				sb.Append("SEND SSTP/1.1\r\n");
				sb.Append("Sender: MomeNicoLibrary\r\n");
				sb.Append("Script: \\0\\s0MoeNicoLiveAlert: " + ci.title + "\\e\r\n");
				sb.Append("Option: nodescript,notranslate\r\n");
				sb.Append("Charset: UTF-8\r\n\r\n");
				sw.Write(sb);
				sw.Flush();
				sr.Read(c, 0, c.Length);
//				Console.WriteLine(c);
				tcp.Close();
			}
		}

		class Information
		{
			public AlertInformation ai;
			public NicoCommunity.CommunityInfo ci;
		}
	}
}
