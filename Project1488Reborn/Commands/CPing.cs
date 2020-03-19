using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HornetInc.JsonAPI.Json;

namespace Project1488Reborn.Commands
{
    class CPing : Command
    {
        public override string Name => "ping";


        public override void Execute(HornetInc.JsonAPI.Json.Command com)
        {
            apiManager.AddNewAnswer("Ping");
            apiManager.AddNewAnswer("Ping");
            apiManager.AddNewAnswer("Ping");
            apiManager.AddNewAnswer("Ping");
            apiManager.AddNewAnswer("Ping");
            apiManager.AddNewAnswer("Ping");
            apiManager.AddNewAnswer("Ping");
            apiManager.AddNewAnswer("Ping");
            apiManager.AddNewAnswer("Ping");
            apiManager.AddNewAnswer("Ping");
            apiManager.AddNewAnswer("Ping");
            if (com.Value.Length == 0)
                apiManager.AddNewAnswer($"Pong!");
            else
                apiManager.AddNewAnswer($"Pong {com.Value}!");
        }
    }
}
