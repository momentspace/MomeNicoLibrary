using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MomeNicoLibrary.NicoLive;
using MomeNicoLibrary.NicoLive.Information;
using MomeNicoLibrary.Utility;

namespace NicoLiveAPITest
{
	class Program
	{
		static void Main(string[] args)
		{
			CookieCollection cookies;
			using (FirefoxCookie cookieGetter = new FirefoxCookie("X:\\Firefox\\Profiles\\y0bgcsvb.default\\cookies.sqlite"))
			{
				cookies = cookieGetter.GetCookie();
			}
			string broadcastId = "lv6392901";
			HeartBeatInformation info = NicoLiveAPI.HeartBeat(broadcastId, cookies);
			Console.WriteLine(info);
		}
	}
}
