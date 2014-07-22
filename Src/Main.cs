using System;
using System.Threading;


namespace RestServer
{
    class MainClass
    {
        public static User b_user;

        public static void Main(string[] args)
        {
            core b_core = new core();
            RegisterAPIs();

            Listener api = new Listener(new string[]{"http://localhost:1224/"});

            b_core.Start();

            Thread.Sleep(100);


            while (true)
            {
                b_core.Update();
                Thread.Sleep(16);
            }


        }

        public static void RegisterAPIs()
        {
            b_user = User.Instance;
         
        }
    }
}
