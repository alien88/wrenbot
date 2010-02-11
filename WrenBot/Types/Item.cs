using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Item Object (Inherits MapEntity)
    /// </summary>
    public class Item : MapEntity
    {
        /// <summary>
        /// Item Icons Class
        /// </summary>
        public class Icons
        {
            /// <summary>
            /// Default Icon Numbers
            /// </summary>
            public static ushort
                GoldPile = 0x808C,
                SilverPile = 0x808D;
        }

        /// <summary>
        /// Default Item Constructor
        /// </summary>
        public Item() : base(Type.Item) { }

        /// <summary>
        /// Number Of Pickup Tries
        /// </summary>
        public int PickUpTries = 0;
    }
}
