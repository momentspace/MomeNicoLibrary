using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Data.SQLite;
using System.Data;

namespace MomeNicoLibrary.Utility
{
	public class FirefoxCookie : IDisposable
	{
		private string filepath;
		public FirefoxCookie(string filepath)
		{
			const string MSG_ILLIGAL_PATH = "指定されたファイルが存在しませんでした";
			const string MSG_DB_BROKEN = "Firefoxのデータベースファイルが壊れています";
			const string MSG_UNKNOWN = "not implements";

			// 存在チェック
			if (!File.Exists(filepath))
			{
				throw new FileNotFoundException(MSG_ILLIGAL_PATH);
			}

			// オープンチェック
			string tmppath = Path.GetTempFileName();
			try
			{
				File.Copy(filepath, tmppath, true);
				const string SQLITE_HEADER = "SQLite";
				using (StreamReader reader = new StreamReader(tmppath))
				{
					// ヘッダチェック
					char[] bufs = new char[6];
					reader.Read(bufs, 0, bufs.Length);
					if (SQLITE_HEADER != new string(bufs))
					{
						throw new FileLoadException(MSG_DB_BROKEN);
					}
				}
			}
			catch (Exception e)
			{
				throw new FileLoadException(MSG_UNKNOWN);
			}

			// 保存
			this.filepath = tmppath;
		}

		public CookieCollection GetCookie()
		{
			string value = "";
			const string DOMAIN = ".nicovideo.jp";
			const string KEY = "user_session";
			const string TABLE = "moz_cookies";
	        string QUERY = string.Format("SELECT * FROM {2}  WHERE host = \"{0}\" AND name = \"{1}\"", DOMAIN, KEY, TABLE);
			using (SQLiteConnection con = new SQLiteConnection("Data Source=" + filepath))
			using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(QUERY, con))
			{
				DataSet ds = new DataSet();
				adapter.Fill(ds);
				try
				{
					value = (string)ds.Tables[0].Rows[0][2];
				}
				catch (Exception e)
				{
				}
			}
	
			CookieCollection cookies = new CookieCollection();
			cookies.Add(new Cookie(KEY, value, "/", DOMAIN));

			return cookies;
		}

		#region IDisposable メンバ

		public void Dispose()
		{
			File.Delete(this.filepath);
		}

		#endregion
	}
}
