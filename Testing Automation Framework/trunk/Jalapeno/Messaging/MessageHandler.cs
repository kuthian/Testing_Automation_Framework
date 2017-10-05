using System;
using System.Diagnostics;
using ZylSerialPort;
using Jalapeno.Utils;

namespace Jalapeno.Messaging
{
    // Reads from the buffer and handles the data in order to create PacketData class with the correct payload. Contains High-Level info like Packet Length, Packet Type, and CRC.
    public class MessageHandler
    {
        public MessagePacketData PacketData { get; set; }
        public enum PacketTypeEnum { None = 0, AppData = 4, KeepAlive = 5, Confirmation = 6, Notification = 9 };
        public PacketTypeEnum PacketType { get; set; }
        public UInt16 ModelNum { get; set; }
        public UInt16 SerialNum { get; set; }      
        private int DataLength; // PACKET_DATA LENGTH 
        private int PacketLength; // PACKET_DATA LENGTH + 13
        private UInt16 CRC;
        public bool CompleteMessage { get; set; }

        //---------------------------------------------------------------------------------------------
        // Packet Summary
        //---------------------------------------------------------------------------------------------
        // byte[0 - 1]  = SOF [ 0xAA, 0x55 ]
        // byte[2 - 3]  = MODEL_NUM //little endian
        // byte[4 - 5]  = SERIAL_NUM //little endian
        // byte[6]  = PACKET_TYPE 
        // byte[7 - 8]  = DATA LENGTH  (x) //little endian
        // byte[9 - (8+x)]  = PACKET_DATA (length defined by packet data length) 
        // byte[(9+x) - (10+x)]  = CRC 
        // byte[(11+x) - (12+x)]  = EOF [ 0xFF, 0xCC ]
        //---------------------------------------------------------------------------------------------
        // ...
        //---------------------------------------------------------------------------------------------

        public void DePacketizeMessage(SerialLink CurrentConnection, byte[] ReceivedMessage)
        {
            try
            {
                //Check this is the start of a valid message 
                if (checkSOF(ReceivedMessage))
                {
                    if (!CheckIfCompleteMessage(ReceivedMessage))
                    {
                        return;
                    }

                    //Populate header variables
                    readHeader(ReceivedMessage);

                    //If data has been received, create the PacketData Array
                    if(DataLength > 0)
                    {          
                        PacketData = new MessagePacketData(ReceivedMessage);
                    }
                    else
                    {
                        PacketData = new MessagePacketData();
                    }

                    //Show the Received Message byte array for debug purposes
                    Debug.WriteLine("Received: " + BitConverter.ToString(ReceivedMessage) + "\n");

                    //Assessing packet types
                    AssessPacketType();

                    //Raise MsgType event
                    CurrentConnection.MsgListener.OnMsgTypeReceived(PacketType, PacketData.CommandType);

                    //A valid message has been read
                    CompleteMessage = true;
                    
                }
                else
                {
                    Debug.WriteLine("Message Discarded: Incorrect SOF bytes :" + BitConverter.ToString(ReceivedMessage) + "\n");
                    CompleteMessage = true; // If incorrect SOF bytes, CompleteMessage is kept true to ignore this messsage entirely

                }
            }
            catch
            {
                if (CompleteMessage)
                {
                    Debug.WriteLine("Message Discarded: Error Reading Message\n");
                }
                else
                {
                    Debug.WriteLine("Incomplete Message\n");
                }
                
                return;
            }
        }

        public void DePacketizeIncompleteMessage(SerialLink CurrentConnection, byte[] ReceivedMessage)
        {
            try
            {
                CompleteMessage = false;

                //ensure that this is a continuatation packet, and that it is the last packet
                if (!checkSOF(ReceivedMessage) && checkEOF(ReceivedMessage))
                {
                    //create an array from buffer queue
                    byte[] NewReceivedMessage = Tools.CreateByteArrayFromQueue(CurrentConnection.IncompleteReceivedData);

                    //Confirm that the buffer queue message is complete
                    CheckIfCompleteMessage(NewReceivedMessage);

                    //Populate header variables
                    readHeader(NewReceivedMessage);

                    //If data has been received, create the PacketData Array
                    if (DataLength > 0)
                    {
                        PacketData = new MessagePacketData(NewReceivedMessage);
                    }
                    else
                    {
                        PacketData = new MessagePacketData(); ;
                    }

                    //Show the Received Message byte array for debug purposes
                    Debug.WriteLine("Received: " + BitConverter.ToString(NewReceivedMessage) + "\n");


                    //Check if this is a Configuration message first( and update configuration if it is) before checking other packet types. 
                    //This is added because AssessPacketType does not have access to CurrentConnection.
                    if (PacketData.CommandType == MessagePacketData.CommandTypeEnum.READ_CONFIGURATION_REPLY)
                    {
                        CurrentConnection.CurrentConfiguration.UpdateFullConfiguration(PacketData.PayloadData.DataArray);
                    }
                    else
                    {                      
                        //Assessing packet types
                        AssessPacketType();
                    }

                    //Raise MsgType event
                    CurrentConnection.MsgListener.OnMsgTypeReceived(PacketType, PacketData.CommandType);

                    //A valid message has been read
                    CompleteMessage = true;
                }
                else
                {
                    //Console.WriteLine("Message is still incomplete.\n");
                }
            }
            catch
            {
                Debug.WriteLine("Message Discarded: Error Reading Message\n");
                CompleteMessage = true;
                return;
            }
        }

        //Populate header variables
        private void readHeader(byte[] ReceivedMess)
        {
            readModelNum(ReceivedMess);
            readSerialNum(ReceivedMess);
            readPacketType(ReceivedMess);
            readPacketDataLength(ReceivedMess);
            checkCRC(ReceivedMess);
        }

        public bool CheckIfCompleteMessage(byte[] ReceivedMess)
        {
            CompleteMessage = (checkSOF(ReceivedMess) && checkEOF(ReceivedMess));
            return CompleteMessage;
        }

        //Start Of Frame (SOF) Indicator should be [ 0xAA, 0x55 ]
        private bool checkSOF(byte[] ReceivedMess)
        {
            return ((ReceivedMess[0] == 0xAA) && (ReceivedMess[1] == 0x55));
        }

        //Store ModelNum
        public void readModelNum(byte[] ReceivedMess)
        {
            ModelNum = BitConverter.ToUInt16(ReceivedMess, 2); //little endian
        }

        //Store SerialNum
        public void readSerialNum(byte[] ReceivedMess)
        {
            SerialNum = BitConverter.ToUInt16(ReceivedMess, 4); ; //little endian
        }
        //Store PacketType;
        private void readPacketType(byte[] ReceivedMess)
        {
            PacketType = (PacketTypeEnum) ReceivedMess[6];
        }

        //Store DataLength and PacketLength;
        private void readPacketDataLength(byte[] ReceivedMess)
        {
            DataLength = BitConverter.ToUInt16(ReceivedMess, 7); //little endian
            PacketLength = DataLength + 13;
        }

        //Calculate CRC in the packet and check that it is equal to 
        private bool checkCRC(byte [] ReceivedMess)
        {
            UInt16 CRCvalue = (UInt16) UtilCRC.calculateCRC(ReceivedMess, 2, DataLength + 7); 
            CRC = BitConverter.ToUInt16(ReceivedMess, DataLength + 9); 

            return (CRC == CRCvalue);
        }
        
        //End Of Frame Indicator should be [ 0xFF, 0xCC ]
        private bool checkEOF(byte [] ReceivedMess)
        {
            return ((ReceivedMess[ReceivedMess.Length-2] == 0xFF) && (ReceivedMess[ReceivedMess.Length-1] == 0xCC));
        }

        private void AssessPacketType()
        {
            switch (PacketType)
            {
                case (PacketTypeEnum.AppData): //APP Data
                    {
                        switch (PacketData.CommandType)
                        {
                            case (MessagePacketData.CommandTypeEnum.READ_CONFIGURATION_REPLY): //APP Data
                                {
                                    Debug.WriteLine("App Data: - Read Configuration Reply -");
                                }
                                break;
                            case (MessagePacketData.CommandTypeEnum.WRITE_CONFIGURATION_REPLY): //APP Data
                                {
                                    Console.WriteLine("App Data: - Write Configuration Reply - Result Code: " + PacketData.ResultCode + "\n");
                                    Debug.WriteLine("App Data: - Write Configuration Reply - Result Code: " + PacketData.ResultCode + "\n");
                                }
                                break;
                            case (MessagePacketData.CommandTypeEnum.REQUEST_POSITION_REPLY): //APP Data
                                {
                                    Console.WriteLine("App Data - Request Position Reply - Result Code: " + PacketData.ResultCode + "\n");
                                    Debug.WriteLine("App Data - Request Position Reply - Result Code: " + PacketData.ResultCode + "\n");
                                }
                                break;
                            case (MessagePacketData.CommandTypeEnum.ENABLE_REALTIME_TRACKING_REPLY): //APP Data
                                {
                                    Console.WriteLine("App Data - Enable RTT Reply - Result Code: " + PacketData.ResultCode + "\n");
                                    Debug.WriteLine("App Data - Enable RTT Reply - Result Code: " + PacketData.ResultCode + "\n");
                                }
                                break;
                            case (MessagePacketData.CommandTypeEnum.DISABLE_REALTIME_TRACKING_REPLY): //APP Data
                                {
                                    Console.WriteLine("App Data - Disable RTT Reply - Result Code: " + PacketData.ResultCode + "\n");
                                    Debug.WriteLine("App Data - Disable RTT Reply - Result Code: " + PacketData.ResultCode + "\n");
                                }
                                break;
                            case (MessagePacketData.CommandTypeEnum.REQUEST_POSITION_FIX): //APP Data
                                {
                                    Console.WriteLine("App Data - GPS FIX - Position Report -This data is not currently handled\n");
                                    Debug.WriteLine("App Data - GPS FIX - Position Report - This data is not currently handled\n");
                                }
                                break;
                            case (MessagePacketData.CommandTypeEnum.REALTIME_TRACKING_FIX): //APP Data
                                {
                                    Console.WriteLine("App Data - GPS FIX - Position Report -This data is not currently handled\n");
                                    Debug.WriteLine("App Data - GPS FIX - Position Report - This data is not currently handled\n");
                                }
                                break;
                            case (MessagePacketData.CommandTypeEnum.DIAGNOSTIC_STATUS_REPLY): //APP Data
                                {
                                    Console.WriteLine("App Data - Diagnostics - This data is not currently handled\n");
                                    Debug.WriteLine("App Data - Diagnostics - This data is not currently handled\n");
                                }
                                break;
                            case (MessagePacketData.CommandTypeEnum.IDENTIFICATION_REPLY): //APP Data
                                {
                                    Console.WriteLine("App Data - Identification - This data is not currently handled\n");
                                    Debug.WriteLine("App Data - Identification - This data is not currently handled\n");
                                }
                                break;
                            default:
                                {
                                    Console.WriteLine(PacketData.CommandType + ": " + SerialPort.ASCIIByteArrayToString(PacketData.PayloadData.DataArray));
                                    Debug.WriteLine(PacketData.CommandType + ": " + SerialPort.ASCIIByteArrayToString(PacketData.PayloadData.DataArray));
                                }
                                break;
                        }
                    }
                    break;
                case (PacketTypeEnum.Confirmation): //Confirmation Message
                    {
                        Debug.WriteLine("Confirmation message Received\n");
                    }
                    break;

                case (PacketTypeEnum.Notification): //Notification Message
                    {
                        Debug.WriteLine("Notification message Received\n");
                    }
                    break;
                default:
                    {
                        Debug.WriteLine("Message received\n");
                    }
                    break;
            }
        }
    }
}
