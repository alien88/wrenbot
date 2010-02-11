using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot
{
    public class string8
    {
        public string8()
        {
            value = "";
        }
        public string8(string value)
        {
            this.value = value;
        }
        public static implicit operator string8(string value)
        {
            return new string8(value);
        }
        public static implicit operator string(string8 value)
        {
            return value.value;
        }
        public string value;
        public int Length { get { return value.Length; } }
    }
    public class string16
    {
        public string16()
        {
            value = "";
        }
        public string16(string value)
        {
            this.value = value;
        }
        public static implicit operator string16(string value)
        {
            return new string16(value);
        }
        public static implicit operator string(string16 value)
        {
            return value.value;
        }
        public string value;
    }
    public class short8
    {
        public short8()
        {
            value = 0;
        }
        public short8(short value)
        {
            this.value = value;
        }
        public static implicit operator short8(short value)
        {
            return new short8(value);
        }
        public static implicit operator short(short8 value)
        {
            return value.value;
        }
        short value;
        public void SetBytes(byte[] Value)
        {
            if (Value[1] >= 155)
                value = (short)(0xFF - Value[1]);
            else value = (short)Value[1];
        }
        public byte[] GetBytes()
        {
            if (value < 0)
                return new byte[] { 0x00, (byte)(0xFF - (-1 * value)) };
            return new byte[] { 0x00, (byte)value };
        }
    }
}
