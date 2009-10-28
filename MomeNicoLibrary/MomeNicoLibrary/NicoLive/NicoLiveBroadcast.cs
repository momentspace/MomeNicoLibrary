using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Web;
using MomeNicoLibrary.NicoLive.Information;
using MomeNicoLibrary.NicoMovie;
using MomeNicoLibrary.Common;

namespace MomeNicoLibrary.NicoLive
{
	/// <summary>
	/// ニコ生の番組クラス
	/// </summary>
	public class NicoLiveBroadcast
	{

		/// <summary>
		/// 番組に結びついているコメント
		/// </summary>
		private BroadcastCommentCollection comments = new BroadcastCommentCollection();

		private CommentReceiver receive;
		private CommentSender sender;

		private string mail;
		private string password;
		private string key = "";

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public NicoLiveBroadcast(string broadcastId, string mail, string password, string key)
		{
			this.broadcastId = broadcastId;
			this.mail = mail;
			this.password = password;
			this.key = key;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public NicoLiveBroadcast(string broadcastId, string mail, string password)
		{
			this.broadcastId = broadcastId;
			this.mail = mail;
			this.password = password;
			this.key = "";
		}

		/// <summary>
		/// 放送ID
		/// </summary>
		private string broadcastId;

		/// <summary>
		/// コメント取得開始
		/// </summary>
		/// <param name="csi"></param>
		public void Start()
		{
			NicoMovie.NicoMovie movie = new MomeNicoLibrary.NicoMovie.NicoMovie();
			CookieCollection cookies;
			if (key != string.Empty)
			{
				// 暗号化パスワード
				cookies = movie.Login(mail, password, key);
			}
			else
			{
				// 平文パスワード
				cookies = movie.Login(mail, password);
			}

			PlayerStatus ps = NicoLiveAPI.GetPlayerStatus(cookies, broadcastId);
			// 取得成功
			if (ps.Status)
			{
				receive = new CommentReceiver(ps.ServerInfo);
				receive.Open();
				receive.Initialize(0);
				receive.ReceivedEvent += new CommentReceiver.ReceivedEventHandler(CommentCallback);
				//			receive.AddObserver(this.CommentCallback);
				receive.Receive();
				receive.Close();
			}
		}

		public void Close()
		{
			if (receive != null)
			{
				receive.Close();
			}
		}

		public void SendComment(string comment)
		{
			// TODO not implements
			//PostComment(broadcastId, comment);
		}

		public string RecvComment()
		{
			// TODO not implements
			return "";
		}

		public void BroadcastInfo()
		{
			// TODO not implements

		}

		///// <summary>
		///// 番組情報を取得するためのSocket
		///// </summary>
		//private NicoLiveXMLSocket socket;

		/// <summary>
		/// コメント用コールバック
		/// </summary>
		/// <param name="line"></param>
		public void CommentCallback(string line)
		{
			BroadcastComment comment = BroadcastComment.Parse(line);

			CommentReceivedEvent(comment);
		}

		#region " 通知する側"
		/// <summary>
		/// コメント受信イベントハンドラー
		/// </summary>
		public delegate void CommentReceivedEventHandler(BroadcastComment comment);

		/// <summary>
		/// コメント受信イベント
		/// </summary>
		public event CommentReceivedEventHandler CommentReceivedEvent = OnReceiveEventHandler;

		/// <summary>
		/// コメント受信イベントホルダー
		/// </summary>
		public static void OnReceiveEventHandler(BroadcastComment comment) { }
		#endregion

		///// <summary>
		///// コメント投稿
		///// </summary>
		///// <param name="broadcast_id"></param>
		///// <param name="comment"></param>
		//public void PostComment(string broadcast_id, string comment)
		//{
		//    // ログインAPI その１（チケット取得）
		//    const string mail = "nico@bloodpledge.info";
		//    const string password = "dpXLD6yH9knRHf7yIi6cyGR2WxDW7wmayyoKEtUeaEoxqA8GDIjg8j+5lAOz/6DZg4fgAdnEo3yUTH4C" +
		//        "HyrTDu0pP8yj5evqZpWPhFJcZnXELsE/qE2xwNNqcC+BtPnBOHxRjFb9bru/BQrvyEiWQ6ADJOdw58BI" +
		//        "yt4hdGZp4oY=";
		//    CookieCollection cookies = NicoLiveAPI.Login(mail, password);

		//    PlayerStatus ps = NicoLiveAPI.GetPlayerStatus(cookies, broadcast_id);

		//    {
		//        ServerInformation si = ps.ServerInfo;

		//        // コメントサーバに接続
		//        System.Net.Sockets.TcpClient tcp = new System.Net.Sockets.TcpClient(si.Address, si.Port);

		//        // コメントサーバに下記のコマンドを投げると情報がかえる
		//        Stream s = tcp.GetStream();
		//        StreamWriter sw = new StreamWriter(s);

		//        string cmd = "<thread thread=\"" + si.Thread + "\" res_from=\"-" + 0 + "\" version=\"20061206\" />\0";
		//        sw.Write(cmd);

		//        // last_resに書かれてるレス番を取得する
		//        System.Net.Sockets.TcpClient tcp2 = new System.Net.Sockets.TcpClient(si.Address, si.Port);

		//        Stream ts = tcp2.GetStream();
		//        string t = "<thread thread=\"" + si.Thread + "\" version=\"20061206\" res_from=\"-1\"/>\0";
		//        byte[] b = Encoding.ASCII.GetBytes(t);
		//        ts.Write(b, 0, b.Length);

		//        byte[] buf = new byte[512];
		//        int size = ts.Read(buf, 0, buf.Length);
		//        string line = Encoding.UTF8.GetString(buf).Trim('\0');
		//        ts.Close();

		//        XmlDocument xml = new XmlDocument();
		//        xml.LoadXml(line);

		//        int last_res = 0;
		//        string ticket = "";

		//        foreach (XmlNode node in xml.GetElementsByTagName("thread"))
		//        {
		//            foreach (XmlNode attr in node.Attributes)
		//            {
		//                switch (attr.Name)
		//                {
		//                    case "last_res":
		//                        last_res = int.Parse(attr.Value);
		//                        break;
		//                    case "ticket":
		//                        ticket = attr.Value;
		//                        break;
		//                    default:
		//                        //System.Console.WriteLine(attr.Name);
		//                        //System.Console.WriteLine(attr.Value);
		//                        break;
		//                }
		//            }
		//        }

		//        // 下記のURLに接続する
		//        string posturl = "http://live.nicovideo.jp/api/getpostkey?thread=" + si.Thread + "&block_no=" + last_res / 100;
		//        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(posturl);
		//        request.Method = "GET";
		//        request.CookieContainer = new CookieContainer();
		//        foreach (Cookie c in cookies)
		//        {
		//            request.CookieContainer.Add(c);
		//        }

		//        WebResponse response = request.GetResponse();
		//        Stream st = response.GetResponseStream();
		//        byte[] buf2 = new byte[512];
		//        st.Read(buf2, 0, buf2.Length);

		//        string postkey = Encoding.UTF8.GetString(buf2).Trim('\0').Split('=')[1];
		//        //				Console.WriteLine(postkey);

		//        // コメントサーバに下記のデータを送信
		//        long cur = UnixEpochTime.Now();

		//        int vpos = ((int)(cur / 1000) - (ps.bti.baseTime)) * 100;
		//        string postXMLTmpl = "<chat thread=\"" + si.Thread + "\" ticket=\"" + ticket + "\" vpos=\"" + vpos + "\" postkey=\"" + postkey + "\" mail=\"\" user_id=\"" + ps.csi.usrId + "\" premium=\"0\">" + comment + "</chat>\0";
		//        sw.Write(postXMLTmpl);
		//        sw.Flush();


		//        //            final String postXML = String.format(postXMLTmpl, msThread,
		//        //                                               ticket, vpos,
		//        //                                             postkey, cmd.toString(),
		//        //                                           userId, premium,
		//        //                                         comment);
		//        return;
		//    }
		//}	
	}
}
