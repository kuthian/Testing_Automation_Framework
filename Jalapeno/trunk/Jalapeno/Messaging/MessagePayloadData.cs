using System;
using System.Collections.Generic;
using Jalapeno.Utils;

namespace Jalapeno.Messaging
{
    public class MessagePayloadData
    {
        public byte[] DataArray;

        public MessagePayloadData(byte[] ReceivedMess)
        {
            UInt16 DataLength = BitConverter.ToUInt16(ReceivedMess, 7) ;
            int PayloadLength = DataLength - 2;

            //---------------------------------------------------------------------------------------------
            // PACKET_DATA Summary (APP_DATA)
            //---------------------------------------------------------------------------------------------
            // byte[9]  = CMD_TYPE
            // byte[10] = CMD_SUB_TYPE
            // byte[11 - (8+x)]  = PAYLOAD (where x is DATA LENGTH)
            // byte[(9+x) - (10+x)]  = EOF [ 0xFF, 0xCC ]
            //---------------------------------------------------------------------------------------------
            // ...
            //---------------------------------------------------------------------------------------------

            //Payload array size is defined
            DataArray = new byte[PayloadLength];
            //Payload data loaded into array
            for (int i = 0; i < PayloadLength; i++)
            {
                DataArray[i] = ReceivedMess[i + 11];
            }
        }

        //To be used for configurations for now
        public MessagePayloadData(Queue<byte[]> ReceivedMessQueue)
        {
            DataArray = Tools.CreateByteArrayFromQueue(ReceivedMessQueue);
        }
    }
}
