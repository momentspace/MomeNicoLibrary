using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MomeNicoLibrary.NicoLive.Information
{
	class AlertServerInfoItems : XPathItems
	{
		//<getalertstatus status="ok" time="[アクセスタイム]">
		//  <user_id>[ユーザーID:0000]</user_id>
		//  <user_hash>[認証用ハッシュ:XXXXXXXXXXXXXXXXXXXXXXXXXXX]</user_hash>
		//  <communities>
		//    <community_id>[加入コミュニティID:coXXXX]</community_id>
		//    <community_id>[加入チャンネルID:chXXXX]</community_id>
		//  </communities>
		//  <ms>
		//    <addr>[コメントサーバーのアドレス]</addr>
		//    <port>[コメントサーバーのポート]</port>
		//    <thread>[スレッドID]</thread>
		//  </ms>
		//</getalertstatus>

		public AlertServerInfoItems()
		{
			status = "/getalertstatus/@status";
			requests.Add(new XmlRequest(ADDRESS));
			requests.Add(new XmlRequest(PORT));
			requests.Add(new XmlRequest(THREAD));
		}

		public const string ADDRESS = "getalertstatus/ms/addr";
		public const string PORT = "getalertstatus/ms/port";
		public const string THREAD = "getalertstatus/ms/thread";
	}

	public class AlertServerInfo : NicoInformation
	{
		private AlertServerInfo(Dictionary<string, string> dict)
		{
			this.dict = dict;
		}

		public static AlertServerInfo Parse(string message)
		{
			AlertServerInfo info = new AlertServerInfo(
				XmlParse(message, new AlertServerInfoItems()));
			return info;
		}

		public ServerInformation ServerInfo
		{
			get
			{
				string address = dict[AlertServerInfoItems.ADDRESS];
				int port;
				int thread;
				int.TryParse(dict[AlertServerInfoItems.THREAD], out port);
				int.TryParse(dict[AlertServerInfoItems.PORT], out thread);
				ServerInformation info = 
					new ServerInformation(address, port, thread);
				return info;
			}
		}

	}
}
