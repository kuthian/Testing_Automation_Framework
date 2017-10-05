using System;
using System.Collections.Generic;
using Jalapeno.Utils;
using Jalapeno.Config;

namespace Jalapeno.Messaging
{
    public class Packetizer
    {
        public MessagePacketData.CommandTypeEnum CommandType;
        public MessageHandler.PacketTypeEnum PacketType;
        public UInt16 DataLength;
        public UInt16 ConfigDataLength;
        public Queue<byte[]> Packet;

        public Packetizer()
        {
            Packet = new Queue<byte[]>();
            addPacketHeaderStart();
            DataLength = 0;
            ConfigDataLength = 0;
        }

        public byte[] CompleteMessage()
        {
            addPacketHeaderEnd();
            byte[] CompletePacket = Tools.CreateByteArrayFromQueue(Packet);
            addDataType(CompletePacket);
            addDataLength(CompletePacket);
            addCRC(CompletePacket);

            return CompletePacket;
        }

        public byte[] CompleteConfigMessage()
        {
            addPacketHeaderEnd();
            byte[] CompletePacket = Tools.CreateByteArrayFromQueue(Packet);
            addDataType(CompletePacket);
            addDataLength(CompletePacket);           
            addConfigCRC(CompletePacket);
            addCRC(CompletePacket);

            return CompletePacket;
        }
        public byte[] CompleteMultiConfigMessage(byte[] CompletePacket)
        {
            addConfigCRC(CompletePacket);
            addCRC(CompletePacket);

            return CompletePacket;
        }

        private void addDataType(byte[] CompletePacket)
        {
            CompletePacket[6] = (byte) PacketType;
        }
        private void addPacketHeaderStart()
        {
            byte[] HeaderStart = new byte[9];

            //SOF
            HeaderStart[0] = 0xAA;
            HeaderStart[1] = 0x55;
            //Broadcast values
            HeaderStart[2] = 0xFF;
            HeaderStart[3] = 0xFF;
            HeaderStart[4] = 0xFF;
            HeaderStart[5] = 0xFF;

            //PacketType placeholder
            HeaderStart[6] = 0x00;

            //DataLength placeholders
            HeaderStart[7] = 0x00;
            HeaderStart[8] = 0x00;

            Packet.Enqueue(HeaderStart);
            
        }

        private void addPacketHeaderEnd()
        {
            byte[] HeaderEnd = new byte[4];

            //CRC placeholders
            HeaderEnd[0] = 0x00;
            HeaderEnd[1] = 0x00;
            //EOF
            HeaderEnd[2] = 0xFF;
            HeaderEnd[3] = 0xCC;

            Packet.Enqueue(HeaderEnd);
        }

        private void addCRC(byte[] CompletePacket)
        {
            UInt16 CompletePacketCRC = UtilCRC.calculateCRC(CompletePacket, 2, CompletePacket.Length - 6);
            CompletePacket[CompletePacket.Length - 4] = Tools.getLowByte(CompletePacketCRC);
            CompletePacket[CompletePacket.Length - 3] = Tools.getHighByte(CompletePacketCRC);
        }

        public void addConfigCRC(byte[] CompletePacket)
        {
            CompletePacket[11] = Tools.getLowByte(FullConfiguration.UpdatedConfigCRC);
            CompletePacket[12] = Tools.getHighByte(FullConfiguration.UpdatedConfigCRC);
        }

         
        private void addDataLength(byte[] CompletePacket)
        {
            CompletePacket[7] = BitConverter.GetBytes(DataLength)[0];
            CompletePacket[8] = BitConverter.GetBytes(DataLength)[1];
        }

        public void addCommandType()
        {
            byte[] CommandArray = new byte[2];

            switch(CommandType)
            {
                case (MessagePacketData.CommandTypeEnum.IDENTIFICATION_REQUEST):
                    {
                        addCommandArray(0x03, 0x01);
                    }
                    break;
                case (MessagePacketData.CommandTypeEnum.DIAGNOSTIC_STATUS_REQUEST):
                    {
                        addCommandArray(0x03, 0x02);
                    }
                    break;
                case (MessagePacketData.CommandTypeEnum.READ_CONFIGURATION):
                    {
                        addCommandArray(0x05, 0x01);
                    }
                    break;
                case (MessagePacketData.CommandTypeEnum.WRITE_CONFIGURATION):
                    {
                        addCommandArray(0x05, 0x02);
                    }
                    break;
                case (MessagePacketData.CommandTypeEnum.STOP_DOWNLOAD_REQUEST):
                    {
                        addCommandArray(0x09, 0x20);
                    }
                    break;
                case (MessagePacketData.CommandTypeEnum.START_DOWNLOAD_REQUEST):
                    {
                        addCommandArray(0x09, 0x21);
                    }
                    break;
                case (MessagePacketData.CommandTypeEnum.CLEAR_MEMORY_LOG_REQUEST):
                    {
                        addCommandArray(0x09, 0x30);
                    }
                    break;
                case (MessagePacketData.CommandTypeEnum.ENABLE_REALTIME_TRACKING_REQUEST):
                    {
                        addCommandArray(0x0B, 0x01);
                    }
                    break;
                case (MessagePacketData.CommandTypeEnum.DISABLE_REALTIME_TRACKING_REQUEST):
                    {
                        addCommandArray(0x0B, 0x02);
                    }
                    break;
                case (MessagePacketData.CommandTypeEnum.REQUEST_POSITION):
                    {
                        addCommandArray(0x0B, 0x03);
                    }
                    break;
                case (MessagePacketData.CommandTypeEnum.OUTPUT_CONTROL_REQUEST):
                    {
                        addCommandArray(0x0B, 0x05);
                    }
                    break;
            }

            DataLength += 2;
        }

        private void addCommandArray(byte CommandHex, byte SubCommandHex)
        {
            byte[] CommandArray = new byte[2];
            CommandArray[0] = CommandHex;
            CommandArray[1] = SubCommandHex;

            Packet.Enqueue(CommandArray);
        }

        //adding CRC_U32 [4], ConfigLocation [2], ConfigLength [1] 
        public void addBlockInfo(UInt16 ConfigLocation, byte ConfigLength)
        {
            byte[] BlockInfoArray = new byte[7];
            BlockInfoArray[4] = Tools.getLowByte(ConfigLocation);
            BlockInfoArray[5] = Tools.getHighByte(ConfigLocation);
            BlockInfoArray[6] = ConfigLength;
            Packet.Enqueue(BlockInfoArray);

            DataLength += 7;
        }

        public void addDefaultMode(Mode NewDefaultMode, UInt16 ModeNumber)
        {
            Packet.Enqueue(NewDefaultMode.ToByteArray());
            FullConfiguration.UpdateConfigurationArray(NewDefaultMode.ToByteArray(), (UInt16) (Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNumber)));
            DataLength += 46;
            ConfigDataLength += 46;
        }

        public void addGeofence(Geofence NewGeofence, UInt16 GeofenceNumber)
        {
            Packet.Enqueue(NewGeofence.ToByteArray());
            FullConfiguration.UpdateConfigurationArray(NewGeofence.ToByteArray(), (UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNumber)));

            DataLength += 25;
            ConfigDataLength += 25;
        }

        //Adding a static timefence for now, this info will eventually be read from a file 
        public void addTimefence(Timefence NewTimefence, UInt16 TimefenceNumber)
        {
            Packet.Enqueue(NewTimefence.ToByteArray());
            FullConfiguration.UpdateConfigurationArray(NewTimefence.ToByteArray(), (UInt16)(Timefence.ConfigLocation + (UInt16)(Timefence.ConfigLength * TimefenceNumber)));

            DataLength += 17;
            ConfigDataLength += 17;
        }


        public void addIOSettings(IOSettings NewIOSettings)
        {
            Packet.Enqueue(NewIOSettings.ToByteArray());
            FullConfiguration.UpdateConfigurationArray(NewIOSettings.ToByteArray(), IOSettings.ConfigLocation);

            DataLength += 2;
            ConfigDataLength += 2;
        }

        public void addNotifications(Notifications NewNotifications)
        {
            Packet.Enqueue(NewNotifications.ToByteArray());
            FullConfiguration.UpdateConfigurationArray(NewNotifications.ToByteArray(), Notifications.ConfigLocation);

            DataLength += 11;
            ConfigDataLength += 11;
        }

        public void addIridium(Iridium NewIridium)
        {
            Packet.Enqueue(NewIridium.ToByteArray());
            FullConfiguration.UpdateConfigurationArray(NewIridium.ToByteArray(), Iridium.ConfigLocation);

            DataLength += 32;
            ConfigDataLength += 32;
        }

        public void addCell(Cell NewCell)
        {
            Packet.Enqueue(NewCell.ToByteArray());
            FullConfiguration.UpdateConfigurationArray(NewCell.ToByteArray(), Cell.ConfigLocation);

            DataLength += 186;
            ConfigDataLength += 186;
        }

        public void addTimeout(UInt16 TimeoutInSeconds)
        {
            byte[] TimeoutArray = new byte[2];
            TimeoutArray = BitConverter.GetBytes(TimeoutInSeconds);
            Packet.Enqueue(TimeoutArray);
            DataLength += 2;
        }
    }
}
