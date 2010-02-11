using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;
using WrenLib;

namespace WrenBot
{
    public class Bot : MarshalByRefObject
    {

        #region Base Vars
        public Form1 BaseForm { get; set; }
        public NewProxy Proxy { get; set; }
        public ProxySocket Socket { get; set; }
        public uint ClientSerial { get; set; }
        public BotClient Client
        {
            get
            {
                return BaseForm.Clients[ClientSerial];
            }
        }
        public Aisling Aisling
        {
            get { return Client.Aisling; }
            set { Client.Aisling = value; }
        }
        public Map Map
        {
            get { return Aisling.Map; }
            set { Client.Aisling.Map = value; }
        }
        public List<AislingEntity> Players
        {
            get { return Aisling.Players; }
        }
        public List<Monster> Monsters
        {
            get { return Client.Monsters; }
        }
        public List<NPC> Aislings
        {
            get { return Client.NPCs; }
        }
        public List<Item> Items
        {
            get { return Aisling.Items; }
        }
        #endregion

        public virtual void Start() { }
        public virtual void Stop() { }
        public virtual void OnSpellBar() { }
        public virtual void OnAnimation() { }
    }
}