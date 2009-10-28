using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MomeNicoLibrary.NicoLive.Information;

namespace MomeNicoLibrary.NicoLive
{
	/// <summary>
	/// 現状はアラートもコメント受信クラスと同じ扱い
	/// </summary>
	class AlertReveicer : CommentReceiver
	{
		public AlertReveicer(ServerInformation si) : base(si)
		{
		}
	}
}
