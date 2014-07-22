using System;
using System.Reflection;
using System.Web;

namespace RestServer
{
	public class container:Singleton<container>
	{
		
	public string processdata(string data)
		{ 
		//	try
		//	{
                data = Uri.UnescapeDataString(data);
                Log.Instance.Write(data);
			string[] rawdata = data.Split("~"[0]);

			string[] data1 = rawdata[0].Split(":"[0]);
			
			return api_container.Instance.Exec(int.Parse(data1[0]) , data1[1],rawdata[1]);
		//	}
			//catch(System.Exception e)
			//{
			//return JSONResponse.Build("5","Error" ,""+e.Data + e.Message);
		//	}
		}
	
        public string processInputData(string data)
        {
            data.Replace("%3a" , ":");
            data.Replace("%2c" , ",");

            return data;
        }
    }

	public struct RequestParser
	{	
		public int version_id;
		public string cmd;
		public string rawdata;
		public string[] keys;
		public string[] values;
	}
}

