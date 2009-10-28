using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using MomeNicoLibrary.NicoLive;
using MomeNicoLibrary.NicoLive.Information;
using MomeNicoLibrary.Utility;
using System.Xml;

namespace NicoLiveNgListViewer
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
			CookieCollection cookies = NicoLiveAPI.Login(mail, RSALib.Decrypt(password, RSAKey.SecretKey));

			NgWord table = NicoLiveAPI.ConfigureNgWord(cookies, "lv5478875");
			//foreach (NicoLiveAPI.NgWord ngWord in table.ngList)
			//{
			//    StringBuilder sb = new StringBuilder();
			//    sb.Append("NG WORD: ").Append(ngWord.source);
			//    Console.WriteLine(sb.ToString());
			//}
			string a = "";
		}

		void sample()
		{
			string b = "<chat anonymity=\"1\" date=\"1255295013\" mail=\"184\" no=\"128\" premium=\"1\" thread=\"1006553117\" user_id=\"yN1oR_OugN7wEhEjm8Yaik5ZXdY\" vpos=\"70309\">寝姿公開なら文句なし</chat>";
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(b);
			XmlNodeList list = xml.SelectNodes("chat/@nothing");
			foreach (XmlNode node in list)
			{
				Console.WriteLine(node.Name);
			}
			return;
		}
	}
}
