using System;

namespace Jalapeno.Messaging.Messages
{
    class DiagnosticStatusMessage : Message
    {
        public DiagnosticStatusMessage(): base()
        {
            MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.DIAGNOSTIC_STATUS_REQUEST;
            MessagePacketizer.addCommandType();

            MessagePacket = MessagePacketizer.CompleteMessage();
        }
    }
}
