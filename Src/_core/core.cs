using System;
using System.Threading;
using System.Reflection;

namespace RestServer
{
	public class core:Singleton<core>
	{

		public const int MaxServerLogSize = 5000;
		
		//public event System.Action OnUpdate;
		
        //public SQLite.SqLiteDatabase sqlite_default = new SQLite.SqLiteDatabase();
		public Thread ReqTaskSeq;
		
		public void Start()
		{
            //Log.Instance.Write( sqlite_default.OpenConnection() == true ? "Opened Connection to db":"Failed to open conn to db");

			ReqTaskSeq = new Thread(RequestTask.ProcessTasks);
			ReqTaskSeq.Start();
			
			//PluginRouter.register();
            Log.Instance.Write("On Start ...");
		}
		
		public void Update()
		{
			//if(OnUpdate != null)
			//	OnUpdate();
			
            string cmd = Console.ReadLine();

            string[] args = cmd.Split(" "[0]);

            try
                {
                typeof(ServerCMD).GetMethod(args[0]).Invoke(ServerCMD.Instance , new [] {args});
                }
            catch(System.Exception e)
                {
                Log.Instance.Write(e.Message,MessageType.Error);
                }

		}
		
		public void LateUpdate()
		{
			
		}
	}

    public class ServerCMD:Singleton<ServerCMD>
    {
        public void buildsql(object[] data)
        {
           /* Log.Instance.Write(core.Instance.sqlite_default.ExecuteScalar(
                    @"CREATE TABLE IF NOT EXISTS `default` (
                    `username` varchar(20) NOT NULL,
                    `userdata` varchar(10000) NOT NULL)"));*/
        }

        public void report(object[] data)
        {

        }

        public void status(object[] data)
        {
            Log.Instance.Write(User.Instance == null ? "User Daemon not initialized" : "User Daemon Initialized");
            }

        public void nitem(object[] data)
        {
                //Inventory.Instance.items.Add();
        }

       
    }
}

