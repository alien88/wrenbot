using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot
{
    public class Packet
    {
        public Packet()
        {
            this.Data = new byte[0];
        }
        public Packet(byte Action)
            : this()
        {
            Data = new byte[1];
            this.Action = Action;
        }
        public Packet(byte Action, byte Ordinal)
            : this()
        {
            this.Action = Action;
            this.Ordinal = Ordinal;
        }
        public Packet(byte[] RawData)
        {
            Data = RawData;
        }
        public bool Encrypt;
        public byte[] Data;
        public byte Action
        {
            get { return Data[0]; }
            set { Data[0] = value; }
        }
        public byte Ordinal
        {
            get { return Data[1]; }
            set { Data[1] = value; }
        }
        public byte this[int Index]
        {
            get
            {
                try
                {
                    return Data[Index];
                }
                catch
                {
                    return 0x00;
                }
            }
            set { Data[Index] = value; }
        }

        public int Length
        {
            get { return Data.Length - 2; }
        }
        private void WriteByte(byte value)
        {
            Array.Resize<byte>(ref this.Data, this.Data.Length + 1);
            this.Data[this.Data.Length - 1] = value;
        }
        private void WriteShort(short value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }
        private void WriteUShort(ushort value)
        {
            WriteBytes(new byte[] { (byte)((value >> 8) % 256), (byte)(value % 256) });
        }
        private void WriteInt(int value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }
        private void WriteUInt(uint value)
        {
            WriteBytes(new byte[] { (byte)((value >> 24) % 256), (byte)((value >> 16) % 256), (byte)((value >> 8) % 256), (byte)(value % 256) });
        }
        public void WriteBytes(byte[] value)
        {
            Array.Resize<byte>(ref this.Data, this.Data.Length + value.Length);
            Array.Copy(value, 0, this.Data, this.Data.Length - value.Length, value.Length);
        }
        private static int ConvertBitToInt(bool value)
        {
            if (value) return 1;
            else return 0;
        }
        private bool[] ReadBitmask(int Index)
        {
            byte Val = this[Index];
            return new bool[]{
                Val%2==1,
                (Val>>1)%2==1,
                (Val>>2)%2==1,
                (Val>>3)%2==1,
                (Val>>4)%2==1,
                (Val>>5)%2==1,
                (Val>>6)%2==1,
                (Val>>7)%2==1,
            };
        }
        private void WriteBitmask(bool[] value)
        {
            WriteByte(
                (byte)
                (
                    (ConvertBitToInt(value[0]) << 7) +
                    (ConvertBitToInt(value[1]) << 6) +
                    (ConvertBitToInt(value[2]) << 5) +
                    (ConvertBitToInt(value[3]) << 4) +
                    (ConvertBitToInt(value[4]) << 3) +
                    (ConvertBitToInt(value[5]) << 2) +
                    (ConvertBitToInt(value[6]) << 1) +
                    ConvertBitToInt(value[7])
                )
            );
        }
        private enum StringType
        {
            String,
            String8,
            String16
        }
        private void WriteString(string Value, StringType Type)
        {
            switch (Type)
            {
                case StringType.String:
                    WriteBytes(Encoding.ASCII.GetBytes(Value));
                    break;
                case StringType.String8:
                    WriteByte((byte)Value.Length);
                    WriteBytes(Encoding.ASCII.GetBytes(Value));
                    break;
                case StringType.String16:
                    WriteUShort((ushort)Value.Length);
                    WriteBytes(Encoding.ASCII.GetBytes(Value));
                    break;
            }
        }
        private ushort ReadUShort(int Position)
        {
            return (ushort)((this[Position] << 8) + this[Position + 1]);
        }
        private uint ReadUInt(int Position)
        {
            return (uint)((this[Position] << 24) + (this[Position + 1] << 16) + (this[Position + 2] << 8) + this[Position + 3]);
        }
        private byte[] ReadBytes(int Position, int Length)
        {
            byte[] Buffer = new byte[Length];
            Array.Copy(Data, Position, Buffer, 0, Length);
            return Buffer;
        }
        private object Read(Type Type, ref int Index)
        {
            object Obj = Activator.CreateInstance(Type);
            foreach (System.Reflection.PropertyInfo Field in Type.GetProperties())
            {
                if (Field.Name == "SaveOrdinal")
                    continue;
                if (Field.PropertyType.IsEnum)
                {
                    Field.SetValue(Obj, (object)this[Index], null);
                    Index++;
                    continue;
                }
                switch (Field.PropertyType.Name)
                {
                    case "short8":
                        {
                            byte[] Value = ReadBytes(Index, 2);
                            Index += 2;
                            short8 val = new short8();
                            val.SetBytes(Value);
                            Field.SetValue(Obj, val, null);
                        } break;
                    case "Boolean":
                        Field.SetValue(Obj, Convert.ToBoolean(this[Index]), null);
                        Index++;
                        break;
                    case "Boolean[]":
                        Field.SetValue(Obj, ReadBitmask(Index), null);
                        Index++;
                        break;
                    case "Byte":
                        Field.SetValue(Obj, this[Index], null);
                        Index++;
                        break;
                    case "UInt16":
                        Field.SetValue(Obj, ReadUShort(Index), null);
                        Index += 2;
                        break;
                    case "UInt32":
                        Field.SetValue(Obj, ReadUInt(Index), null);
                        Index += 4;
                        break;
                    case "String":
                        Field.SetValue(Obj, Encoding.ASCII.GetString(ReadBytes(Index, this.Length)), null);
                        Index += this[Index] + 1;
                        break;
                    case "string8":
                        Field.SetValue(Obj, (string8)Encoding.ASCII.GetString(ReadBytes(Index + 1, (int)this[Index])), null);
                        Index += this[Index] + 1;
                        break;
                    case "string16":
                        Field.SetValue(Obj, (string16)Encoding.ASCII.GetString(ReadBytes(Index + 2, (int)(((this[Index]) << 8) + this[Index + 1]))), null);
                        Index += this[Index] + 1;
                        break;
                    case "Byte[]":
                        if (Field.GetValue(Obj, null) != null)
                        {
                            Field.SetValue(Obj, ReadBytes(Index, ((byte[])Field.GetValue(Obj, null)).Length), null);
                            Index += ((byte[])Field.GetValue(Obj, null)).Length;
                        }
                        break;
                    case "IPEndPoint":
                        Field.SetValue(
                            Obj,
                            new System.Net.IPEndPoint(
                                new System.Net.IPAddress(
                                    new byte[] 
                                    { 
                                        this[Index+3],
                                        this[Index+2],
                                        this[Index+1],
                                        this[Index]
                                    }
                                ), ReadUShort(Index + 4)),
                            null
                        );
                        Index += 6;
                        break;                   
                    default:
                        {
                            Field.SetValue(
                                Obj,
                                Read(Field.PropertyType, ref Index),
                                null
                            );
                        } break;
                }
            }
            return Obj;
        }
        public T Read<T>(int Index) where T : new()
        {
            T Obj = new T();
            foreach (System.Reflection.PropertyInfo Field in typeof(T).GetProperties())
            {
                if (Field.Name == "SaveOrdinal")
                    continue;
                if (Field.PropertyType.IsEnum)
                {
                    Field.SetValue(Obj, (object)this[Index], null);
                    Index++;
                    continue;
                }
                switch (Field.PropertyType.Name)
                {
                    case "short8":
                        {
                            byte[] Value = ReadBytes(Index, 2);
                            Index += 2;
                            short8 val = new short8();
                            val.SetBytes(Value);
                            Field.SetValue(Obj, val, null);
                        } break;
                    case "Boolean":
                        Field.SetValue(Obj, Convert.ToBoolean(this[Index]), null);
                        Index++;
                        break;
                    case "Boolean[]":
                        Field.SetValue(Obj, ReadBitmask(Index), null);
                        Index++;
                        break;
                    case "Byte":
                        Field.SetValue(Obj, this[Index], null);
                        Index++;
                        break;
                    case "Int16":
                        Field.SetValue(Obj, BitConverter.ToInt16(new byte[] { this[Index + 1], this[Index] }, 0), null);
                        Index += 2;
                        break;
                    case "UInt16":
                        Field.SetValue(Obj, ReadUShort(Index), null);
                        Index += 2;
                        break;
                    case "UInt32":
                        Field.SetValue(Obj, ReadUInt(Index), null);
                        Index += 4;
                        break;
                    case "String":
                        Field.SetValue(Obj, Encoding.ASCII.GetString(ReadBytes(Index, this.Length)), null);
                        Index += this[Index] + 1;
                        break;
                    case "string8":
                        Field.SetValue(Obj, (string8)Encoding.ASCII.GetString(ReadBytes(Index + 1, (int)this[Index])), null);
                        Index += this[Index] + 1;
                        break;
                    case "string16":
                        Field.SetValue(Obj, (string16)Encoding.ASCII.GetString(ReadBytes(Index + 2, (int)(((this[Index]) << 8) + this[Index + 1]))), null);
                        Index += this[Index] + 1;
                        break;
                    case "Byte[]":
                        if (Field.GetValue(Obj, null) != null)
                        {
                            Field.SetValue(Obj, ReadBytes(Index, ((byte[])Field.GetValue(Obj, null)).Length), null);
                            Index += ((byte[])Field.GetValue(Obj, null)).Length;
                        }
                        break;
                    case "IPEndPoint":
                        Field.SetValue(
                            Obj,
                            new System.Net.IPEndPoint(
                                new System.Net.IPAddress(
                                    new byte[] 
                                    { 
                                        this[Index+3],
                                        this[Index+2],
                                        this[Index+1],
                                        this[Index]
                                    }
                                ), ReadUShort(Index + 4)),
                            null
                        );
                        Index += 6;
                        break;                
                    default:                       
                         break;
                }
            }
            return Obj;
        }
        public bool Write(object Object)
        {
            bool SaveOrdinal = false;
            foreach (System.Reflection.PropertyInfo Field in Object.GetType().GetProperties())
            {

                if (Field.Name == "SaveOrdinal")
                {
                    SaveOrdinal = true;
                    continue;
                }
                object FieldObject = Field.GetValue(Object, null);
                switch (FieldObject.GetType().Name)
                {
                    case "short8": WriteBytes(((short8)FieldObject).GetBytes()); break;
                    case "Boolean": WriteByte(Convert.ToByte(FieldObject)); break;
                    case "Boolean[]": WriteBitmask((bool[])FieldObject); break;
                    case "Byte": WriteByte((byte)FieldObject); break;
                    case "Int16": WriteShort((Int16)FieldObject); break;
                    case "UInt16": WriteUShort((ushort)FieldObject); break;
                    case "Int32": WriteInt((Int32)FieldObject); break;
                    case "UInt32": WriteUInt((uint)FieldObject); break;
                    case "String": WriteBytes(Encoding.ASCII.GetBytes((string)FieldObject)); break;
                    case "string8": WriteString(((string8)FieldObject).value, StringType.String8); break;
                    case "string16": WriteString(((string16)FieldObject).value, StringType.String16); break;
                    case "Byte[]": WriteBytes((byte[])FieldObject); break;
                    case "IPEndPoint":
                        byte[] AddressBytes = ((System.Net.IPEndPoint)FieldObject).Address.GetAddressBytes();
                        WriteBytes(new byte[] { AddressBytes[3], AddressBytes[2], AddressBytes[1], AddressBytes[0] });
                        WriteUShort((ushort)((System.Net.IPEndPoint)FieldObject).Port);
                        break;                   
                    default:
                        if (FieldObject.GetType().IsArray)
                        {
                            foreach (object OBJ in FieldObject as Array)
                                Write(OBJ);
                            break;
                        }
                        else if (FieldObject.GetType().IsEnum)
                        {
                            WriteByte(Convert.ToByte(FieldObject));
                            break;
                        }
                        switch (FieldObject.GetType().FullName)
                        {
                            default:
                                Write(FieldObject);
                                break;
                        } break;
                }
            }
            return SaveOrdinal;
        }
        public byte[] ToArray()
        {
            byte[] Buffer = new byte[Data.Length + 3];
            Buffer[0] = 0xAA;
            Buffer[1] = (byte)((Data.Length >> 8) % 256);
            Buffer[2] = (byte)(Data.Length % 256);
            Array.Copy(Data, 0, Buffer, 3, Data.Length);
            return Buffer;
        }

        #region Packet String
        public string GetPacketString(byte[] packet)
        {
            try
            {
                System.Text.StringBuilder packetstring = new System.Text.StringBuilder(packet.Length * 3);
                for (int i = 0; i < packet.Length; i++)
                    packetstring.AppendFormat("{0:X02} ", packet[i]);

                return packetstring.ToString();
            }
            catch { }
            return "";
        }
        public override string ToString()
        {
            return GetPacketString(Data);
        }
        #endregion
    }
}