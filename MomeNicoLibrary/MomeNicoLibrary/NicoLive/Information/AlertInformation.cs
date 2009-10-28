using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MomeNicoLibrary.Utility;

namespace MomeNicoLibrary.NicoLive.Information
{
	public class AlertItems : XPathItems
	{
		//<chat date=\"1256356669\" no=\"1078217\" premium=\"2\" thread=\"1000000003\" user_id=\"394\" vpos=\"0\">5709555,co124096,1072887</chat>
		public AlertItems()
		{
			requests.Add(new XmlRequest(COMMENT));
			requests.Add(new XmlRequest(DATE));
			requests.Add(new XmlRequest(NUMBER));
			requests.Add(new XmlRequest(THREAD));
			requests.Add(new XmlRequest(USERID));
			requests.Add(new XmlRequest(VPOS));
		}

		public const string COMMENT = "chat";
		public const string DATE = "chat/@date";
		public const string NUMBER = "chat/@no";
		public const string THREAD = "chat/@thread";
		public const string USERID = "chat/@user_id";
		public const string VPOS = "chat/@vpos";
	}

	/// <summary>
	/// アラート情報を表すクラス
	/// </summary>
	public class AlertInformation : NicoInformation
	{
		protected AlertInformation(Dictionary<string, string> dict)
		{
			this.dict = dict;
		}

		/// <summary>
		/// アラートパーサー
		/// </summary>
		/// <param name="chat"></param>
		static public AlertInformation Parse(string chat)
		{
			AlertInformation info = new AlertInformation(
				XmlParse(chat, new AlertItems()));
			return info;
		}

		public string Comment
		{
			get
			{
				return dict[AlertItems.COMMENT];
			}
		}

		public int ElapsedTime
		{
			get
			{
				int output;
				int.TryParse(dict[AlertItems.DATE], out output);
				return output;
			}
		}

		public int Number
		{
			get
			{
				int output;
				int.TryParse(dict[AlertItems.NUMBER], out output);
				return output;
			}
		}

		public int BroadcastId
		{
			get
			{
				int output;
				int.TryParse(dict[AlertItems.COMMENT].Split(',')[0], out output);
				return output;
			}
		}

		public int ChannelId
		{
			get
			{
				int output;
				int.TryParse(dict[AlertItems.COMMENT].Split(',')[1], out output);
				return output;
			}
		}

		public int CasterId
		{
			get
			{
				int output;
				int.TryParse(dict[AlertItems.COMMENT].Split(',')[2], out output);
				return output;
			}
		}

		//public int Thread
		//{
		//    get
		//    {
		//        int output;
		//        int.TryParse(dict[AlertInformation.THREAD], out output);
		//        return output;
		//    }
		//}

		//public string UserId
		//{
		//    get
		//    {
		//        return dict[AlertInformation.USERID];
		//    }
		//}

		//public int VPos
		//{
		//    get
		//    {
		//        int output;
		//        int.TryParse(dict[AlertInformation.VPOS], out output);
		//        return output;
		//    }
		//}
	}
}
