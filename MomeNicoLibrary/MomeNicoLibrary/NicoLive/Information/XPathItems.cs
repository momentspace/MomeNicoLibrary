using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MomeNicoLibrary.NicoLive.Information
{
	public class XmlRequest
	{
		public XmlRequest(string xpath)
		{
			this.xpath = xpath;
			this.needValidate = true;
		}

		public XmlRequest(string xpath, bool needValidate)
		{
			this.xpath = xpath;
			this.needValidate = needValidate;
		}

		public string xpath;
		public bool needValidate;
	}

	abstract public class XPathItems : IEnumerable<XmlRequest>, System.Collections.IEnumerable
	{
		public string status = "dummy";
		protected List<XmlRequest> requests = new List<XmlRequest>();
		public IEnumerator<XmlRequest> GetEnumerator()
		{
			return this.requests.GetEnumerator();
		}

		#region IEnumerable メンバ

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return requests.GetEnumerator();
		}

		#endregion
	}
}
