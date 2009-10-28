using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MomeNicoLibrary.Common
{
	public class UnixEpochTime
	{
		public const long EPOCH = 621355968000000000;
		public static long Now()
		{
			return (DateTime.Now.ToUniversalTime().Ticks - EPOCH) / 10000;
		}

		public static long toUnixEpochTime(long time)
		{
			return (time - EPOCH) / 10000;
		}

		public static DateTime UNIXTimeToDateTime(int timeStamp)
		{
			return new DateTime(1970, 1, 1).AddSeconds((double)timeStamp).ToLocalTime();
		}
	}
}
