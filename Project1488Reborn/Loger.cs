using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Project1488Reborn
{
    public static class Loger
    {
        public static string PathLogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Hornet Inc\\Project1488\\Logs.log");
        public static string LogMask = $"[{DateTime.Now}] <{Process.GetCurrentProcess().ProcessName}>({Process.GetCurrentProcess().Id}) => [text]";

        static object LWrite = new object();

        public static void New(string text)
        {
            if (!Directory.Exists(Path.GetDirectoryName(PathLogFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(PathLogFile));

            Console.WriteLine(text);

            new Thread(new ThreadStart(() =>
            {
                LogMask = $"[{DateTime.Now}] <{Process.GetCurrentProcess().ProcessName}>({Process.GetCurrentProcess().Id}) => [text]";
                lock (LWrite)
                    WriteToFile(text);
            })).Start();
        }

        static void WriteToFile(string text)
        {
            using (Stream s = new FileStream(PathLogFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.WriteLine(LogMask.Replace("[text]", text));
                }
            }
        }
    }
}
