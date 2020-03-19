using HornetInc.JsonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1488Reborn.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }


        internal ApiManager apiManager { get => Program.apiManager; }

        public bool IsTrueName(string name)
        {

            if (Name == name)
                return true;
            else
                return false;
        }

        public abstract void Execute(HornetInc.JsonAPI.Json.Command com);
    }
}
