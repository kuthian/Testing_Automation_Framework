using System;

namespace Jalapeno.Messaging.Messages
{
    class EnableRealTimeTrackingMessage : Message
    {
        public EnableRealTimeTrackingMessage() : base()
        {
            MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.ENABLE_REALTIME_TRACKING_REQUEST;
            MessagePacketizer.addCommandType();
            MessagePacketizer.addTimeout(120); // Default

            MessagePacket = MessagePacketizer.CompleteMessage();
        }
    }

    class DisableRealTimeTrackingMessage : Message
    {
        public DisableRealTimeTrackingMessage(): base()
        {
            MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.DISABLE_REALTIME_TRACKING_REQUEST;
            MessagePacketizer.addCommandType();

            MessagePacket = MessagePacketizer.CompleteMessage();
        }
    }
}
