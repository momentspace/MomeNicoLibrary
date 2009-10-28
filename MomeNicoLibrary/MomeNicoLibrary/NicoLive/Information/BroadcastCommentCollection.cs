using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MomeNicoLibrary.NicoLive.Information;

namespace MomeNicoLibrary.NicoLive
{
	/// <summary>
	/// 放送のコメントコレクション
	/// </summary>
	public class BroadcastCommentCollection
	{
		/// <summary>
		/// コメントリスト
		/// </summary>
		private Queue<BroadcastComment> comments = new Queue<BroadcastComment>();

		/// <summary>
		/// コメント設定
		/// </summary>
		/// <param name="comment"></param>
		public void SetComment(BroadcastComment comment)
		{
			comments.Enqueue(comment);
		}

		/// <summary>
		/// コメント取得
		/// </summary>
		/// <returns></returns>
		public BroadcastComment GetComment()
		{
			return comments.Dequeue();
		}

		/// <summary>
		/// イテレータ実装
		/// </summary>
		/// <returns></returns>
		public IEnumerable<BroadcastComment> GetEnumerator()
		{
			foreach(BroadcastComment comment in comments)
			{
				yield return comment;
			}
		}
	}
}
