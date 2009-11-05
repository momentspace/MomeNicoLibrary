using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using MomeNicoLibrary.Utility;
using MomeNicoLibrary.NicoLive;

namespace NicoLiveCommentSenderSample
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
			NicoLiveBroadcast broadcast = new NicoLiveBroadcast("lv6225686", mail, password, RSAKey.SecretKey);
			//			broadcast.AddObserver(Program.callback);
		}

	}
}
