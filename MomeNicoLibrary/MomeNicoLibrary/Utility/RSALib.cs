using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MomeNicoLibrary.Utility
{
	public class RSALib
	{
		/// <summary>
		/// 公開鍵と秘密鍵を作成して返す
		/// </summary>
		/// <param name="publicKey">作成された公開鍵(XML形式)</param>
		/// <param name="privateKey">作成された秘密鍵(XML形式)</param>
		public static void CreateKeys(out string publicKey, out string privateKey)
		{
			//RSACryptoServiceProviderオブジェクトの作成
			System.Security.Cryptography.RSACryptoServiceProvider rsa =
				new System.Security.Cryptography.RSACryptoServiceProvider();

			//公開鍵をXML形式で取得
			publicKey = rsa.ToXmlString(false);
			//秘密鍵をXML形式で取得
			privateKey = rsa.ToXmlString(true);
		}

		/// <summary>
		/// 公開鍵を使って文字列を暗号化する
		/// </summary>
		/// <param name="str">暗号化する文字列</param>
		/// <param name="publicKey">暗号化に使用する公開鍵(XML形式)</param>
		/// <returns>暗号化された文字列</returns>
		public static string Encrypt(string str, string publicKey)
		{
			//RSACryptoServiceProviderオブジェクトの作成
			System.Security.Cryptography.RSACryptoServiceProvider rsa =
				new System.Security.Cryptography.RSACryptoServiceProvider();

			//公開鍵を指定
			rsa.FromXmlString(publicKey);

			//暗号化する文字列をバイト配列に
			byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
			//暗号化する
			//（XP以降の場合のみ2項目にTrueを指定し、OAEPパディングを使用できる）
			byte[] encryptedData = rsa.Encrypt(data, false);

			//Base64で結果を文字列に変換
			return System.Convert.ToBase64String(encryptedData);
		}

		/// <summary>
		/// 秘密鍵を使って文字列を復号化する
		/// </summary>
		/// <param name="str">Encryptメソッドにより暗号化された文字列</param>
		/// <param name="privateKey">復号化に必要な秘密鍵(XML形式)</param>
		/// <returns>復号化された文字列</returns>
		public static string Decrypt(string str, string privateKey)
		{
			//RSACryptoServiceProviderオブジェクトの作成
			System.Security.Cryptography.RSACryptoServiceProvider rsa =
				new System.Security.Cryptography.RSACryptoServiceProvider();

			//秘密鍵を指定
			rsa.FromXmlString(privateKey);

			//復号化する文字列をバイト配列に
			byte[] data = System.Convert.FromBase64String(str);
			//復号化する
			byte[] decryptedData = rsa.Decrypt(data, false);

			//結果を文字列に変換
			return System.Text.Encoding.UTF8.GetString(decryptedData);
		}
	}
}
