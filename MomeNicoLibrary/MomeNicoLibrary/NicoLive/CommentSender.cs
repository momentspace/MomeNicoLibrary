using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Web;
using MomeNicoLibrary.Utility;
using MomeNicoLibrary.NicoLive.Information;

namespace MomeNicoLibrary.NicoLive
{
	public class CommentSender
	{
		/// <summary>
		/// 送信するコメント
		/// </summary>
		private BroadcastCommentCollection sendComments = new BroadcastCommentCollection();

		private Thread th;
		private ServerInformation si;
		private CookieCollection cookies;
		public CommentSender(ServerInformation si, CookieCollection cookies)
		{
			this.th = new Thread(this.Sending);
			this.si = si;
			this.cookies = cookies;
		}

		private void Sending()
		{
			while (true)
			{
				if (sendComments.Count != 0)
				{
					PostComment(sendComments.DequeueComment());
				}
			}
		}

		public void Send(BroadcastComment comment)
		{
			sendComments.EnqueueComment(comment);
		}

		/// <summary>
		/// コメント投稿
		/// </summary>
		/// <param name="broadcast_id"></param>
		/// <param name="comment"></param>
		public void PostComment(BroadcastComment comment)
		{
			// コメントサーバに接続
			System.Net.Sockets.TcpClient tcp = new System.Net.Sockets.TcpClient(si.Address, si.Port);

			// コメントサーバに下記のコマンドを投げると情報がかえる
			Stream s = tcp.GetStream();
			StreamWriter sw = new StreamWriter(s);
			string cmd = "<thread thread=\"" + si.Thread + "\" res_from=\"-" + 0 + "\" version=\"20061206\" />\0";
			sw.Write(cmd);

			// last_resに書かれてるレス番を取得する
			System.Net.Sockets.TcpClient tcp2 = new System.Net.Sockets.TcpClient(si.Address, si.Port);

			Stream ts = tcp2.GetStream();
			string t = "<thread thread=\"" + si.Thread + "\" version=\"20061206\" res_from=\"-1\"/>\0";
			byte[] b = Encoding.ASCII.GetBytes(t);
			ts.Write(b, 0, b.Length);

			byte[] buf = new byte[512];
			int size = ts.Read(buf, 0, buf.Length);
			string line = Encoding.UTF8.GetString(buf).Trim('\0');
			ts.Close();

			XmlDocument xml = new XmlDocument();
			xml.LoadXml(line);

			int last_res = 0;
			string ticket = "";

			foreach (XmlNode node in xml.GetElementsByTagName("thread"))
			{
				foreach (XmlNode attr in node.Attributes)
				{
					switch (attr.Name)
					{
						case "last_res":
							last_res = int.Parse(attr.Value);
							break;
						case "ticket":
							ticket = attr.Value;
							break;
						default:
							//System.Console.WriteLine(attr.Name);
							//System.Console.WriteLine(attr.Value);
							break;
					}
				}
			}
			
			// 下記のURLに接続する
			string posturl = "http://live.nicovideo.jp/api/getpostkey?thread=" + si.Thread + "&block_no=" + last_res / 100;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(posturl);
			request.Method = "GET";
			request.CookieContainer = new CookieContainer();
			foreach (Cookie c in cookies)
			{
				request.CookieContainer.Add(c);
			}
			
			WebResponse response = request.GetResponse();
			Stream st = response.GetResponseStream();
			byte[] buf2 = new byte[512];
			st.Read(buf2, 0, buf2.Length);

			string postkey = Encoding.UTF8.GetString(buf2).Trim('\0').Split('=')[1];
			//				Console.WriteLine(postkey);

			// コメントサーバに下記のデータを送信
			long cur = UnixEpochTime.Now();

			//				int vpos = ((int)(cur / 1000) - (ps.bti.baseTime)) * 100;
			//				string postXMLTmpl = "<chat thread=\"" + si.Thread + "\" ticket=\"" + ticket + "\" vpos=\"" + vpos + "\" postkey=\"" + postkey + "\" mail=\"\" user_id=\"" + ps.csi.usrId + "\" premium=\"0\">" + comment + "</chat>\0";
			string postXMLTmpl = "<chat thread=\"" + si.Thread + "\" ticket=\"" + ticket + "\" vpos=\"0\" postkey=\"" + postkey + "\" mail=\"\" user_id=\"" + "" + "\" premium=\"0\">" + comment + "</chat>\0";
			sw.Write(postXMLTmpl);
			sw.Flush();
			sw.Close();
		}

		public void PostOwnerComment(string comment, string broadcastId)
		{
			// 下記のURLに接続する
			string ownerurl = "http://live.nicovideo.jp/api/broadcast/" + broadcastId;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ownerurl);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.CookieContainer = new CookieContainer();
			foreach (Cookie c in cookies)
			{
				request.CookieContainer.Add(c);
			}

			// POSTの内容
			string param = "mail=&body=" + HttpUtility.UrlEncode(comment);
			byte[] bufs = Encoding.ASCII.GetBytes(param);
			request.ContentLength = bufs.Length;

			// POST書き込み
			using (Stream s = request.GetRequestStream())
			{
				s.Write(bufs, 0, bufs.Length);
				s.Flush();
			}

			// 受信後捨てる
			WebResponse response = request.GetResponse();
			using (StreamReader sr = new StreamReader(response.GetResponseStream()))
			{
				char[] dust = new char[512];
				sr.Read(dust, 0, dust.Length);
				Console.WriteLine(dust);
			}
		}

		public void Close()
		{

		}
	}
}
