/*
 * Revision 0.1.5 
 *  
 */

using System;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace Models
{
	public class UserModels
	{
		public class NewUserModel
		{
			public string user {get ; set ;}
			public string password {get; set;}
			public string email {get; set;}
			
			public void SetData(string data)
			{
				
			}
		}
		
        [XmlRoot("ClientView")][System.Serializable]
        public class UserClientView
        {
            public UserClientView(){}


        
            public UserClientView GetBase()
            {
                return this;
            }
        }

        [XmlRoot("Root")][System.Serializable]
        public class UserBase:UserClientView
		{
			public UserBase()
			{
				
			}


            public string username;

            public List<string> data= new List<string>();

            //TODO remove serializers form here
            public static string Serialize<T>(T tData)
		    {
		        var serializer = new XmlSerializer(typeof(T));
		
		        TextWriter writer = new StringWriter();
		        serializer.Serialize(writer, tData);
		
		        return writer.ToString();
		    }

		    public static T Deserialize<T>(string tData)
		    {

		        var serializer = new XmlSerializer(typeof(T));
		
		        TextReader reader = new StringReader(tData);
		
		        return (T)serializer.Deserialize(reader);
		    }
          
			
			public void save()
			{
				string data = UserBase.Serialize(this);
				
				StreamWriter sw = new StreamWriter(RestServer.SystemPath.UserPath + username + ".dat");
				sw.Write(data);
				sw.Close();
			}

            public string GetXML()
            {
                return UserBase.Serialize(this);
            }

			

			public bool remove()
			{
				try
				{
					File.Delete(RestServer.SystemPath.UserPath + username + ".dat");
                    return true;
				}
				catch(System.Exception e)
				{
					RestServer.Log.Instance.Write("Error:" + e.Data + e.Message);
                    return false;
                }
			}

		
			private string GenerateUID()
			{
				return System.Guid.NewGuid().ToString();
			}
		}
	}
}

