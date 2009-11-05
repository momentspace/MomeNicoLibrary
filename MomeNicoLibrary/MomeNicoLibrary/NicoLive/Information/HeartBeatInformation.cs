using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MomeNicoLibrary.NicoLive.Information
{
	class HeartBeatItems : XPathItems
	{
		// <heartbeat status="ok">
		//   <watchCount>21</watchCount>
		//   <commentCount>16</commentCount>
		//   <ticket>697078:lv6345877:0:1257577997:27b0c7dbae376b87</ticket>
		// </heartbeat>

		public HeartBeatItems()
		{
			status = "heartbeat/@status";
			requests.Add(new XmlRequest(WATCHCOUNT));
			requests.Add(new XmlRequest(COMMENTCOUNT));
			requests.Add(new XmlRequest(TICKET));
		}

		public const string WATCHCOUNT = "heartbeat/watchCount";
		public const string COMMENTCOUNT = "heartbeat/commentCount";
		public const string TICKET = "heartbeat/ticket";
	}

	public class HeartBeatInformation : NicoInformation
	{
		private HeartBeatInformation(Dictionary<string, string> dict)
		{
			this.dict = dict;
		}

		/// <summary>
		/// コメントパーサー
		/// </summary>
		/// <param name="message"></param>
		static public HeartBeatInformation Parse(string message)
		{
			HeartBeatInformation info = new HeartBeatInformation(
				XmlParse(message, new HeartBeatItems()));
			return info;
		}

	}
}
