using System;
using System.IO;

namespace RestServer
{
	public class SystemPath
	{
		public static void Init()
		{
			if(Directory.Exists(UserPath) == false)
			{
				Directory.CreateDirectory(UserPath);
			}
		}
		
		public static string UserPath = "UserColection/";
		public static string LogsPath = "ServerLogs/";
		public static string ItemsPath = "Items/";
	}
}

