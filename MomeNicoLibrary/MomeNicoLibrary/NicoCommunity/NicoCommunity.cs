using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace MomeNicoLibrary.NicoCommunity
{
	/// <summary>
	/// コミュニティを表すクラス 暗号化パスワード対応
	/// </summary>
	public class NicoCommunity
	{
		/// <summary>
		/// コミュニティ情報
		/// </summary>
		public struct CommunityInfo
		{
			public string title;
			public string description;
			public string thumbnail_url;
		}

		public CommunityInfo GetCommunityInfo(int broadcast_id)
		{
			CommunityInfo ci;
			string title = "";
			string description = "";
			string thumbnail_url = "";

			string loginURL = "http://live.nicovideo.jp/api/getstreaminfo/lv" + broadcast_id;

			// Request開始
			WebRequest request = HttpWebRequest.Create(loginURL);
			request.Method = "GET";

			WebResponse response = request.GetResponse();
			Stream resStream = response.GetResponseStream();

			//			StreamReader sr = new StreamReader(resStream, Encoding.UTF8);
			//			string html = sr.ReadToEnd();
			//			sr.Close();
			//			resStream.Close();
			//			System.Console.WriteLine(html);

			XmlDocument xml2 = new XmlDocument();
			xml2.Load(resStream);

			foreach (XmlNode node2 in xml2.GetElementsByTagName("title"))
			{
				string innerText2 = node2.InnerText;
				title = innerText2;
				//				Console.WriteLine(innerText2);
			}

			foreach (XmlNode node2 in xml2.GetElementsByTagName("description"))
			{
				string innerText2 = node2.InnerText;
				description = innerText2;
				//				Console.WriteLine(innerText2);
			}

			foreach (XmlNode node2 in xml2.GetElementsByTagName("thumbnail"))
			{
				string innerText2 = node2.InnerText;
				thumbnail_url = innerText2;
				//				Console.WriteLine(innerText2);
			}

			ci.title = title;
			ci.description = description;
			ci.thumbnail_url = thumbnail_url;

			return ci;
		}
	}
}
