using System;
using System.IO;

namespace RestServer
{
	public class BaseIO
	{
		public static string ReadFile(string path )
		{
			try
				{
				StreamReader sr = new StreamReader(path);
				string data = sr.ReadToEnd();
				sr.Close();
				return data;
				}
				catch(System.Exception e)
				{
				return "";
				}
		}
		
        public static bool WriteFile(string path, string data)
		{
			try
				{
				StreamWriter sw = new StreamWriter(path);
				sw.Write(data);
				sw.Flush();
				sw.Close();
				return true;
				}
			catch(System.Exception e)
				{
				return false;
				}
		}
	}
}

