using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HornetInc.JsonAPI.Json
{
    public class Command
    {
        public string Name;
        public string Value;

        public Command(string command)
        {
            string[] cc = command.Split(' ');

            Name = cc[0];
            Value = command.Replace(cc[0], "").Trim();
        }

        public Command(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
