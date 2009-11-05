using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Web;
using MomeNicoLibrary.Utility;
using MomeNicoLibrary.NicoLive.Information;

namespace MomeNicoLibrary.NicoLive
{
	public class NicoLiveAlert
	{
		public NicoLiveAlert()
		{
		}

		public NicoLiveAlert(string mail, string password)
		{
			string ticket = Login(mail, password);
			Alert(ticket);
		}

		#region " 通知する側"
		/// <summary>
		/// アラートイベントハンドラー
		/// </summary>
		public delegate void AlertEventHandler(AlertInformation info);

		/// <summary>
		/// アラートイベント
		/// </summary>
		public event AlertEventHandler AlertEvent = OnAlertEventHandler;

		/// <summary>
		/// プレスホルダー
		/// </summary>
		public static void OnAlertEventHandler(AlertInformation info) {	}
		#endregion

		/// <summary>
		/// ニコ生アラート ログインAPI 暗号化パスワード
		/// </summary>
		/// <param name="mail"></param>
		/// <param name="cryptedPassword"></param>
		/// <param name="secretKey"></param>
		/// <returns></returns>
		public string Login(string mail, string cryptedPassword, string RSAsecretKey)
		{
			return Login(mail, RSALib.Decrypt(cryptedPassword, RSAsecretKey));
		}

		/// <summary>
		/// ニコ生アラート ログインAPI
		/// </summary>
		/// <param name="mail">ログイン用メール</param>
		/// <param name="password">RSA暗号化されたパスワード</param>
		/// <returns>チケット</returns>
		public string Login(string mail, string password)
		{
			return NicoLiveAPI.AlertLogin(mail, password);
		}

		public string AnonyLogin()
		{
			// not implements
			return "";
		}

		/// <summary>
		/// アラート受信クラス
		/// </summary>
		private AlertReveicer receiver;

		/// <summary>
		/// 
		/// </summary>
		public void Alert(string ticket)
		{
			// 認証API（認証用ハッシュ取得とコメントサーバ取得）
			AlertServerInfo ai = NicoLiveAPI.GetAlertServer(ticket);
			if (ai.Status)
			{
				// サーバ情報
				ServerInformation si = ai.ServerInfo;
				// XMLSocket部分
				receiver = new AlertReveicer(si);
				receiver.Initialize(-1);
				receiver.ReceivedEvent += new AlertReveicer.ReceivedEventHandler(AlertCallback);
				receiver.Receive();
				receiver.Close();
			}
		}

		#region " コメントレシーバから通知される側"
		/// <summary>
		/// アラート用コールバック
		/// </summary>
		/// <param name="line"></param>
		public void AlertCallback(string line)
		{
			AlertInformation info = AlertInformation.Parse(line);
			AlertEvent(info);
		}

		#endregion
	}
}
