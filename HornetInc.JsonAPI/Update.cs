using HornetInc.JsonAPI.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HornetInc.JsonAPI
{
    public static class Update
    {
        public static void Start(ApiManager apiManager, GlobalProjectConfig config, int BuildVersion)
        {
            if (config.NewBuildVersion == BuildVersion)
                return;

            List<byte> ReadedBytes = new List<byte>();
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create($"{apiManager.URLtoAPI}?file=Setup.exe");
            request.Method = "GET";
            request.ContentType = "application/octet-stream";
            request.ContentLength = 0;
            request.Timeout = 2000;

            Debug.WriteLine($"Downloading [Update]");

            using (Stream s = request.GetResponse().GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    int iby;
                    while ((iby = s.ReadByte()) != -1)
                    {
                        ReadedBytes.Add((byte)iby);
                    }
                }
            }
            request.Abort();

            File.WriteAllBytes("Setup.exe", ReadedBytes.ToArray());

            Process proc = new Process();
            proc.StartInfo.FileName = $"Setup.exe";
            proc.StartInfo.Arguments = $"";
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();

            Environment.Exit(0);

        }

        public static void Upload(ApiManager apiManager)
        {
            byte[] buffer = File.ReadAllBytes("Setup.exe");

            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create($"{apiManager.URLtoAPI}?name=Setup.exe");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buffer.Length;


            Debug.WriteLine($"Uploading [GConfig]");

            using (Stream s = request.GetRequestStream())
            {
                s.Write(buffer, 0, buffer.Length);
            }
            request.GetResponse();
            request.Abort();
        }
    }
}
