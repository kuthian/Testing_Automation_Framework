using System;
using Jalapeno.Messaging;

namespace Jalapeno.Utils
{
    public class MessageEventArgs : EventArgs
    {
        public MessageHandler.PacketTypeEnum PacketType { get; set; } 
        public MessagePacketData.CommandTypeEnum CommandType { get; set; } 
    }
}
