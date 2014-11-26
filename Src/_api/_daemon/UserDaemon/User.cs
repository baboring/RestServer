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
        // Methods
        public User()
        {
            this.RegisterAPI();
        }

        public string AddData(string rawData)
        {
            Dictionary<string, string> dictionary = parser.ToDex(rawData);
            foreach (string str in dictionary.Keys)
            {
                Singleton<Log>.Instance.Write(str);
            }
            if (dictionary["format"] == "text")
            {
                Singleton<Log>.Instance.Write(dictionary["data"]);
                this.UserLogText(dictionary["user"], dictionary["data"]);
            }
            else
            {
                this.SaveImg(dictionary["user"], dictionary["data"]);
            }
            return "";
        }

        public string BuildDayString()
        {
            string str = "";
            object obj2 = str;
            return string.Concat(new object[] { obj2, DateTime.UtcNow.Hour, "-", DateTime.UtcNow.Minute, "-", DateTime.UtcNow.Second });
        }

        public string BuildDayStringS()
        {
            string str = "";
            object obj2 = str;
            return string.Concat(new object[] { obj2, DateTime.UtcNow.DayOfWeek, "-", DateTime.UtcNow.Month });
        }

        public void CreateIfNull(string usr)
        {
            if (!Directory.Exists("Users/"))
            {
                Directory.CreateDirectory("Users");
            }
            if (!Directory.Exists("Users/" + usr + "/"))
            {
                Directory.CreateDirectory("Users/" + usr + "/");
            }
            if (!File.Exists("Users/" + usr + "/usr.cfg"))
            {
                StreamWriter writer = new StreamWriter("Users/" + usr + "/usr.cfg");
                writer.WriteLine("30,1,1,30,50", true);
                writer.Close();
            }
            if (!File.Exists("Users/" + usr + "/" + this.BuildDayStringS()))
            {
                new StreamWriter("Users/" + usr + "/" + this.BuildDayStringS() + ".txt", true).Close();
            }
            if (!Directory.Exists("Users/" + usr + "/" + this.BuildDayStringS() + "_IMG/"))
            {
                Directory.CreateDirectory("Users/" + usr + "/" + this.BuildDayStringS() + "_IMG/");
            }
        }

        public string GetAPI(string rawData)
        {
            Dictionary<string, string> dictionary = parser.ToDex(rawData);
            this.CreateIfNull(dictionary["user"]);
            if (!dictionary.ContainsKey("chmd"))
            {
                StreamReader reader = new StreamReader("Users/" + dictionary["user"] + "/usr.cfg");
                return reader.ReadLine();
            }
            return "";
        }

        public StreamWriter GetUsr(string usr)
        {
            if (!Directory.Exists("Users"))
            {
                Directory.CreateDirectory("Users");
            }
            if (File.Exists("Users/" + usr + ".txt"))
            {
                return new StreamWriter("Users/" + usr + ".txt", true);
            }
            File.Create("Users/" + usr + ".txt");
            StreamWriter writer = new StreamWriter("Users/" + usr + ".txt", true);
            writer.WriteLine("[Created At] = " + DateTime.UtcNow.ToString());
            return writer;
        }

        public void RegisterAPI()
        {
            api item = new api
            {
                version = 0
            };
            item.list.Add(new api_bvh("add", new api_callback(this.AddData)));
            item.list.Add(new api_bvh("g                                                               et", new api_callback(this.GetAPI)));
            Singleton<api_container>.Instance.apis.Add(item);
            Singleton<Log>.Instance.Write("Registered User API ...");
        }

        public void SaveByteArrayAsImage(string fullOutputPath, string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            File.WriteAllBytes(fullOutputPath, bytes);
        }

        public void SaveImg(string usr, string img)
        {
            this.CreateIfNull(usr);
            this.SaveByteArrayAsImage("Users/" + usr + "/" + this.BuildDayStringS() + "_IMG/" + this.BuildDayString() + ".png", img);
        }

        public void UserLogText(string usr, string data)
        {
            this.CreateIfNull(usr);
            StreamWriter writer = new StreamWriter("Users/" + usr + "/" + this.BuildDayStringS() + ".txt", true);
            writer.WriteLine("[" + this.BuildDayString() + "] = " + data);
            writer.Close();
        }

        // Properties
        public StreamWriter sw { get; set; }
    }


}

