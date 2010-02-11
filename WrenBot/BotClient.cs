using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenLib;
using WrenBot.Types;
using Magic;
using WrenBot.PathFinding;

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
        public uint AttackTargetSerial { get; set; }
        public uint ItemTargetSerial;
        public int HPPercent;
        public BotForm BotForm;
        public uint SpellTarget { get; set; }
        public uint LootTarget { get; set; }
        public List<string> GroupMembers { get; set; }
        public List<Point> RequestedTileInfo = new List<Point>();
        public List<ushort> LootableItems = new List<ushort>(new ushort[] {
                Item.Icons.GoldPile,
                Item.Icons.SilverPile
            }
        );
        public List<ushort> BlackListMons = new List<ushort>();
        public Monster MonsterTarget
        {
            get
            {
                try
                {
                    if (Aisling.Map.Entities.ContainsKey(AttackTargetSerial))
                        return (Monster)Aisling.Map.Entities[AttackTargetSerial];
                }
                catch { return null; }
                return null;
            }
            set { }
        }
        #endregion

        #region Accessors
        public uint SocketSerial
        {
            get
            {
                foreach (System.Collections.Generic.KeyValuePair<uint, ProxySocket> Sockets in Proxy.Clients)
                    if (Client.Socket.ConnectedSocket.ID == Sockets.Value.ConnectedSocket.ID)
                        return Sockets.Value.Serial;
                return 0;
            }
        }

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
            this.BotForm = new BotForm();
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
        public MapEntity EntityFromSerial(uint Entity)
        {
            return Aisling.Monsters.Find(((Monster o) => o.Serial == Entity));
        }

        public uint SerialFromEntity(MapEntity Entity)
        {
            return Aisling.Monsters.Find((Monster o) => o.Serial == Entity.Serial).Serial;
        }

        public void WaitForMapLoad()
        {
            try { while (!Aisling.Map.LoadMatrix()) { System.Threading.Thread.Sleep(100); } } catch { }
        }

        public bool IsClient(string Name)
        {
            try
            {
                uint[] ClientSerials = null;
                lock (Clients)
                {
                    ClientSerials = new uint[Clients.Count];
                    Clients.Keys.CopyTo(ClientSerials, 0);
                }
                foreach (uint Serial in ClientSerials)
                {
                    if ((BotClients.ContainsKey(Serial)) && BotClients[Serial].Aisling.Name.ToLower() == Name.ToLower())
                        return true;
                }
            }
            catch { }
            return false;
        }

        public bool HasPathTo(Location Location)
        {
            return Client.Aisling.Map.FindPath(Client.Aisling.Location, Location, true, Proxy, Client.Form) != null;
        }


        #endregion

        #region Collections
        public List<MapEntity> Entities
        {
            get { return Aisling.Map.EntityList; }
        }
        public List<AislingEntity> Aislings
        {
            get { return Aisling.Players; }
        }
        public List<Monster> Monsters
        {
            get { return Aisling.Monsters; }
        }
        public Dictionary<uint, MapEntity> EntitysToRestore
        {
            get;
            set;
        }
        public List<Item> Items
        {
            get { return Aisling.Items; }
        }
        public List<NPC> NPCs
        {
            get { return Aisling.NPCs; }
        }
        #endregion
    }
}
