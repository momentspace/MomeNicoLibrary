using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Web;

using MomeNicoLibrary.Utility;
using MomeNicoLibrary.NicoLive;

namespace MomeNicoLibrary.NicoMovie
{
	public class NicoMovie
	{
		/// <summary>
		/// ニコニコ ログインAPI 暗号化パスワード対応
		/// </summary>
		/// <param name="mail"></param>
		/// <param name="cryptedPassword"></param>
		/// <param name="secretKey"></param>
		/// <returns></returns>
		public CookieCollection Login(string mail, string cryptedPassword, string RSAsecretKey)
		{
			return Login(mail, RSALib.Decrypt(cryptedPassword, RSAsecretKey));
		}

		/// <summary>
		/// ニコニコ ログインAPI
		/// </summary>
		/// <param name="mail"></param>
		/// <param name="cryptedPassword"></param>
		/// <returns>session</returns>
		public CookieCollection Login(string mail, string password)
		{
			return NicoLiveAPI.Login(mail, password);
		}
	}
}
