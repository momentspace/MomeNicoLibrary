using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Web;
using MomeNicoLibrary.Common;
using MomeNicoLibrary.NicoCommunity;

namespace MomeNicoLibrary.NicoLive
{
	public class AlertData
	{
		/// <summary>
		/// アラート用コールバック
		/// </summary>
		/// <param name="line"></param>
		static public void AlertCallback(string line)
		{
			int date = 0;
			string no = "";
			string th = "";
			string user_id = "";
			int broadcast_id = 0;
			string channel_id = "";
			string caster_id = "";

			XmlDocument xml = new XmlDocument();
			xml.LoadXml(line);
			XmlNodeList lst = xml.GetElementsByTagName("chat");
			foreach (XmlNode node in lst)
			{
				date = int.Parse(node.Attributes["date"].Value);
				no = node.Attributes["no"].Value;
				th = node.Attributes["thread"].Value;
				user_id = node.Attributes["user_id"].Value;
				string[] innerText = node.InnerText.Split(',');

				broadcast_id = int.Parse(innerText[0]);
				channel_id = innerText[1];
				caster_id = innerText[2];
			}

			NicoCommunity.NicoCommunity nc = new NicoCommunity.NicoCommunity();
			NicoCommunity.NicoCommunity.CommunityInfo ci = nc.GetCommunityInfo(broadcast_id);
			Console.WriteLine(new StringBuilder()
				.Append(no).Append(" ")
				.Append(UnixEpochTime.UNIXTimeToDateTime(date)).Append(" ")
				.Append(ci.title).Append(" ")
				//							.Append(th).Append(" ")
				//							.Append(user_id).Append(" ")
			);
		}
		

	}
}
