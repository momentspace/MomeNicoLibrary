using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MomeNicoLibrary.Utility;

namespace MomeNicoLibrary.NicoLive.Information
{
	class NgWordListItems : XPathItems
	{
		public NgWordListItems()
		{
			status = "response_ngword/@status";
			requests.Add(new XmlRequest(COUNT));
		}

		const string COUNT = "response_ngword/count";
	}

	class NgWordList : NicoInformation
	{
		private NgWordList(Dictionary<string, string> dict, List<NgWord> ngwords)
		{
			this.dict = dict;
			this.ngwords = ngwords;
		}

		public static NgWordList Parse(string message)
		{
			List<NgWord> ngwords = new List<NgWord>();
			const string ngwordPath = "response_ngword/ngclient";
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(message);
			XmlNodeList nodes = xml.SelectNodes(ngwordPath);

			foreach (XmlNode node in nodes)
			{
				ngwords.Add(NgWord.Parse(node.InnerXml));
			}

			NgWordList info = new NgWordList(
				XmlParse(message, new NgWordListItems()), ngwords);

			return info;
		}

		private List<NgWord> ngwords = new List<NgWord>();
	}
}
