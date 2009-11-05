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
		/// コメントをキューの最後に追加する
		/// </summary>
		/// <param name="comment"></param>
		public void EnqueueComment(BroadcastComment comment)
		{
			lock (lockObject)
			{
				comments.Enqueue(comment);
			}
		}

		public int Count
		{
			get
			{
				int count;
				lock (lockObject)
				{
					count = comments.Count;
				}
				return count;
			}
		}

		/// <summary>
		/// コメントをキューの最後から取り出す
		/// </summary>
		/// <returns></returns>
		public BroadcastComment DequeueComment()
		{
			BroadcastComment comment;
			lock (lockObject)
			{
				comment = comments.Dequeue();
			}
			return comment;
		}

		private object lockObject = new object();

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
