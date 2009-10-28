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
using MomeNicoLibrary.Utility;

namespace MomeNicoLibrary.NicoLive.Information
{
	class CommentItems : XPathItems
	{
		//<chat anonymity="1" date="1255295013" mail="184" no="128" premium="1" thread="10
		//06553117" user_id="yN1oR_OugN7wEhEjm8Yaik5ZXdY" vpos="70309">寝姿公開なら文句な
		//し</chat>

		public CommentItems()
		{
			requests.Add(new XmlRequest(COMMENT));
			requests.Add(new XmlRequest(ANONYMOUS, false));
			requests.Add(new XmlRequest(DATE));
			requests.Add(new XmlRequest(MAIL, false));
			requests.Add(new XmlRequest(NUMBER));
			requests.Add(new XmlRequest(THREAD));
			requests.Add(new XmlRequest(PREMIUM, false));
			requests.Add(new XmlRequest(USERID, false));
			requests.Add(new XmlRequest(VPOS));
		}

		public const string COMMENT = "chat";
		public const string ANONYMOUS = "chat/@anonymity";
		public const string DATE = "chat/@date";
		public const string MAIL = "chat/@mail";
		public const string NUMBER = "chat/@no";
		public const string THREAD = "chat/@thread";
		public const string PREMIUM = "chat/@premium";
		public const string USERID = "chat/@user_id";
		public const string VPOS = "chat/@vpos";
	}

	/// <summary>
	/// 放送のコメントを表すクラス
	/// </summary>
	public class BroadcastComment : NicoInformation
	{
		protected BroadcastComment(Dictionary<string, string> dict)
		{
			this.dict = dict;
		}

		/// <summary>
		/// コメントパーサー
		/// </summary>
		/// <param name="chat"></param>
		static public BroadcastComment Parse(string chat)
		{
			BroadcastComment comment = new BroadcastComment(
				XmlParse(chat, new CommentItems()));
			return comment;
		}

		public string Comment
		{
			get
			{
				return dict[CommentItems.COMMENT];
			}
		}

		public int Anonymous
		{
			get
			{
				int output;
				int.TryParse(dict[CommentItems.ANONYMOUS], out output);
				return output;
			}
		}

		public int ElapsedTime
		{
			get
			{
				int output;
				int.TryParse(dict[CommentItems.DATE], out output);
				return output;
			}
		}

		//public TimeSpan ElapsedTime
		//{
		//    get
		//    {
		//        int output;
		//        int.TryParse(dict[ChatAttributes.DATE.ToString()], out output);
		//        return new TimeSpan(0, 0, output);
		//    }
		//}

		public int MAIL
		{
			get
			{
				int output;
				int.TryParse(dict[CommentItems.MAIL], out output);
				return output;
			}
		}

		public int Number
		{
			get
			{
				int output;
				int.TryParse(dict[CommentItems.NUMBER], out output);
				return output;
			}
		}

		public int Thread
		{
			get
			{
				int output;
				int.TryParse(dict[CommentItems.THREAD], out output);
				return output;
			}
		}

		public bool Premium
		{
			get
			{
				return (dict[CommentItems.PREMIUM] == "1");
			}
		}

		public string UserId
		{
			get
			{
				return dict[CommentItems.USERID];
			}
		}

		public int VPos
		{
			get
			{
				int output;
				int.TryParse(dict[CommentItems.VPOS], out output);
				return output;
			}
		}
	}
}
