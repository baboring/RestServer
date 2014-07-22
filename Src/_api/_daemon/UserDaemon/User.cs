using System;
using Models;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
//using JsonFx.Json;

namespace RestServer
{
    public class User : Singleton<User>
    {
        public User()
        {
            RegisterAPI();
        }

        public void RegisterAPI()
        {
            #region  Register Version 0
            api l_api = new api();
            l_api.version = 0;

            l_api.list.Add(new api_bvh("add", AddData));
            l_api.list.Add(new api_bvh("get", GetAPI));

            api_container.Instance.apis.Add(l_api);
            #endregion

            Log.Instance.Write("Registered User API ...");
        }

       

        public string GetAPI(string rawData)
        {
            Dictionary<string, string> tdex = parser.ToDex(rawData);

            string user = tdex["user"];

            return "";
        }

        public string AddData(string rawData)
        {
            Dictionary<string, string> tdex = parser.ToDex(rawData);

            string user = tdex["user"];

            return "";
        }



        public StreamWriter sw { get; set; }
    }
}

