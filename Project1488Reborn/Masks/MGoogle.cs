using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Project1488Reborn.Masks
{
    class MGoogle : Mask
    {
        public override DirectoryInfo MaskDir => new DirectoryInfo($"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}\\Google\\Update\\");

        public override string NameMainFile => "GoogleUpdateHandle.exe";

        public override Icon icon => Properties.Resources.Google;

        public override bool Enable => true;
    }
}
