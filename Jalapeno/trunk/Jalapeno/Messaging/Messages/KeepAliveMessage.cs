using System;

namespace Jalapeno.Messaging.Messages
{
    public class KeepAliveMessage : Message
    {
        public KeepAliveMessage(): base()
        {
            MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.KeepAlive;
            MessagePacket = MessagePacketizer.CompleteMessage();
        }
    }
}
