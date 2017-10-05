using System;

namespace Jalapeno.Messaging.Messages
{
    //Base class for all messags except multi-config messages.
    public class Message
    {
        public Packetizer MessagePacketizer;
        public byte[] MessagePacket {get; set;} 
        
        public Message()
        {
            MessagePacketizer = new Packetizer();
        }
    }
}
