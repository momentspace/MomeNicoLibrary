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
using MomeNicoLibrary.Utility;

namespace MomeNicoLibrary.NicoLive
{
	public class NicoLiveAPI
	{

		private NicoLiveAPI()
		{
			// Can not create instance
		}

		/// <summary>
		/// GetPlayerStatus
		/// </summary>
		/// <param name="cookies"></param>
		/// <param name="broadcast_id"></param>
		/// <returns></returns>
		public static PlayerStatus GetPlayerStatus(CookieCollection cookies, string broadcast_id)
		{
			string loginURL = "http://watch.live.nicovideo.jp/api/getplayerstatus?v=" + broadcast_id;

			// Request開始
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(loginURL);
			request.CookieContainer = new CookieContainer();
			request.Method = "GET";
			foreach (Cookie c in cookies)
			{
				request.CookieContainer.Add(c);
			}

			WebResponse response = request.GetResponse();
			Stream resStream = response.GetResponseStream();
			StreamReader sr = new StreamReader(resStream, Encoding.UTF8);
			string html = sr.ReadToEnd();

			PlayerStatus ps = PlayerStatus.Parse(html);
//			ServerInformation si = ps.ServerInfo;
//			BroadcastTimeInformation bti = new BroadcastTimeInformation();

			// getplayerstatus
			//<ms>
			//<addr>msg101.live.nicovideo.jp</addr>
			//<port>2808</port>
			//<thread>1006533792</thread>
			//</ms>

			//foreach (XmlNode node2 in xml2.GetElementsByTagName("addr"))
			//{
			//    string innerText2 = node2.InnerText;
			//    csi.address = innerText2;
			//    //								Console.WriteLine(innerText2);
			//}

			//foreach (XmlNode node2 in xml2.GetElementsByTagName("port"))
			//{
			//    string innerText2 = node2.InnerText;
			//    csi.port = int.Parse(innerText2);
			//    //								Console.WriteLine(innerText2);
			//}

			//foreach (XmlNode node2 in xml2.GetElementsByTagName("thread"))
			//{
			//    string innerText2 = node2.InnerText;
			//    csi.thread = innerText2;
			//    //								Console.WriteLine(innerText2);
			//}

			//foreach (XmlNode node2 in xml2.GetElementsByTagName("thread"))
			//{
			//    string innerText2 = node2.InnerText;
			//    csi.thread = innerText2;
			//    //								Console.WriteLine(innerText2);
			//}

			//foreach (XmlNode node2 in xml2.GetElementsByTagName("base_time"))
			//{
			//    string innerText2 = node2.InnerText;
			//    bti.baseTime = int.Parse(innerText2);
			//    //								Console.WriteLine(innerText2);
			//}

			//foreach (XmlNode node2 in xml2.GetElementsByTagName("open_time"))
			//{
			//    string innerText2 = node2.InnerText;
			//    bti.openTime = int.Parse(innerText2);
			//    //								Console.WriteLine(innerText2);
			//}

			//foreach (XmlNode node2 in xml2.GetElementsByTagName("start_time"))
			//{
			//    string innerText2 = node2.InnerText;
			//    bti.startTime = int.Parse(innerText2);
			//    //								Console.WriteLine(innerText2);
			//}

			//foreach (XmlNode node2 in xml2.GetElementsByTagName("user_id"))
			//{
			//    string innerText2 = node2.InnerText;
			//    csi.usrId = innerText2;
			//}

			//foreach (XmlNode node in xml2.GetElementsByTagName("user_hash"))
			//{
			//    string innerText2 = node.InnerText;
			//    csi.usrIdHash = innerText2;
			//}

			//ps.csi = csi;
			//ps.bti = bti;
			return ps;
		}

		/// <summary>
		/// GetAlertServer
		/// </summary>
		/// <param name="ticket"></param>
		/// <returns></returns>
		public static AlertServerInfo GetAlertServer(string ticket)
		{
			string loginURL = "http://live.nicovideo.jp/api/getalertstatus";

			// POSTの内容
			string param = "ticket=" + ticket;

			// Request開始
			WebRequest request = HttpWebRequest.Create(loginURL);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			byte[] bufs = Encoding.ASCII.GetBytes(param);
			request.ContentLength = bufs.Length;

			// POST書き込み
			Stream s = request.GetRequestStream();
			s.Write(bufs, 0, bufs.Length);
			s.Flush();

			WebResponse response = request.GetResponse();
			Stream resStream = response.GetResponseStream();
			StreamReader sr = new StreamReader(resStream, Encoding.UTF8);
			string html = sr.ReadToEnd();

			// Alert
			AlertServerInfo info = AlertServerInfo.Parse(html);
			return info;
		}

		/// <summary>
		/// ログインAPI
		/// </summary>
		/// <param name="mail"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static CookieCollection Login(string mail, string password)
		{
			//// デバッグ用
			//ServicePointManager.ServerCertificateValidationCallback =
			//    new System.Net.Security.RemoteCertificateValidationCallback(OnRemoteCertificateValidationCallback);

			string loginURL = "https://secure.nicovideo.jp/secure/login?site=niconico";

			// POSTの内容
			string param = string.Format("mail={0}&password={1}",HttpUtility.UrlEncode(mail), HttpUtility.UrlEncode(password));

			// Request開始
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(loginURL);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			byte[] bufs = Encoding.ASCII.GetBytes(param);
			request.ContentLength = bufs.Length;
			request.CookieContainer = new CookieContainer();

			// POST書き込み
			Stream s = request.GetRequestStream();
			s.Write(bufs, 0, bufs.Length);
			s.Flush();

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream resStream = response.GetResponseStream();

			// Cookieを取り出す
			CookieCollection cookies = request.CookieContainer.GetCookies(new Uri("http://www.nicovideo.jp"));

			StreamReader sr = new StreamReader(resStream, Encoding.UTF8);
			string html = sr.ReadToEnd();

			sr.Close();
			resStream.Close();

			return cookies;
		}

		/// <summary>
		/// アラート用ログインAPI
		/// </summary>
		/// <param name="mail"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static string AlertLogin(string mail, string password)
		{
			string loginURL = "https://secure.nicovideo.jp/secure/login?site=nicolive_antenna";

			// POSTの内容
			string param = string.Format("mail={0}&password={1}", HttpUtility.UrlEncode(mail), HttpUtility.UrlEncode(password));

			// Request開始
			WebRequest request = HttpWebRequest.Create(loginURL);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			//			request.Proxy = new WebProxy("http://localhost:8888");
			byte[] bufs = Encoding.ASCII.GetBytes(param);
			//			Console.WriteLine(.Length);
			request.ContentLength = bufs.Length;

			// POST書き込み
			Stream s = request.GetRequestStream();
			s.Write(bufs, 0, bufs.Length);
			s.Flush();

			WebResponse response = request.GetResponse();
			Stream resStream = response.GetResponseStream();

			StreamReader sr = new StreamReader(resStream, Encoding.UTF8);
			string html = sr.ReadToEnd();
			sr.Close();
			resStream.Close();

			AlertLoginInfo ali = AlertLoginInfo.Parse(html);
			return ali.Ticket;
		}

		/// <summary>
		/// ConfigureNgWord
		/// </summary>
		/// <param name="broadcast_id"></param>
		static public NgWord ConfigureNgWord(CookieCollection cookies, string broadcast_id)
		{
			string url = "http://live.nicovideo.jp/api/configurengword?video=" + broadcast_id + "&mode=get&video=" + broadcast_id;

			// Request開始
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.Method = "GET";
			request.CookieContainer = new CookieContainer();
			foreach (Cookie c in cookies)
			{
				request.CookieContainer.Add(c);
			}

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream resStream = response.GetResponseStream();

			StreamReader sr = new StreamReader(resStream, Encoding.UTF8);
			string html = sr.ReadToEnd();

			sr.Close();
			resStream.Close();

			// デバッグ用
//			System.Console.WriteLine(html);

			NgWord list = NgWord.Parse(html);

			return list;
		}

		public static HeartBeatInformation HeartBeat(string broadcastId, CookieCollection cookies)
		{
			string url = "http://live.nicovideo.jp/api/heartbeat?v=" + broadcastId;

			// Request開始
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.Method = "GET";
			request.CookieContainer = new CookieContainer();
			foreach (Cookie c in cookies)
			{
				request.CookieContainer.Add(c);
			}

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream resStream = response.GetResponseStream();

			StreamReader sr = new StreamReader(resStream, Encoding.UTF8);
			string html = sr.ReadToEnd();

			HeartBeatInformation info = HeartBeatInformation.Parse(html);
	
			sr.Close();
			resStream.Close();

			return info;
		}
	}
}
