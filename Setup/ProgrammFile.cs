using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Setup
{
    public class ProgrammFile
    {
        public string Name;
        public byte[] bin;

        public ProgrammFile(string name, byte[] bin)
        {
            Name = name;
            this.bin = bin;
        }

        public ProgrammFile(string name)
        {
            Name = name;
            this.bin = File.ReadAllBytes(name);
        }
    }
}
