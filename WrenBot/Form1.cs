using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WrenLib;
using Magic;
using System.Diagnostics;

namespace WrenBot
{
    public partial class Form1 : Form
    {
        public Dictionary<uint, BotClient> Clients = new Dictionary<uint, BotClient>();

        public NewProxy Proxy = null;
        public Form1()
        {
            Form.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Proxy = new NewProxy(2610);
            Proxy.OnConnect += new SerialDelegate(_OnConnect);
            Proxy.OnDisconnect += new SerialDelegate(_OnDisconnect);
            Proxy.OnGameServerConnect += new SerialDelegate(_OnGameEnter);
            Proxy.OnGameEnter += new OnGameLogin(_OnGameLogin);

        }

        public void _OnConnect(ProxySocket Socket, uint ClientSerial, NewProxy Proxy)
        {
            int BotClients = Clients.Count();
            if (Proxy.Clients.ContainsKey(ClientSerial))
            {
                if (!Clients.ContainsKey(ClientSerial))
                {
                    Clients.Add(ClientSerial, new BotClient(this)
                    {
                        ClientSerial = ClientSerial,
                        Proxy = Proxy,
                        Socket = Socket
                    });
                }
            }
        }

        public BlackMagic Magic;
        public string Character;
        public BlackMagic EnableMagic(string who, ProxySocket Socket)
        {
            Process[] p = Process.GetProcessesByName("Darkages");
            foreach (Process s in p)
            {
                Magic = new BlackMagic(s.MainWindowHandle);
                var o = Magic.ReadASCIIString(0x0075E850, 20);
                if (this.Character.ToLower() == o.ToLower())
                {
                    return Magic;
                }
            }
            return null;
        }

        public void _OnGameLogin(ProxySocket Socket, uint ClientSerial, NewProxy Proxy, string CharacterName)
        {
            this.Character = CharacterName;
            Socket.Name = CharacterName;
            Clients[Socket.ConnectedSocket.ID].Aisling.Name = CharacterName;
            Clients[ClientSerial].Roles = new WrenBot.Types.ClientRoles()
            {
                CharacterName = CharacterName,
                Role = WrenBot.Types.ClientRoles.Roles.NonSet,
                Socket = Socket
            };
            ListViewItem I = new ListViewItem(new string[] { Clients[ClientSerial].Roles.CharacterName, Clients[ClientSerial].Roles.Role.ToString(), "Idle" });
            listView1.Items.Add(I);
        }

        public void _OnDisconnect(ProxySocket Socket, uint ClientSerial, NewProxy Proxy)
        {
            try
            {
                if (Clients[ClientSerial].Magic.ReadByte(0x00780488) == 1)
                {
                    if (Proxy.Clients.ContainsKey(ClientSerial))
                    {
                        Proxy.Clients.Remove(ClientSerial);
                        listView1.Items.Remove(GetItemFromSocket(Socket));
                    }
                    Clients.Remove(ClientSerial);
                }
            }
            catch
            {
            }
            RemoveUnwantedItems();
        }

        public void _OnGameEnter(ProxySocket Socket, uint ClientSerial, NewProxy Proxy)
        {
            BlackMagic M = EnableMagic(Socket.Name, Socket); 
            Clients[ClientSerial].Magic = M;
        }

        public ListViewItem GetItemFromSocket(ProxySocket Socket)
        {
            foreach (ListViewItem s in listView1.Items)
            {
                if (Socket.Name.ToLower() == s.SubItems[0].Text.ToLower())
                {
                    return s;
                }
            }
            return null;
        }

        public void RemoveUnwantedItems()
        {
            foreach (ListViewItem f in listView1.Items)
            {
                string n = f.SubItems[0].Text.ToLower();
                try
                {
                    var Socket = (from v in Proxy.Clients
                                  where v.Value.Name.ToLower() == n.ToLower()
                                  select v.Value).Single();
                }
                catch
                {
                    listView1.Items.Remove(f);
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var i = (ListView)sender;
                string n = listView1.SelectedItems[0].SubItems[0].Text;
                var Socket = (from v in Proxy.Clients
                              where v.Value.Name.ToLower() == n.ToLower()
                              select v.Value).Single();
                if (Socket != null)
                {
                    if (!Clients[Socket.ConnectedSocket.ID].RolesForm.Visible)
                    {
                        Clients[Socket.ConnectedSocket.ID].RolesForm.Socket = Socket;
                        Invoke(new MethodInvoker(Clients[Socket.ConnectedSocket.ID].RolesForm.Show));
                    }
                    else
                    {
                        Invoke(new MethodInvoker(Clients[Socket.ConnectedSocket.ID].RolesForm.Hide));
                    }
                }
            }
            catch 
            {
            }
        }
    }
}
