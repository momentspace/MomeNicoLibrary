using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MomeNicoLibrary.NicoLive.Information
{
	/// <summary>
	/// 全ての放送情報
	/// not implements
	/// </summary>
	public class BroadcastInformation
	{
		public BroadcastInformation(int baseTime, int openTime, int startTime)
		{
			this.baseTime = baseTime;
			this.openTime = openTime;
			this.startTime = startTime;
		}

		public int baseTime;
		public int openTime;
		public int startTime;

		public int BaseTime
		{
			get
			{
				return baseTime;
			}
		}

		public int OpenTime
		{
			get
			{
				return openTime;
			}
		}

		public int StartTime
		{
			get
			{
				return startTime;
			}
		}
	}
}
