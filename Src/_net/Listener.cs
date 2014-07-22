using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;


namespace RestServer
{
    class Listener
    {
		public Listener(string[] _prefixes)
		{
			prefixes = _prefixes;
			Thread ListenerThr = new Thread(HttpListener); 
			ListenerThr.Start();
		}
		private static string[] prefixes;
		private static bool runListener = true;
		
        private static void HttpListener()
        {
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes. 
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
			
            Log.Instance.Write("Listening...");
			
			while(runListener)
			{
            // Note: The GetContext method blocks while waiting for a request. 
            HttpListenerContext context = listener.GetContext();
         
			lock(RequestQueue.Instance.requests)
				{
				RequestQueue.Instance.Push(new Request(context));	
				}
					/*		}
			HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response. 
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.*/
            //output.Close();
			}
			
            listener.Stop();
        }
    }
}
