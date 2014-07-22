#define LOG_TO_FILE
#define LOG_ERRORS
#define LOG_TCONSOLE
#define LOG_WARNINGS
#define LOG_DEBUGS
#define WINDOWS

using System;

using System.Collections.Generic;

namespace RestServer
{
	public enum MessageType
	{
		Debug,
		Warning,
		Error
	}
	
	public class Log:Singleton<Log>
	{
        #if WINDOWS
		public const ConsoleColor debug = ConsoleColor.White;
        #elif !WINDOWS
        public const ConsoleColor debug = ConsoleColor.Black;
        #endif

		public const ConsoleColor warning = ConsoleColor.DarkYellow;
		public const ConsoleColor error = ConsoleColor.DarkRed;
		
		public ServerLog srv = new ServerLog();

		public void Write(string data) { Write(data , MessageType.Debug); }

		public void Write(string data , MessageType type)
		{
		#if LOG_TCONSOLE
		Console.ForegroundColor = getColor(type);
        ClearCurrentConsoleLine();
		Console.WriteLine(data);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("=>");

		#endif
			
		#if LOG_TO_FILE
		srv.Write(data , type);
		#endif
		}
		
		public ConsoleColor getColor(MessageType type)
		{
		switch(type)
			{
			case MessageType.Error: return error;
			case MessageType.Warning: return warning;
			default: return debug;
			}
		}

        public void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, currentLineCursor);
        }
	}
		
	public class ServerLog:BaseIO
	{
		#if LOG_ERRORS || LOG_WARNINGS || LOG_DEBUGS
		public List<string> serverlog = new List<string>();
		#endif
		
		public void Write(string data, MessageType mtype)
		{
		#if LOG_ERRORS
		if(mtype == MessageType.Error)
			{
				serverlog.Add("Error:"+data);
				Flush();
			}
		#endif
			
		#if LOG_WARNINGS
		if(mtype == MessageType.Warning)
			{
			}
		#endif
					
		//#if_LOG_DEBUGS
		if(mtype == MessageType.Debug)
			{
			}
	//	#endif
		}
		
		public void Flush()
		{
			if(serverlog.Count >= core.MaxServerLogSize)
			{
			string data = String.Join(Environment.NewLine , serverlog.ToArray());
			
			WriteFile(SystemPath.LogsPath + ExtMath.Timestamp() + ".log",data);
				
			serverlog.Clear();
			}
		}
	}
}

