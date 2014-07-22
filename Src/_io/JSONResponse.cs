using System;
using System.IO;
using System.Text;

namespace RestServer
{
	public class JSONResponse
	{
		public static string To<T>(T obj)
		{
			string retVal = null;
			System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
			using (MemoryStream ms = new MemoryStream())
			{
				serializer.WriteObject(ms, obj);
				retVal = Encoding.Default.GetString(ms.ToArray());
			}
			
			return retVal;
		}
		
		public static T From<T>(string json)
		{
			T obj = Activator.CreateInstance<T>();
			using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
			{
				System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
				obj = (T)serializer.ReadObject(ms);
			}
			
			return obj;
		}


		public static string Build(string messagetype , string statustype , string data)
		{
			//return "{\u0022messagetype\u0022:\u0022" + messagetype + "\u0022,\u0022statustype\u0022:\u0022" + statustype +"\u0022,\u0022data\u0022:\u0022"+data+"\u0022}";
        return "messagetype:"+messagetype+",statustype:"+statustype+ ",data:"+data;
        }
	}
}

