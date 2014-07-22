using System;

namespace RestServer
{
	public class Request:IRequest
	{
		public Request(System.Net.HttpListenerContext _context)
		{
			context = _context;
		}

		#region IRequest implementation
		public IResponse BuildResponse (HttpMethod method)
		{
			throw new NotImplementedException ();
		}

		public void Dispose ()
		{
			throw new NotImplementedException ();
		}

		public int uid 
		{
			get {
				throw new NotImplementedException ();
			}
			
			set {
				throw new NotImplementedException ();
			}
		}

		public System.Net.HttpListenerContext context 
		{
			get {
				return l_context;
			}
			
			set {
				l_context = value;
			}
		}
		private System.Net.HttpListenerContext l_context;
		#endregion
	}
}

