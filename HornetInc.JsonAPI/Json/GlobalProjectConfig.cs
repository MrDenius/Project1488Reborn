using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HornetInc.JsonAPI.Json
{
    public class GlobalProjectConfig
    {
        public int NewBuildVersion;
        public bool Enable;
        public bool CleaningMode;

        public GlobalProjectConfig(int newBuildVersion, bool enable, bool cleaningMode)
        {
            NewBuildVersion = newBuildVersion;
            Enable = enable;
            CleaningMode = cleaningMode;
        }

    }
}
