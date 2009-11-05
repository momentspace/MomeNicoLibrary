using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MomeNicoLibrary.NicoLive;
using MomeNicoLibrary.Utility;
using MomeNicoLibrary.NicoLive.Information;
using System.Net;

namespace MovieRequester
{
	class Program
	{
		private static NicoLiveBroadcast broadcast;
		static void Main(string[] args)
		{
			CookieCollection cookies;
			using (FirefoxCookie cookieGetter = new FirefoxCookie("X:\\Firefox\\Profiles\\y0bgcsvb.default\\cookies.sqlite"))
			{
				cookies = cookieGetter.GetCookie();
			}

			broadcast = new NicoLiveBroadcast("lv6359296", cookies);
			broadcast.CommentReceivedEvent += new NicoLiveBroadcast.CommentReceivedEventHandler(callback);
			broadcast.Start(1);
		}

		public static void callback(BroadcastComment comment)
		{
			Console.WriteLine(new StringBuilder()
				.Append(comment.Number).Append(" ")
				.Append(comment.ElapsedTime.ToString()).Append(" ")
				.Append(comment.Premium ? "P" : "N").Append(" ")
				.Append(comment.UserId).Append(" ")
				.Append(comment.CommentValue).Append(" ")
			);
			
			if (comment.CommentValue.StartsWith("sm"))
			{
				//broadcast.SendComment("/play " + comment.CommentValue + " sub");
			}
			return;
		}
	}

}
