using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ASPMVCProducts
{
	public class RSA
	{
		private static string mPrivateKey;
		private static string mPublicKey;
		private static UnicodeEncoding _encoder = new UnicodeEncoding();

		static RSA()
		{
			var rsa = new RSACryptoServiceProvider();
			var privateParameters = rsa.ExportParameters(true);
			var publicParameters = rsa.ExportParameters(false);
			mPrivateKey = rsa.ToXmlString(true);
			mPublicKey = rsa.ToXmlString(false);
		}

		public static string Decrypt(string data)
		{
			var rsa = new RSACryptoServiceProvider();
			var dataArray = data.Split(new char[] { ',' });
			byte[] dataByte = new byte[dataArray.Length];
			for (int i = 0; i < dataArray.Length; i++)
			{
				dataByte[i] = Convert.ToByte(dataArray[i]);
			}
			rsa.FromXmlString(mPrivateKey);
			var decryptedByte = rsa.Decrypt(dataByte, false);
			return _encoder.GetString(decryptedByte);
		}

		public static string Encrypt(string data)
		{
			var rsa = new RSACryptoServiceProvider();
			rsa.FromXmlString(mPublicKey);
			var dataToEncrypt = _encoder.GetBytes(data);
			var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
			var length = encryptedByteArray.Count();
			var item = 0;
			var sb = new StringBuilder();
			foreach (var x in encryptedByteArray)
			{
				item++;
				sb.Append(x);

				if (item < length)
					sb.Append(",");
			}

			return sb.ToString();
		}
	}
}
