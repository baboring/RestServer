using System;
using System.Net;

namespace RestServer
{
	public interface IRequest
	{
		int uid {get; set;}
		HttpListenerContext context {get; set;}
		
		IResponse BuildResponse(HttpMethod method);
		void Dispose();
	}
}

