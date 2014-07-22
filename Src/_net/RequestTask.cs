using System;
using System.IO;
using System.Text;
using System.Threading;

namespace RestServer
{
	public class RequestTask
	{	
		public static void ProcessTasks()
		{
			while(true)
			{
				Request request;
				
				lock(RequestQueue.Instance.requests)
				{
					request = RequestQueue.Instance.Pop();
				}
				
				if(request != null)
				{			
				
				string text="";
				                                                     
				using (var reader = new StreamReader(request.context.Request.InputStream,
				                                     request.context.Request.ContentEncoding))
				{
				    text = reader.ReadToEnd();
				}
                text = text.Substring(1);

                Log.Instance.Write(text , MessageType.Debug);
				string responsedata = container.Instance.processdata(text);
				
				byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responsedata);
            
	            request.context.Response.ContentLength64 = buffer.Length;
	            System.IO.Stream output = request.context.Response.OutputStream;
	            output.Write(buffer, 0, buffer.Length);
            	output.Close();
				
				
				} 
				
				
			}
		}
	}
}

