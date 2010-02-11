using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenLib;

namespace WrenBot.Types
{
    public class ClientRoles
    {

        public Roles Role { get; set; }
        public string CharacterName { get; set; }
        public ProxySocket Socket { get; set; }

        public enum Roles
        {
            NonSet = 0,
            IsCaster = 1,
            IsBasher = 2,
            isFollowing = 3,
            IsWatcher = 4
        }
    }
}
