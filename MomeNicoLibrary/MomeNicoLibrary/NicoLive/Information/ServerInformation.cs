using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MomeNicoLibrary.NicoLive.Information
{
	public class ServerInformation
	{
		public ServerInformation(string address, int thread, int port)
		{
			this.address = address;
			this.thread = thread;
			this.port = port;
		}

		private string address;
		private int thread;
		private int port;

		public string Address
		{
			get
			{
				return address;
			}
		}

		public int Thread
		{
			get
			{
				return thread;
			}
		}

		public int Port
		{
			get
			{
				return port;
			}
		}
	}
}
