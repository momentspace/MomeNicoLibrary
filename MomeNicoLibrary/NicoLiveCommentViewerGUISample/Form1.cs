using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MomeNicoLibrary.NicoLive;
using MomeNicoLibrary.NicoLive.Information;
using MomeNicoLibrary.Utility;
using System.Threading;

namespace NicoLiveCommentViewerGUISample
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			listView1.Columns.Add("P");		// プレミアム
			listView1.Columns.Add("No");		// コメントNo
			listView1.Columns.Add("Time");		// 経過時間
			listView1.Columns.Add("UserId");	// ユーザID
			listView1.Columns.Add("Comment");	// コメント内容
			FormClosing += new FormClosingEventHandler(Form1_Closing);
		}

		private void Form1_Closing(object sender, FormClosingEventArgs e)
		{
			broadcast.Close();
			th.Abort();
		}

		const string mail = "sute@bloodpledge.info";
		const string password = "dpXLD6yH9knRHf7yIi6cyGR2WxDW7wmayyoKEtUeaEoxqA8GDIjg8j+5lAOz/6DZg4fgAdnEo3yUTH4C" +
			"HyrTDu0pP8yj5evqZpWPhFJcZnXELsE/qE2xwNNqcC+BtPnBOHxRjFb9bru/BQrvyEiWQ6ADJOdw58BI" +
			"yt4hdGZp4oY=";

		private Thread th;

		private void method()
		{
			string[] bufs = textBox1.Text.Split('/');
			string bouadcastId = bufs[bufs.Length - 1];

			// リストクリア
			Invoke(new MethodInvoker(delegate()
			{
				listView1.Items.Clear();	
			}));

			// 放送初期化
			broadcast = new NicoLiveBroadcast(bouadcastId, mail, password, RSAKey.SecretKey);
			broadcast.CommentReceivedEvent += new NicoLiveBroadcast.CommentReceivedEventHandler(callback);
			//			broadcast.AddObserver(Program.callback);
			broadcast.Start();
		}

		public void callback(BroadcastComment comment)
		{
			this.comment = comment;
			Invoke(new MethodInvoker(ViewUpdate));
		}

		private BroadcastComment comment;
		private void ViewUpdate()
		{
			listView1.Items.Add(new ListViewItem(new string[] {
				comment.Premium ? "P" : "-",
				comment.Number.ToString(),
				comment.ElapsedTime.ToString(),
				comment.UserId,
				comment.Comment,
			}));
			Application.DoEvents();
		}



		/// <summary>
		/// 放送クラス
		/// </summary>
		NicoLiveBroadcast broadcast;

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				if (textBox1.Text != "")
				{
					th = new Thread(new ThreadStart(method));
					th.Start();
				}
			}
		}

		private void Form1_Leave(object sender, EventArgs e)
		{
		}
	}
}
