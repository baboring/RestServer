using System;
using System.Text;
using System.IO;
using System.Globalization;

namespace RestServer
{
	public class ExtMath
	{
		public static string FromB64(string _data)
		{
			byte[] data = Convert.FromBase64String(_data);
			return Encoding.UTF8.GetString(data);
		}
		
		public static string Timestamp()
		{
			return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:fff",CultureInfo.InvariantCulture);
		}
	}
}

