using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class MapInfo
    {
        public MapInfo()
        {
            Name = "";
        }
        public MapInfo(ushort MapNumber, ushort TileX, ushort TileY, ushort CRC, string Name)
        {
            this.MapNumber = MapNumber;
            this.TileX = TileX;
            this.TileY = TileY;
            this.CRC = CRC;
            this.Name = Name;
        }
        public byte Action { get { return 0x15; } set { } }
        public byte Ordinal { get; set; }
        public ushort MapNumber { get; set; }
        public ushort TileX;
        public ushort TileY;
        public byte TileXLB
        {
            get
            {
                return (byte)(TileX % 256);
            }
            set
            {
                TileX = (ushort)(((byte)(TileX >> 8) << 8) + value);
            }
        }
        public byte TileYLB
        {
            get
            {
                return (byte)(TileX % 256);
            }
            set
            {
                TileY = (ushort)(((byte)(TileY >> 8) << 8) + value);
            }
        }
        public byte TileXHB
        {
            get
            {
                return (byte)(TileX >> 8);
            }
            set
            {
                TileX = (ushort)((byte)(TileX % 256) + (value << 8));
            }
        }
        public byte TileYHB
        {
            get
            {
                return (byte)(TileY >> 8);
            }
            set
            {
                TileX = (ushort)((byte)(TileX % 256) + (value << 8));
            }
        }
        public byte Unknown { get { return 0x00; } set { } }
        public ushort CRC { get; set; }
        public string8 Name { get; set; }
    }
}
