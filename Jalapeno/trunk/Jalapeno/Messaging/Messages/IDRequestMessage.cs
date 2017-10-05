using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jalapeno.Messaging.Messages
{
    class IDRequestMessage : Message
    {
        public IDRequestMessage(): base()
        {
            MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.IDENTIFICATION_REQUEST;
            MessagePacketizer.addCommandType();

            MessagePacket = MessagePacketizer.CompleteMessage();
        }
    }
}
