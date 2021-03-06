﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotnetSpider.Enterprise.Core
{
	public static class CryptoUtil
	{
		private const string AesKey = "wtt0wI9Q<K}kC;8)Qvbifo7*IO#VF/.K";
		private const string AesVector = "DX&crmK]P@&U=#*R";

		public static string Md5Encrypt(string myString)
		{
			MD5 md5 = MD5.Create();

			byte[] fromData = Encoding.UTF8.GetBytes(myString);
			byte[] targetData = md5.ComputeHash(fromData);

			return BitConverter.ToString(targetData).Replace("-", "").Substring(8, 16).ToLower();
		}

		public static string FullMd5Encrypt(string myString)
		{
			MD5 md5 = MD5.Create();

			byte[] fromData = Encoding.UTF8.GetBytes(myString);
			byte[] targetData = md5.ComputeHash(fromData);

			return BitConverter.ToString(targetData).Replace("-", "").ToLower();
		}

		/// <summary>
		/// AES加密
		/// </summary>
		/// <param name="plainStr">明文字符串</param>
		/// <returns>密文</returns>
		public static string AesEncrypt(string plainStr)
		{
			byte[] bKey = Encoding.UTF8.GetBytes(AesKey);
			byte[] bIV = Encoding.UTF8.GetBytes(AesVector);
			byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

			string encrypt = null;
			var aes = Aes.Create();
			try
			{
				using (MemoryStream mStream = new MemoryStream())
				{
					using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
					{
						cStream.Write(byteArray, 0, byteArray.Length);
						cStream.FlushFinalBlock();
						encrypt = Convert.ToBase64String(mStream.ToArray());
					}
				}
			}
			catch (Exception)
			{
				// ignored
			}

			aes.Dispose();

			return encrypt;
		}

		/// <summary>
		/// AES加密
		/// </summary>
		/// <param name="plainStr">明文字符串</param>
		/// <param name="returnNull">加密失败时是否返回 null，false 返回 String.Empty</param>
		/// <returns>密文</returns>
		public static string AesEncrypt(string plainStr, bool returnNull)
		{
			string encrypt = AesEncrypt(plainStr);
			return returnNull ? encrypt : (encrypt == null ? String.Empty : encrypt);
		}

		/// <summary>
		/// AES解密
		/// </summary>
		/// <param name="encryptStr">密文字符串</param>
		/// <returns>明文</returns>
		public static string AesDecrypt(string encryptStr)
		{
			byte[] bKey = Encoding.UTF8.GetBytes(AesKey);
			byte[] bIv = Encoding.UTF8.GetBytes(AesVector);
			byte[] byteArray = Convert.FromBase64String(encryptStr);

			string decrypt = null;
			var aes = Aes.Create();
			try
			{
				using (MemoryStream mStream = new MemoryStream())
				{
					using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIv), CryptoStreamMode.Write))
					{
						cStream.Write(byteArray, 0, byteArray.Length);
						cStream.FlushFinalBlock();
						decrypt = Encoding.UTF8.GetString(mStream.ToArray());
					}
				}
			}
			catch (Exception)
			{
				// ignored
			}

			aes.Dispose();

			return decrypt;
		}

		/// <summary>
		/// AES解密
		/// </summary>
		/// <param name="encryptStr">密文字符串</param>
		/// <param name="returnNull">解密失败时是否返回 null，false 返回 String.Empty</param>
		/// <returns>明文</returns>
		public static string AesDecrypt(string encryptStr, bool returnNull)
		{
			string decrypt = AesDecrypt(encryptStr);
			return returnNull ? decrypt : (decrypt ?? String.Empty);
		}
	}
}
