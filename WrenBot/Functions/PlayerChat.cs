using System;
using System.Collections.Generic;
using System.Linq;
using WrenBot;
using WrenBot.Net.ServerStructs;
using WrenBot.Net.ClientStructs;
using WrenBot.Types;

namespace WrenBot.Functions
{
    public static partial class Functions
    {
        public static void Chat(this BotClient Client, string _Message, AislingEntity Entity)
        {
            Packet ChatPacket = new Packet();
            ChatPacket.Write(new Chat()
            {
                Message = _Message,
                Serial = Entity.Serial,
                Type = 0x00
            }
            );
            byte[] bytes = ChatPacket.Data;
            Client.Socket.SendToServer(bytes, Client.Socket.Serial);            
        }
        public static void Chat(this BotClient Client, string _Message, uint _Serial)
        {
            Packet ChatPacket = new Packet();
            ChatPacket.Write(new Chat()
            {
                Message = _Message,
                Serial = _Serial,
                Type = 0x02
            }
            );
            byte[] bytes = ChatPacket.Data;
            Client.Socket.SendToClient(bytes, Client.Socket.Serial);
        }
    }
}
