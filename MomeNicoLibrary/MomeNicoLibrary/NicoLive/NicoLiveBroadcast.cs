using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Timers;
using MomeNicoLibrary.NicoLive.Information;
using MomeNicoLibrary.NicoMovie;
using MomeNicoLibrary.Utility;

namespace MomeNicoLibrary.NicoLive
{
	/// <summary>
	/// ニコ生の番組クラス
	/// </summary>
	public class NicoLiveBroadcast : IDisposable
	{
		/// <summary>
		/// コメント受信者
		/// </summary>
		private CommentReceiver receiver;

		/// <summary>
		/// コメント送信者
		/// </summary>
		private CommentSender sender;

		/// <summary>
		/// ログインクッキー
		/// </summary>
		private CookieCollection cookies;

		/// <summary>
		/// 放送ID
		/// </summary>
		private string broadcastId;

		/// <summary>
		/// 放送情報
		/// </summary>
		private PlayerStatus ps;

		/// <summary>
		/// 暗号化パスワード用 コンストラクタ
		/// パスワードを復号化してパスワード平文用コンストラクタを呼び出す
		/// </summary>
		/// <param name="broadcastId">放送ID</param>
		/// <param name="mail">メールアドレス</param>
		/// <param name="cryptedPassword">暗号化パスワード</param>
		/// <param name="key">復号化用RSAプライベートキー</param>
		public NicoLiveBroadcast(string broadcastId, string mail, string cryptedPassword, string key)
			: this(broadcastId, mail, RSALib.Decrypt(cryptedPassword, key))
		{
			// function is transferred 
		}

		/// <summary>
		/// パスワード平文用 コンストラクタ
		/// </summary>
		/// <param name="broadcastId">放送ID</param>
		/// <param name="mail">メールアドレス</param>
		/// <param name="password">パスワード</param>
		public NicoLiveBroadcast(string broadcastId, string mail, string password)
		{
			this.broadcastId = broadcastId;

			NicoMovie.NicoMovie movie = new MomeNicoLibrary.NicoMovie.NicoMovie();
			this.cookies = movie.Login(mail, password);

			this.Initialize();
		}

		/// <summary>
		/// Cookie渡しのコンストラクタ
		/// </summary>
		/// <param name="broadcastId"></param>
		/// <param name="cookies"></param>
		public NicoLiveBroadcast(string broadcastId, CookieCollection cookies)
		{
			this.broadcastId = broadcastId;
			this.cookies = cookies;

			this.Initialize();
		}

		private void Initialize()
		{
			// HeartBeatAPI定期送信開始
			this.HeartBeatStart();

			// 番組情報取得
			ps = NicoLiveAPI.GetPlayerStatus(cookies, broadcastId);
		}

		private const double HEARTBEAT_INTERVAL = 45.0 * 100;
		private Timer heartBeatTimer = new Timer();

		private void HeartBeatStart()
		{
			heartBeatTimer.Interval = HEARTBEAT_INTERVAL;
			heartBeatTimer.Elapsed +=
				new ElapsedEventHandler(delegate(object sender, ElapsedEventArgs e)
				{
					HeartBeatInformation info = NicoLiveAPI.HeartBeat(broadcastId, cookies);
					Console.WriteLine(info);
				}
			);
			heartBeatTimer.Start();
		}

		/// <summary>
		/// 放送情報を更新する
		/// </summary>
		public void RefleshBroadcastInfo()
		{
			ps = NicoLiveAPI.GetPlayerStatus(cookies, broadcastId);
		}

		/// <summary>
		/// コメント送受信開始
		/// 開始時に（過去のコメントを含めて）すべてのコメントを取得する
		/// </summary>
		public void Start()
		{
			this.Start(0);
		}

		/// <summary>
		/// コメント送受信開始
		/// </summary>
		/// <param name="preCommentIndex">コメントインデックス</param>
		public void Start(int preCommentIndex)
		{
			// 番組情報が取得成功していること
			if (ps.Status)
			{
				// 送信
				sender = new CommentSender(ps.ServerInfo, cookies);

				// 受信
				receiver = new CommentReceiver(ps.ServerInfo);
				receiver.Initialize(preCommentIndex);
				receiver.ReceivedEvent += new CommentReceiver.ReceivedEventHandler(CommentCallback);
				receiver.Receive();
			}
		}

		public void Close()
		{
			if (receiver != null)
			{
				receiver.Close();
			}
		}

		public void SendComment(BroadcastComment comment)
		{
			sender.Send(comment);
		}

		public BroadcastComment RecvComment()
		{
			return receiver.Recv();
		}

		private BroadcastInformation broadcastInfo;
		public BroadcastInformation BroadcastInfo
		{
			get
			{
				return broadcastInfo;
			}
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
			// 取得したコメント（1つ分）を放送コメントにParseする
			BroadcastComment comment = BroadcastComment.Parse(line);

			// コメントをイベント通知する
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

		#region IDisposable メンバ

		public void Dispose()
		{
			heartBeatTimer.Close();
			sender.Close();
			receiver.Close();
		}

		#endregion
	}
}
