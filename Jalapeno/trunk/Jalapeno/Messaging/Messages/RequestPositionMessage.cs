using System;

namespace Jalapeno.Messaging.Messages
{
    public class RequestPositionMessage : Message
    {
        public RequestPositionMessage() : base()
        {
            MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.REQUEST_POSITION;
            MessagePacketizer.addCommandType();
            MessagePacketizer.addTimeout(60); // Default

            MessagePacket = MessagePacketizer.CompleteMessage();
        }
    }
}
