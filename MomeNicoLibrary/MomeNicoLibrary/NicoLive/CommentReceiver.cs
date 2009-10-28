using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using MomeNicoLibrary.NicoLive.Information;

namespace MomeNicoLibrary.NicoLive
{
	/// <summary>
	/// コメント受信クラス
	/// </summary>
	public class CommentReceiver
	{
		ServerInformation si;

		public CommentReceiver(ServerInformation si)
		{
			this.si = si;
		}

		public void Open()
		{
			tcp = new System.Net.Sockets.TcpClient(si.Address, si.Port);
		}

		System.Net.Sockets.TcpClient tcp;
		public void Initialize(int resFrom)
		{
			const string version = "20061206";
			string t = string.Format("<thread thread=\"{0}\" version=\"{1}\" res_from=\"{2}\"/>\0", si.Thread, version, resFrom);
			Stream ts = tcp.GetStream();
			byte[] b = Encoding.ASCII.GetBytes(t);
			ts.Write(b, 0, b.Length);

			// レスポンスヘッダ受信
			int buf;
			List<byte> bufs = new List<byte>();

			// <thread last_res="29" resultcode="0" revision="1"
			//  server_time="1256562642" thread="1007391554" ticket="0x8a9ea98"/>\0
			buf = ts.ReadByte();
			while (buf != -1 && buf != '\0')
			{
				bufs.Add((byte)buf);
				buf = ts.ReadByte();
			}

			try
			{
				string ln = Encoding.UTF8.GetString(bufs.ToArray());
				Console.WriteLine(ln);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.StackTrace);
			}
		}

		public void Receive()
		{
			Stream ts = tcp.GetStream();
			string ln = "";
			string co = "";

			// 接続が切れるまで継続
			while (tcp.Connected)
			{
				// 受信バッファ
				byte[] bf = new byte[512];

				// 1回目の読み込み
				int size = ts.Read(bf, 0, bf.Length);

				// 前回値の追加（coは前回取得の残り）
				ln = co;

				//"<chat> ～ </chat>\0<chat> ～ </chat>\0<chat> ～ "
				//"</chat>\0"

				// 受信文字列を追加
				ln += Encoding.UTF8.GetString(bf);
				while (ln.IndexOf("\0") == -1)
				{
					bf = new byte[512];
					size = ts.Read(bf, 0, bf.Length);
					ln += Encoding.UTF8.GetString(bf);
				}

				// \0をtrimしてラストの\0より右に文字があれば次回分に回す
				ln = ln.Trim('\0');
				if (!ln.EndsWith("</chat>"))
				{
					int len = ln.Length;
					int loc = ln.LastIndexOf('\0');
					// 最後のバッファが\0のみだったとき
					if (len != 0)
					{
						co = ln.Substring(loc, len - loc);
						ln = ln.Substring(0, ln.LastIndexOf('\0'));
					}
					else
					{
						co = "";
					}
				}
				else
				{
					co = "";
				}

				// 区切り文字で区切って通知する
				foreach (string re in ln.Split('\0'))
				{
					ReceivedEvent(re);
				}
			}
		}

		public delegate void ReceivedEventHandler(string message);
		public event ReceivedEventHandler ReceivedEvent = PressHolder;
		private static void PressHolder(string message) { }

		public void Close()
		{
			tcp.Close();
		}

		//public delegate void CommentCallback(string commentLine);
		//private CommentCallback callback;

		//public void AddObserver(CommentCallback callback)
		//{
		//    this.callback += callback;
		//}

		//public void DelObserver(CommentCallback callback)
		//{
		//    this.callback -= callback;
		//}
	}
}
