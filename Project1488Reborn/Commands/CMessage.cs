using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HornetInc.JsonAPI.Json;

namespace Project1488Reborn.Commands
{
    class CMessage : Command
    {
        public override string Name => "message";

        public override void Execute(HornetInc.JsonAPI.Json.Command com)
        {
            new Thread(new ThreadStart(() => { MessageBox.Show(com.Value, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning); })).Start();
        }
    }
}
