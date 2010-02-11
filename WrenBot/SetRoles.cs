using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WrenBot.Types;
using WrenLib;

namespace WrenBot
{
    public delegate void RoleEvent(ClientRoles.Roles Role, ProxySocket Socket);
    public partial class SetRoles : Form
    {
        public ProxySocket Socket;
        public RoleEvent Set;
        public SetRoles(ProxySocket _Socket)
        {            
            InitializeComponent();
            this.Socket = _Socket;
        }

        private void SetRoles_Load(object sender, EventArgs e)
        {
            this.Text = "Set Role for: " + Socket.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Set(ClientRoles.Roles.IsBasher, Socket);
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Set(ClientRoles.Roles.IsCaster, Socket);
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Set(ClientRoles.Roles.IsWatcher, Socket);
            this.Hide();
        }
    }
}
