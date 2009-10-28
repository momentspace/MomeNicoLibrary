using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MomeNicoLibrary.Utility;

namespace MomeNicoLibrary.NicoLive.Information
{
	class AlertLoginInfoItems : XPathItems
	{
		//<?xml version="1.0" encoding="utf-8"?>
		//<nicovideo_user_response status="ok">
		//		<ticket>nicolive_antenna_1438726310416785531151210162</ticket>
		//</nicovideo_user_response>

		public AlertLoginInfoItems()
		{
			status = "nicovideo_user_response/@status";
			requests.Add(new XmlRequest(RESPONSE));
			requests.Add(new XmlRequest(TICKET));
		}

		public const string RESPONSE = "nicovideo_user_response";
		public const string TICKET = "nicovideo_user_response/ticket";
	}

	class AlertLoginInfo : NicoInformation
	{
		private AlertLoginInfo(Dictionary<string, string> dict)
		{
			this.dict = dict;
		}

		public static AlertLoginInfo Parse(string message)
		{
			AlertLoginInfo info = new AlertLoginInfo(
				XmlParse(message, new AlertLoginInfoItems()));
			return info;
		}

		public string Ticket
		{
			get
			{
				return dict[AlertLoginInfoItems.TICKET];
			}
		}
	}
}
