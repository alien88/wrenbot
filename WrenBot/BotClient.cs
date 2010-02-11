using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenLib;
using WrenBot.Types;
using Magic;

namespace WrenBot
{
    public class BotClient
    {
        #region Fields
        public uint ClientSerial;
        public ProxySocket Socket;
        public NewProxy Proxy;
        public Aisling Aisling;
        public ClientRoles Roles;
        public SetRoles RolesForm;
        public Form1 Form;
        public BlackMagic Magic;
        #endregion

        #region Accessors
        public BotClient Client
        {
            get { return Form.Clients[Socket.ConnectedSocket.ID]; }
        }

        public Dictionary<uint, BotClient> BotClients
        {
            get { return Form.Clients; }
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

        #region Constructors
        public BotClient(Form1 _Form)
        {
            this.Form = _Form;
            this.Aisling = new Aisling();
            this.Roles = new ClientRoles();
            this.RolesForm = new SetRoles(this.Socket);
            this.RolesForm.Set += new RoleEvent(SetRole);
        }
        #endregion

        #region Delegate methods
        #region Set Roles
        public void SetRole(ClientRoles.Roles Role, ProxySocket Socket)
        {
            switch (Role)
            {
                case ClientRoles.Roles.IsBasher:
                    Client.Roles.Role = ClientRoles.Roles.IsBasher;
                    Client.Roles.Socket = Socket;
                    Client.Roles.CharacterName = Socket.Name;
                    Form.GetItemFromSocket(Socket).SubItems[1].Text = Role.ToString();
                    break;
                case ClientRoles.Roles.IsCaster:
                    Client.Roles.Role = ClientRoles.Roles.IsCaster;
                    Client.Roles.Socket = Socket;
                    Client.Roles.CharacterName = Socket.Name;
                    Form.GetItemFromSocket(Socket).SubItems[1].Text = Role.ToString();
                    break;
                case ClientRoles.Roles.IsWatcher:
                    Client.Roles.Role = ClientRoles.Roles.IsWatcher;
                    Client.Roles.Socket = Socket;
                    Client.Roles.CharacterName = Socket.Name;
                    Form.GetItemFromSocket(Socket).SubItems[1].Text = Role.ToString();
                    break;
                case ClientRoles.Roles.isFollowing:
                    Client.Roles.Role = ClientRoles.Roles.isFollowing;
                    Client.Roles.Socket = Socket;
                    Client.Roles.CharacterName = Socket.Name;
                    Form.GetItemFromSocket(Socket).SubItems[1].Text = Role.ToString();
                    break;
                case ClientRoles.Roles.NonSet:
                    Client.Roles.Role = ClientRoles.Roles.NonSet;
                    Client.Roles.Socket = Socket;
                    Client.Roles.CharacterName = Socket.Name;
                    Form.GetItemFromSocket(Socket).SubItems[1].Text = Role.ToString();
                    break;
            }
        }
        #endregion
        #endregion

        #region Methods
        #endregion
    }
}
