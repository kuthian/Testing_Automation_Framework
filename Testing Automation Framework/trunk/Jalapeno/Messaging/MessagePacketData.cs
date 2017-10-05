using System;
using System.Collections.Generic;
using Jalapeno.Utils;


namespace Jalapeno.Messaging
{
    public class MessagePacketData
    {
        public MessagePayloadData PayloadData;
        public enum CommandTypeEnum 
        { 
            NONE, 
            WRITE_CONFIGURATION, WRITE_CONFIGURATION_REPLY, 
            READ_CONFIGURATION, READ_CONFIGURATION_REPLY, 
            REQUEST_POSITION, REQUEST_POSITION_REPLY, REQUEST_POSITION_FIX, 
            ENABLE_REALTIME_TRACKING_REQUEST, ENABLE_REALTIME_TRACKING_REPLY, REALTIME_TRACKING_FIX,
            DISABLE_REALTIME_TRACKING_REQUEST, DISABLE_REALTIME_TRACKING_REPLY,
            DIAGNOSTIC_STATUS_REQUEST, DIAGNOSTIC_STATUS_REPLY,
            IDENTIFICATION_REQUEST, IDENTIFICATION_REPLY,
            PERIODIC_TRANSMISSION,
            CLEAR_MEMORY_LOG_REQUEST, CLEAR_MEMORY_LOG_REPLY, CLEAR_MEMORY_PROGRESS, CLEAR_MEMORY_COMPLETE,
            START_DOWNLOAD_REQUEST, START_DOWNLOAD_REPLY, STOP_DOWNLOAD_REQUEST, STOP_DOWNLOAD_REPLY, DOWNLOAD_PACKET,
            OUTPUT_CONTROL_REQUEST, OUTPUT_CONTROL_REPLY,
            SYSTEM_DATA
        }
        public CommandTypeEnum CommandType;
        public UInt32 ConfigCRC;
        public byte ResultCode;

        //---------------------------------------------------------------------------------------------
        // PACKET_DATA Summary (APP_DATA example)
        //---------------------------------------------------------------------------------------------
        // byte[9]  = CMD_TYPE
        // byte[10] = CMD_SUB_TYPE
        // byte[11 - (9+x)]  = PAYLOAD (where x is DATA LENGTH) (x will never be 0 for app data)
        // byte[(10+x) - (11+x)]  = EOF [ 0xFF, 0xCC ]
        //---------------------------------------------------------------------------------------------
        // ...
        //---------------------------------------------------------------------------------------------
        public MessagePacketData()
        {
            CommandType = CommandTypeEnum.NONE;
        }

        public MessagePacketData(byte[] ReceivedMess)
        {
            readCommandType(ReceivedMess);
            
            if (CommandType == CommandTypeEnum.READ_CONFIGURATION_REPLY)               
            {
                Queue<byte[]> ConfigQueue = new Queue<byte[]>();
                UInt16 ConfigCRC = 0;

                int ArrayIndexStart = 0;
                int ArrayIndexEnd = -1;
                
                //Populate DATA_PAYLOAD array if there is more than just a command field
                for (int i = 0; i < ReceivedMess.Length; i++)
                {
                    //iterating through the byte array until a EOF is detected
                    if ((ReceivedMess[i] == 0xFF) && (ReceivedMess[i + 1] == 0xCC))
                    {
                        ArrayIndexStart = ArrayIndexEnd + 1;
                        ArrayIndexEnd = i + 1;

                        //When the EOF is detected, check that packet's CommandType and make Packet 
                        if (ReceivedMess[ArrayIndexStart + 6] == 0x04 && getCommandType(ReceivedMess, ArrayIndexStart) == CommandTypeEnum.READ_CONFIGURATION_REPLY)
                        {
                            // CRC should be calculated here and the entire message should be discarded if CRC check fails. This is not currently done. 
                            if (ConfigCRC == 0)
                            {                              
                                ConfigCRC = BitConverter.ToUInt16(ReceivedMess, 11);                                
                                UtilCRC.ConfigCRC = ConfigCRC;
                                ConfigQueue.Enqueue(Tools.ArrayFromArray(ReceivedMess, ArrayIndexStart + 17, ArrayIndexEnd - 4));
                            }
                            else
                            {
                                ConfigQueue.Enqueue(Tools.ArrayFromArray(ReceivedMess, ArrayIndexStart + 17, ArrayIndexEnd - 4));
                            }              
                        }
                    }
                }

                PayloadData = new MessagePayloadData(ConfigQueue);
            }
            else if (CommandType == CommandTypeEnum.WRITE_CONFIGURATION_REPLY)
            {
                ResultCode = ReceivedMess[11]; 
                if (BitConverter.ToUInt16(ReceivedMess, 7) > 2)
                {
                    PayloadData = new MessagePayloadData(ReceivedMess);
                }
            }
            else
            {
                if (BitConverter.ToUInt16(ReceivedMess, 7) > 2)
                {
                    PayloadData = new MessagePayloadData(ReceivedMess);
                }
            }                
        }

        private void readCommandType(byte[] ReceivedMsg)
        {
            
            CommandType = getCommandType(ReceivedMsg, 0);
        }

        private CommandTypeEnum getCommandType(byte[] ReceivedMess, int Index)
        {
            int i = Index;
            CommandTypeEnum CommType = CommandTypeEnum.NONE;

            switch (ReceivedMess[9 + i])
            {
                case (0x01):
                    {
                        switch (ReceivedMess[10 + i])
                        {
                            case (0xFA):
                                {
                                    CommType = CommandTypeEnum.PERIODIC_TRANSMISSION;
                                }
                                break;
                            case (0xFB):
                                {
                                    CommType = CommandTypeEnum.REQUEST_POSITION_FIX;
                                }
                                break;
                            case (0xFC):
                                {
                                    CommType = CommandTypeEnum.REALTIME_TRACKING_FIX;
                                }
                                break;
                            case (0xFE):
                                {
                                    CommType = CommandTypeEnum.CLEAR_MEMORY_COMPLETE;
                                }
                                break;
                            case (0xFF):
                                {
                                    CommType = CommandTypeEnum.DOWNLOAD_PACKET;
                                }
                                break;
                            case (0xF7):
                                {
                                    CommType = CommandTypeEnum.CLEAR_MEMORY_PROGRESS;
                                }
                                break;
                            default:
                                {
                                    CommType = CommandTypeEnum.NONE;
                                }
                                break;
                        }
                    }
                    break;
                case (0x03):
                    {
                        switch (ReceivedMess[10 + i])
                        {
                            case (0x81):
                                {
                                    CommType = CommandTypeEnum.IDENTIFICATION_REPLY;
                                }
                                break;
                            case (0x82):
                                {
                                    CommType = CommandTypeEnum.DIAGNOSTIC_STATUS_REPLY;
                                }
                                break;
                            default:
                                {
                                    CommType = CommandTypeEnum.NONE;
                                }
                                break;
                        }
                    }
                    break;
                case (0x05):
                    {
                        switch (ReceivedMess[10 + i])
                        {
                            case (0x81):
                                {
                                    CommType = CommandTypeEnum.READ_CONFIGURATION_REPLY;
                                }
                                break;
                            case (0x82):
                                {
                                    CommType = CommandTypeEnum.WRITE_CONFIGURATION_REPLY;
                                }
                                break;
                            default:
                                {
                                    CommType = CommandTypeEnum.NONE;
                                }
                                break;
                        }
                    }
                    break;
                case (0x09):
                    {
                        switch (ReceivedMess[10 + i])
                        {
                            case (0xA0):
                                {
                                    CommType = CommandTypeEnum.STOP_DOWNLOAD_REPLY;
                                }
                                break;
                            case (0xA1):
                                {
                                    CommType = CommandTypeEnum.START_DOWNLOAD_REPLY;
                                }
                                break;
                            case (0xB0):
                                {
                                    CommType = CommandTypeEnum.CLEAR_MEMORY_LOG_REPLY;
                                }
                                break;
                            default:
                                {
                                    CommType = CommandTypeEnum.NONE;
                                }
                                break;
                        }
                    }
                    break;
                case (0x0B):
                    {
                        switch (ReceivedMess[10 + i])
                        {
                            case (0x81):
                                {
                                    CommType = CommandTypeEnum.ENABLE_REALTIME_TRACKING_REPLY;
                                }
                                break;
                            case (0x82):
                                {
                                    CommType = CommandTypeEnum.DISABLE_REALTIME_TRACKING_REPLY;
                                }
                                break;
                            case (0x83):
                                {
                                    CommType = CommandTypeEnum.REQUEST_POSITION_REPLY;
                                }
                                break;
                            case (0x85):
                                {
                                    CommType = CommandTypeEnum.OUTPUT_CONTROL_REPLY;
                                }
                                break;
                            default:
                                {
                                    CommType = CommandTypeEnum.NONE;
                                }
                                break;
                        }
                    }
                    break;
                case (0x0F):
                    {
                        switch (ReceivedMess[10 + i])
                        {
                            case (0x81):
                                {
                                    CommType = CommandTypeEnum.SYSTEM_DATA;
                                }
                                break;
                        }
                    }
                    break;
                default:
                    {
                        CommType = CommandTypeEnum.NONE;
                    }
                    break;
            }
            return CommType;
            //if (ReceivedMess[9+i] == 0x05 && ReceivedMess[10+i] == 0x02)
            //{
            //    CommType = CommandTypeEnum.WRITE_CONFIGURATION;
            //}
            //else if (ReceivedMess[9+i] == 0x05 && ReceivedMess[10+i] == 0x01)
            //{
            //    CommType = CommandTypeEnum.READ_CONFIGURATION;
            //}
            //else if (ReceivedMess[9+i] == 0x05 && ReceivedMess[10+i] == 0x82)
            //{
            //    CommType = CommandTypeEnum.WRITE_CONFIGURATION_REPLY;
            //}
            //else if (ReceivedMess[9+i] == 0x05 && ReceivedMess[10+i] == 0x81)
            //{
            //    CommType = CommandTypeEnum.READ_CONFIGURATION_REPLY;
            //}
            //else if (ReceivedMess[9+i] == 0x0B && ReceivedMess[10+i] == 0x03)
            //{
            //    CommType = CommandTypeEnum.REQUEST_POSITION_REPLY;
            //}
            //else if (ReceivedMess[9+i] == 0x0B && ReceivedMess[10+i] == 0x83)
            //{
            //    CommType = CommandTypeEnum.REQUEST_POSITION_REPLY;
            //}
            //else if (ReceivedMess[9+i] == 0x01 && ReceivedMess[10+i] == 0xFB)
            //{
            //    CommType = CommandTypeEnum.REQUEST_POSITION_FIX;
            //}
            //else if (ReceivedMess[9 + i] == 0x0B && ReceivedMess[10 + i] == 0x81)
            //{
            //    CommType = CommandTypeEnum.ENABLE_REALTIME_TRACKING_REPLY;
            //}
            //else if (ReceivedMess[9 + i] == 0x0B && ReceivedMess[10 + i] == 0x82)
            //{
            //    CommType = CommandTypeEnum.DISABLE_REALTIME_TRACKING_REPLY;
            //}
            //else if (ReceivedMess[9 + i] == 0x01 && ReceivedMess[10 + i] == 0xFC)
            //{
            //    CommType = CommandTypeEnum.REALTIME_TRACKING_FIX;
            //}
            //else if (ReceivedMess[9 + i] == 0x03 && ReceivedMess[10 + i] == 0x82)
            //{
            //    CommType = CommandTypeEnum.DIAGNOSTIC_STATUS_REPLY;
            //}
            //else if (ReceivedMess[9 + i] == 0x03 && ReceivedMess[10 + i] == 0x81)
            //{
            //    CommType = CommandTypeEnum.IDENTIFICATION_REPLY;
            //}
            //else
            //{
            //    CommType = CommandTypeEnum.NONE;
            //}
            
        }
    }
}

