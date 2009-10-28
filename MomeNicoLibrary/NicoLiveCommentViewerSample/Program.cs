using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Web;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Net.Sockets;
using MomeNicoLibrary.NicoLive;
using MomeNicoLibrary.NicoLive.Information;
using MomeNicoLibrary.Utility;

namespace NicoLiveCommentViewer
{
	class Program
	{

		static void Main(string[] args)
		{
			// ログインAPI その１（チケット取得）
			const string mail = "nico@bloodpledge.info";
			const string password = "dpXLD6yH9knRHf7yIi6cyGR2WxDW7wmayyoKEtUeaEoxqA8GDIjg8j+5lAOz/6DZg4fgAdnEo3yUTH4C" +
				"HyrTDu0pP8yj5evqZpWPhFJcZnXELsE/qE2xwNNqcC+BtPnBOHxRjFb9bru/BQrvyEiWQ6ADJOdw58BI" +
				"yt4hdGZp4oY=";

			NicoLiveBroadcast broadcast = new NicoLiveBroadcast("lv5947260", mail, password, RSAKey.SecretKey);
			broadcast.CommentReceivedEvent += new NicoLiveBroadcast.CommentReceivedEventHandler(callback);
//			broadcast.AddObserver(Program.callback);
			broadcast.Start();
		}

		public static void callback(BroadcastComment comment)
		{
			Console.WriteLine(new StringBuilder()
				.Append(comment.Number).Append(" ")
				.Append(comment.ElapsedTime.ToString()).Append(" ")
				.Append(comment.Premium ? "P" : "N" ).Append(" ")
				.Append(comment.UserId).Append(" ")
				.Append(comment.Comment).Append(" ")
			);
			return;

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
			sb.Append("Script: \\0\\s0" + comment.Comment + "\\e\r\n");
			sb.Append("Option: nodescript,notranslate\r\n");
			sb.Append("Charset: UTF-8\r\n\r\n");
			sw.Write(sb);
			sw.Flush();
			sr.Read(c, 0, c.Length);
			Console.WriteLine(c);
			tcp.Close();

		}
	}
}

