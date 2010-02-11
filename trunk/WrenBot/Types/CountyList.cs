using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Online List View Object
    /// </summary>
    public class OnlineListView
    {
        /// <summary>
        /// Default Online List View Constructor
        /// </summary>
        /// <param name="OnlineUsers">Number Of Online Users</param>
        /// <param name="ListedUsers">Number Of Listed Users</param>
        /// <param name="Users">Array Of Users</param>
        public OnlineListView(ushort OnlineUsers, ushort ListedUsers, User[] Users)
        {
            this.OnlineUsers = OnlineUsers;
            this.ListedUsers = ListedUsers;
            this.Users = Users;
        }

        /// <summary>
        /// Online List User Object
        /// </summary>
        public class User
        {
            /// <summary>
            /// Default Online List User Constructor
            /// </summary>
            /// <param name="Class">Users Class</param>
            /// <param name="ListColor">Users Listing Color</param>
            /// <param name="StatusIcon">Users Status Icon</param>
            /// <param name="Title">Users Title</param>
            /// <param name="MasterBoolean">Boolean: Is User Master?</param>
            /// <param name="Name">Users Name</param>
            public User(CharactorClass Class, byte ListColor, byte StatusIcon, string Title, byte MasterBoolean, string Name)
            {
                this.Class = Class;
                this.ListColor = ListColor;
                this.StatusIcon = StatusIcon;
                this.Title = Title;
                this.MasterBoolean = MasterBoolean;
                this.Name = Name;
            }

            /// <summary>
            /// Charactor Class Enumeration
            /// </summary>
            public enum CharactorClass { Peasant = 0x00, Warrior = 0x01, Rogue = 0x02, Wizard = 0x03, Priest = 0x04, Monk = 0x05, Gladiator = 0x06, Archer = 0x07, Summoner = 0x08, Bard = 0x09, Druid = 0x10 }

            /// <summary>
            /// Users Charactor Class
            /// </summary>
            CharactorClass Class;

            /// <summary>
            /// Users Listing Color
            /// </summary>
            public byte ListColor;

            /// <summary>
            /// Users Status Icon
            /// </summary>
            public byte StatusIcon;

            /// <summary>
            /// Users Title
            /// </summary>
            public string Title;

            /// <summary>
            /// Boolean: Is User Mastered?
            /// </summary>
            public byte MasterBoolean;

            /// <summary>
            /// Users Name
            /// </summary>
            public string Name;
        }

        /// <summary>
        /// Number Of Online Users
        /// </summary>
        public ushort OnlineUsers;

        /// <summary>
        /// Number Of Listed Users
        /// </summary>
        public ushort ListedUsers;

        /// <summary>
        /// Array Of Listed Users
        /// </summary>
        public User[] Users;
    }
}
