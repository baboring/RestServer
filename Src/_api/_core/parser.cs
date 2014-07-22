using System;
using System.Collections.Generic;

namespace RestServer
{
	public class parser
	{
		public static Dictionary<string , string> ToDex(string rawdata)
		{
			//TODO , This is just a testing placeholder , modify this to respose class deserialization and dont forget encrypt data to protect against MIM attack
			try
			{
				Dictionary<string , string> req = new Dictionary<string, string>();
				
				string[] r_1 = rawdata.Split(","[0]);
				
				for(int i = 0 ; i < r_1.Length ; i++)
				{
					string[] r_2 = r_1[i].Split(":"[0]);
					//req.Add(ExtMath.FromB64(r_2[0]) , ExtMath.FromB64(r_2[1]));
					req.Add(r_2[0] , r_2[1]); // For webtesting purposes only
				}
				
				return req;
			}
			catch(System.Exception e)
			{
				Console.Write("Error:"+e.Message);
			}
			
			return null;
		}
		
		
	}
}

