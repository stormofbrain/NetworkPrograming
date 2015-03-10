using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace ConsoleApplication6
{

    struct DirectoryItem
    {
        public Uri BaseUri;

        public string AbsolutePath
        {
            get
            {
                return string.Format("{0}/{1}", BaseUri, Name);
            }
        }

        
        public bool IsDirectory;
        public string Name;
        public List<DirectoryItem> Items;
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<DirectoryItem> listing = GetDirectoryInformation("ftp://bambaramia.esy.es", "u232536431", "123456789");
            Console.ReadKey();
        }


        static List<DirectoryItem> GetDirectoryInformation(string address, string username, string password)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(address);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            List<DirectoryItem> returnValue = new List<DirectoryItem>();
            string[] list = null;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                list = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }

            foreach (string line in list)
            {
                
                string data = line;

                
                data = data.Remove(0, 24);

                
                string dir = data.Substring(0, 5);
                bool isDirectory = dir.Equals("<dir>", StringComparison.InvariantCultureIgnoreCase);
                data = data.Remove(0, 5);
                data = data.Remove(0, 10);

               
                string name = data;

                
                DirectoryItem item = new DirectoryItem();
                item.BaseUri = new Uri(address);
                
                item.IsDirectory = isDirectory;
                item.Name = name;

                Debug.WriteLine(item.AbsolutePath);
                item.Items = item.IsDirectory ? GetDirectoryInformation(item.AbsolutePath, username, password) : null;

                returnValue.Add(item);
            }

            return returnValue;
        }
    }
}
