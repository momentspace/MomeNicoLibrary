using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MomeNicoLibrary.Utility;

namespace MomeNicoLibrary.NicoLive.Information
{
	public abstract class NicoInformation
	{
		protected Dictionary<string, string> dict = new Dictionary<string, string>();

		///// <summary>
		///// FactoryMethod
		///// </summary>
		///// <param name="message"></param>
		///// <returns></returns>
//		abstract public NicoInformation Parse(string message);

		public bool Status
		{
			get
			{
				return (dict["status"] == "fail") ? false : true;
			}
		}

		public static Dictionary<string, string> XmlParse(string target, XPathItems items)
		{
			string xpath_debug = "";
			Dictionary<string, string> dict = new Dictionary<string, string>();
			try
			{
				// コメントをDOMパーサに読み込む
				XmlDocument xml = new XmlDocument();
				xml.LoadXml(target);

				// ステータス取得
				XmlNode statusNode = xml.SelectSingleNode(items.status);
				if (statusNode != null)
				{
					if (statusNode.InnerText == "ok")
					{
						dict.Add("status", "ok");
					}
					else
					{
						dict.Add("status", "fail");
						return dict;
					}
				}

				// 情報取得
				foreach (XmlRequest request in items)
				{
					xpath_debug = request.xpath;
					XmlNode node = xml.SelectSingleNode(request.xpath);

					string text = "";
					// 必須じゃなく、取得出来なかったとき
					if (!request.needValidate && node == null)
					{
						text = "";
					}
					else
					{
						text = node.InnerText;
					}
					dict.Add(request.xpath, text);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.StackTrace);
				Console.WriteLine("target = " + target);
				Console.WriteLine("xpath = " + xpath_debug);
				throw new Exception("XML Parse Error?", e);
			}
			finally
			{

			}
			return dict;
		}

		public override string ToString()
		{
			string s = "";
			foreach (KeyValuePair<string, string> pair in dict)
			{
				s += pair.Key + " = " + pair.Value + "\n";
			}
			return s;
		}
	}
}
