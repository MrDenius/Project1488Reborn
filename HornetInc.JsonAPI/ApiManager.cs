using HornetInc.JsonAPI.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace HornetInc.JsonAPI
{
    public class ApiManager
    {
        public string URLtoAPI{ get => UrlToApi; }

        string UrlToApi;
        int id;
        
        public int Id { get => id; }

        public ApiManager(string urlToApi, int id)
        {
            UrlToApi = urlToApi;
            this.id = id;
        }

        public ApiManager(string urlToApi)
        {
            UrlToApi = urlToApi;
            this.id = Convert.ToInt32(Read($"HardwaeId={GetUniqueHardwaeId()}")[0]);
            DoOnlineThread.Start(this);
        }

        private static void DoOnline(object obj)
        {
            ApiManager AM = (ApiManager)obj;
            while (true)
            {
                AM.Write($"", "");
                Debug.WriteLine("Status Updated");
                Console.WriteLine("Status Updated");

                Thread.Sleep(5 * 1000);
            }
        }

        Thread DoOnlineThread = new Thread(new ParameterizedThreadStart(DoOnline));

        public Command[] GetNewCommands()
        {
            List<Command> ret = new List<Command>();

            foreach (string com in Read("type=request"))
            {
                string[] splcom = com.Split('|');
                if (splcom.Length == 2)
                    ret.Add(new Command(splcom[0], splcom[1]));
                else
                    Debug.WriteLine($"ERROR! Request splcom.Length {splcom.Length}");
            }

            return ret.ToArray();
        }

        public string[] GetNewAnswers()
        {
            return Read("type=answer");
        }

        public string[] GetUsers()
        {
            return Read("Online=false&mask=Id:{ID}::{TIMEONLINE}", true);
        }
        public string[] GetOnlineUsers()
        {
            return Read("Online=true&mask=Id:{ID}::{TIMEONLINE}", true);
        }

        public void AddNewCommand(Command com)
        {
            Write("type=request", $"{com.Name}|{com.Value}");
        }

        public void AddNewAnswer(string ans)
        {
            Write("type=answer", $"{ans}");
        }





        object LWriteRead = new object();
        private void Write(string args, string value, bool noId = false)
        {
            lock (LWriteRead)
            { 

                byte[] buffer = Encoding.UTF8.GetBytes(value);

                HttpWebRequest request;
                if (noId)
                    request = (HttpWebRequest)WebRequest.Create($"{UrlToApi}?{args}");
                else
                    request = (HttpWebRequest)WebRequest.Create($"{UrlToApi}{id}?{args}");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = buffer.Length;


                Debug.WriteLine($"Uploading [{args}&id={id}]");
            
                using (Stream s = request.GetRequestStream())
                {
                    s.Write(buffer, 0, buffer.Length);
                }
                request.GetResponse();
                request.Abort();
            }
        }

        private string[] Read(string args, bool noId = false)
        {
            lock (LWriteRead)
            {
                    //List<string> ret = new List<string>();
                    string ret = "";
                HttpWebRequest request;
                if (noId)
                    request = (HttpWebRequest)WebRequest.Create($"{UrlToApi}?{args}");
                else
                    request = (HttpWebRequest)WebRequest.Create($"{UrlToApi}{id}?{args}");
                request.Method = "GET";
                request.ContentType = "application/json";
                request.ContentLength = 0;
                request.Timeout = 2000;

                if (noId)
                    Debug.WriteLine($"Downloading [{args}]");
                else
                    Debug.WriteLine($"Downloading [{args}&id={id}]");

                try
                {
                    using (Stream s = request.GetResponse().GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                ret += line;
                            }
                        }
                    }
                    request.Abort();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Read(args, noId);
                }

                try
                {
                    return JsonConvert.DeserializeObject<string[]>(ret);
                }
                catch
                {
                    return new string[1] { ret };

                }
            }
        }

        private static string GetUniqueHardwaeId()
        {
            StringBuilder sb = new StringBuilder();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
                  "SELECT * FROM Win32_Processor");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                sb.Append(queryObj["NumberOfCores"]);
                sb.Append(queryObj["ProcessorId"]);
                sb.Append(queryObj["Name"]);
                sb.Append(queryObj["SocketDesignation"]);

                Console.WriteLine(queryObj["ProcessorId"]);
                Console.WriteLine(queryObj["Name"]);
                Console.WriteLine(queryObj["SocketDesignation"]);
            }

            searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                sb.Append(queryObj["Manufacturer"]);
                sb.Append(queryObj["Name"]);
                sb.Append(queryObj["Version"]);

                Console.WriteLine(queryObj["Manufacturer"]);
                Console.WriteLine(queryObj["Name"]);
                Console.WriteLine(queryObj["Version"]);
            }

            searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                sb.Append(queryObj["Product"]);
                Console.WriteLine(queryObj["Product"]);
            }

            var bytes = Encoding.ASCII.GetBytes(sb.ToString());
            SHA256Managed sha = new SHA256Managed();

            byte[] hash = sha.ComputeHash(bytes);

            return BitConverter.ToString(hash);
        }

    }
}
