using System;
using System.Collections.Generic;

namespace RestServer
{
	public class RequestQueue:Singleton<RequestQueue>
	{
		public List<Request> requests = new List<Request>();
		
		public void Push(Request request)
		{
			requests.Add(request);
		}
		
		public Request Pop()
		{
			//Console.WriteLine(requests.Count);
			if(requests.Count == 0)
				return null; 
			
			Request request = requests[0];
			requests.RemoveAt(0);
			
			return request;
		}
	}
}

