using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WrenLib;
using WrenBot.Net;
using WrenBot.PathFinding;
using WrenBot.Types;
using WrenBot;
using System.Runtime.InteropServices;
using Microsoft.CSharp;

namespace WrenBot
{
    public partial class BotForm : Form
    {
        #region Constructors
        public BotForm()
        {
            InitializeComponent();
        }

        public BotForm(Form1 _Form, NewProxy _Proxy, uint _ClientSerial, ProxySocket _Socket)
        {
            InitializeComponent();
            this.BaseForm = _Form;
            this.Proxy = _Proxy;
            this.Socket = _Socket;
            this.ClientSerial = _ClientSerial;
        }
        #endregion

        #region Fields
        public Form1 BaseForm;
        public NewProxy Proxy;
        public ProxySocket Socket;
        public uint ClientSerial;
        public Bot BotInterface { get; set; }
        #endregion

        #region Accessors
        public BotClient Client
        {
            get { return BaseForm.Clients[Socket.ConnectedSocket.ID]; }
        }

        public Dictionary<uint, BotClient> BotClients
        {
            get { return BaseForm.Clients; }
        }

        public Dictionary<uint, ProxySocket> Clients
        {
            get { return Proxy.Clients; }
        }

        public ProxySocket ClientSocket
        {
            get { return Proxy.Clients[ClientSerial]; }
        }
        #endregion

        #region Form Events
        private void BotForm_Load(object sender, EventArgs e)
        {
            this.Text = Client.Aisling.Name;
        }
        #endregion

        /// <summary>
        /// Toggle The Bot InterFace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (BotInterface == null)
            {
                if (Client.Roles.Role == ClientRoles.Roles.IsBasher)
                    BotInterface = new BasherScript();
                else if (Client.Roles.Role == ClientRoles.Roles.IsCaster)
                    BotInterface = new CasterScript();
                //TODO: Add more scripts based on Role.

                else if (Client.Roles.Role == ClientRoles.Roles.NonSet)
                {
                    if (!Client.RolesForm.Visible)
                    {
                        Client.RolesForm.Socket = Socket;
                        Invoke(new MethodInvoker(Client.RolesForm.Show));
                    }
                    else
                    {
                        Invoke(new MethodInvoker(Client.RolesForm.Hide));
                    }
                }
                if (BotInterface != null)
                {
                    BotInterface.BaseForm = BaseForm;
                    BotInterface.ClientSerial = ClientSerial;
                    BotInterface.Proxy = Proxy;
                    BotInterface.Socket = Socket;
                    BotInterface.Start();
                    this.button1.Text = "Stop";
                }
            }
            else
            {
                BotInterface.Stop();
                BotInterface = null;
                this.button1.Text = "Start";
            }
        }

    }
}
