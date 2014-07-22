using System;
using System.Collections.Generic;

namespace RestServer
{
	public delegate string api_callback(string data);
	
	public class api_container:Singleton<api_container>
	{
		public List<api> apis = new List<api>();
		
		public string Exec(int version , string cmd , string rawData)
		{
			int ver = FindAPI(version);
			
			if(ver != -1)
			{
				return apis[ver].Exec(cmd , rawData);
			}
			
			return JSONResponse.Build("5","Error" ,"Not Implemented api: " + version);
		}
		
		public int FindAPI(int version)
		{
			for(int i = 0; i < apis.Count ; i++)
			{
				Console.WriteLine("Ver:"+version);
				
				if(version == apis[i].version)
					return i;
			}
			
			return -1;
		}
	}
	
	public class api
	{
		public int version;
		public List<api_bvh> list= new List<api_bvh>();
	
		public string Exec(string cmd , string rawdata)
		{
			int cmd_m = FindCmd(cmd);
			
			if(cmd_m != -1)
				return list[cmd_m].callback(rawdata);
			
			return JSONResponse.Build("5","Error" ,"Not Implemented sub api /cmd:" + cmd + "/ver :" +cmd_m) ;//TODO: Need to improve this
		}
		
		public int FindCmd(string cmd)
		{
			for(int i = 0; i < list.Count ; i++)
			{
                //Log.Instance.Write(list[i].name);
				if(cmd == list[i].name)
				{
					return i;
				}
			}
			
			return -1;
		}
	}
	
	public class api_bvh
	{
		public api_bvh(string _name , api_callback _callback)
		{
			name = _name;
			callback = _callback;
		}
		
		public string name;
		public api_callback callback;
	}
}

