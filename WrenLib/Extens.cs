using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenLib
{
    public static class Extens
    {
        public static IEnumerable<KeyValuePair<K, V>> ToIEnumerable<K, V>(this Dictionary<K, V> Value)
        {
            List<KeyValuePair<K, V>> IEnumerable = new List<KeyValuePair<K, V>>();
            foreach (KeyValuePair<K, V> KVP in Value)
                IEnumerable.Add(KVP);
            return IEnumerable;
        }

        public static List<System.Text.RegularExpressions.Match> ToList(this System.Text.RegularExpressions.MatchCollection Collection)
        {
            List<System.Text.RegularExpressions.Match> Matches = new List<System.Text.RegularExpressions.Match>();
            foreach (System.Text.RegularExpressions.Match m in Collection)
                Matches.Add(m);
            return Matches;
        }

        public static byte[] Reverse(this byte[] Value)
        {
            byte[] Rev = new byte[Value.LongLength];
            Array.Copy(Value, Rev, Value.LongLength);
            Array.Reverse(Rev);
            return Rev;
        }

        public static ulong Reverse(this ulong Value)
        {
            return BitConverter.ToUInt64(BitConverter.GetBytes(Value).Reverse(), 0);
        }

        public static uint Reverse(this uint Value)
        {
            return BitConverter.ToUInt32(BitConverter.GetBytes(Value).Reverse(), 0);
        }

        public static ushort Reverse(this ushort Value)
        {
            return BitConverter.ToUInt16(BitConverter.GetBytes(Value).Reverse(), 0);
        }

        public static string SliceEnd(this string Value, int Len)
        {
            if (Value.Length <= 0) return Value;
            return Value.Substring(0, Value.Length - Len);
        }

        public static uint NextUInt32(this Random Gen)
        {
            byte[] Buffer = new byte[4];
            Gen.NextBytes(Buffer);
            return (uint)(Buffer[0] << 24) + (uint)(Buffer[1] << 16) + (uint)(Buffer[2] << 8) + (uint)Buffer[3];
        }
    }
}
