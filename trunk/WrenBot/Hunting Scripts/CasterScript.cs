using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.Text.RegularExpressions;

namespace WrenBot
{
    public class CasterScript : Bot
    {
        public Thread BotThread;

        public override void Start()
        {
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            BotThread = new Thread(new ThreadStart(RunningThread));
            BotThread.Start();
        }

        public void RunningThread()
        {
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        public override void Stop()
        {
            try { BotThread.Abort(); } catch { }
            finally { BotThread = null; }
        }
    }
}
