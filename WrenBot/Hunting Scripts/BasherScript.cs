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
using WrenBot.Functions;
using WrenBot.Types;
using WrenLib;

namespace WrenBot
{
    public class BasherScript : Bot
    {
        public Thread BotThread;

        public override void Start()
        {
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            BotThread = new Thread(new ThreadStart(RunningThread));
            BotThread.Start();
        }

        public class Anim
        {
            public byte Action { get { return 0x29; } set { } }
            public byte Ordinal { get; set; }
            public uint ToWho { get; set; }
            public uint FromWho { get; set; }
            public ushort Number { get; set; }
            public ushort NFI { get { return 0x0000; } set { } }
            public ushort Speed { get; set; }
        }

        private void SendAnim(ushort number, uint OnWho)
        {
            Packet Packet = new Packet();
            Packet.Write(new Anim()
            {
                Action = 0x29,
                Ordinal = 0x00,
                ToWho = OnWho,
                FromWho = OnWho,
                Number = number,
                Speed = 0x64
            });
            Client.Socket.SendToClient(Packet.Data, Client.SocketSerial);
        }

        public void RunningThread()
        {
            while (true)
            {
                Client.TargetMonster();
                Monster Target = Client.MonsterTarget;
                if (Target != null)
                {
                    SendAnim(139, Target.Serial);
                }
                Thread.Sleep(10);
            }
        }

        public override void Stop()
        {
            try { BotThread.Abort(); } catch { }
            finally { BotThread = null; }
        }
    }
}
