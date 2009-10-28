using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MomeNicoLibrary.Utility;

namespace MomeNicoLibrary.NicoLive.Information
{
	class NgWordItems : XPathItems
	{
		internal NgWordItems()
		{
			requests.Add(new XmlRequest(REGEX));
			requests.Add(new XmlRequest(READONLY));
			requests.Add(new XmlRequest(TYPE));
			requests.Add(new XmlRequest(SOURCE));
			requests.Add(new XmlRequest(REGISTER_TIME));

		}

		// <?xml version=\"1.0\" encoding=\"utf-8\"?>\n
		//<response_ngword status=\"ok\">
		//  <count>200</count>
		//  <ngclient readonly=\"true\">
		//    <type>word</type>
		//    <source>エッチ</source>
		//    <register_time>1213870404</register_time>
		//  </ngclient>
		//</response_ngword>\n

		const string REGEX = "response_ngword/ngclient/@is_regex";
		const string READONLY = "response_ngword/ngclient/@readonly";
		const string TYPE = "response_ngword/ngclient/type";
		const string SOURCE = "response_ngword/ngclient/source";
		const string REGISTER_TIME = "response_ngword/ngclient/register_time";
	}

	public class NgWord : NicoInformation
	{
		private NgWord(Dictionary<string, string> dict)
		{
			this.dict = dict;
		}

		public static NgWord Parse(string message)
		{
			NgWord info = new NgWord(
				XmlParse(message, new NgWordItems()));
			return info;
		}
	}

	//public class NgWordTable
	//{
	//    public NgWordTable()
	//    {
	//        ngList = new List<NgWord>();
	//    }

	//    public int count;
	//    public List<NgWord> ngList;
	//}

	//public struct NgWord
	//{
	//    public string type;
	//    public string source;
	//    public int register_time;
	//    public bool is_readonly;
	//    public bool is_regex;
	//}
}
