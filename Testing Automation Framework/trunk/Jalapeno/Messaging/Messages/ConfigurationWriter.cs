using System;
using System.Collections.Generic;
using Jalapeno.Config;


namespace Jalapeno.Messaging.Messages
{
    public class ReadConfigurationMessage : Message
    {
        public ReadConfigurationMessage(): base()
        {
            MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.READ_CONFIGURATION;

            MessagePacketizer.addCommandType();
            MessagePacket = MessagePacketizer.CompleteMessage();
        }

    }

    public class WriteConfigurationMessage : Message
    {

        public enum ConfigurationItemsEnum 
        {
             DEFAULT_MODE = 0, ADVANCED_MODE_1, ADVANCED_MODE_2, ADVANCED_MODE_3, ADVANCED_MODE_4, ADVANCED_MODE_5,
             GEOFENCE_0, GEOFENCE_1, GEOFENCE_2, GEOFENCE_3, GEOFENCE_4, GEOFENCE_5, GEOFENCE_6, GEOFENCE_7, GEOFENCE_8, GEOFENCE_9, 
             TIMEFENCE_0, TIMEFENCE_1, TIMEFENCE_2, 
             IO_SETTINGS, 
             NOTIFICATION_SETTINGS, 
             IRIDIUM_CONFIGURATION, 
             CELL_CONFIGURATION
        }

        public WriteConfigurationMessage() : base()
        {
            MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.WRITE_CONFIGURATION;
            MessagePacketizer.addCommandType();
        }

        ///<summary>
        ///<para> ModeNumber must be between 0-6 </para>
        //////<para> DefaultMode is held within ModeNumber 0 </para>
        ///<para> Advanced Modes are held within ModeNumber 1-5 </para>
        ///</summary>
        public void WriteMode(ConfigurationItemsEnum ModeNumber)
        {
            UInt16 ModeNum = 0;
            if (ModeNumber == ConfigurationItemsEnum.DEFAULT_MODE)
            {
                ModeNum = 0;
                MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode0, ModeNum);
            }
            else if (ModeNumber == ConfigurationItemsEnum.ADVANCED_MODE_1)
            {
                ModeNum = 1;
                MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode1, ModeNum);
            }
            else if (ModeNumber == ConfigurationItemsEnum.ADVANCED_MODE_2)
            {
                ModeNum = 2;
                MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode2, ModeNum);
            }
            else if (ModeNumber == ConfigurationItemsEnum.ADVANCED_MODE_3)
            {
                ModeNum = 3;
                MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode3, ModeNum);
            }
            else if (ModeNumber == ConfigurationItemsEnum.ADVANCED_MODE_4)
            {
                ModeNum = 4;
                MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode4, ModeNum);
            }
            else if (ModeNumber == ConfigurationItemsEnum.ADVANCED_MODE_5)
            {
                ModeNum = 5;
                MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode5, ModeNum);
            }            

            MessagePacket = MessagePacketizer.CompleteConfigMessage();
        }

        ///<summary>
        ///<para> GeofenceNumber must be between 0-9 </para>
        ///</summary>
        public void WriteGeofence(ConfigurationItemsEnum GeofenceNumber)
        {
            UInt16 GeofenceNum = 0;
            if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_0)
            {
                GeofenceNum = 0;
                MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence0, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_1)
            {
                GeofenceNum = 1;
                MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence1, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_2)
            {
                GeofenceNum = 2;
                MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence2, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_3)
            {
                GeofenceNum = 3;
                MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence3, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_4)
                        {
                GeofenceNum = 4;
                MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence4, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_5)
            {
                GeofenceNum = 5;
                MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence5, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_6)
            {
                GeofenceNum = 6;
                MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence6, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_7)
            {
                GeofenceNum = 7;
                MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence7, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_8)
            {
                GeofenceNum = 8;
                MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence8, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_9)
            {
                GeofenceNum = 9;
                MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence9, GeofenceNum);

            }

            MessagePacket = MessagePacketizer.CompleteConfigMessage();
        }

        ///<summary>
        ///<para> TimefenceNumber must be between 0-2 </para>
        ///</summary>
        public void WriteTimefence(ConfigurationItemsEnum TimefenceNumber)
        {
            UInt16 TimefenceNum = 0;
            if (TimefenceNumber == ConfigurationItemsEnum.TIMEFENCE_0)
            {
                TimefenceNum = 0;
                MessagePacketizer.addBlockInfo((UInt16)(Timefence.ConfigLocation + ((UInt16)(Timefence.ConfigLength * TimefenceNum))), Timefence.ConfigLength);
                MessagePacketizer.addTimefence(FullConfiguration.NewConfigItems.NewConfigTimefence0, TimefenceNum);
            }
            else if (TimefenceNumber == ConfigurationItemsEnum.TIMEFENCE_1)
            {
                TimefenceNum = 1;
                MessagePacketizer.addBlockInfo((UInt16)(Timefence.ConfigLocation + ((UInt16)(Timefence.ConfigLength * TimefenceNum))), Timefence.ConfigLength);
                MessagePacketizer.addTimefence(FullConfiguration.NewConfigItems.NewConfigTimefence1, TimefenceNum);
            }
            else if (TimefenceNumber == ConfigurationItemsEnum.TIMEFENCE_2)
            {
                TimefenceNum = 2;
                MessagePacketizer.addBlockInfo((UInt16)(Timefence.ConfigLocation + ((UInt16)(Timefence.ConfigLength * TimefenceNum))), Timefence.ConfigLength);
                MessagePacketizer.addTimefence(FullConfiguration.NewConfigItems.NewConfigTimefence2, TimefenceNum);
            }

            MessagePacket = MessagePacketizer.CompleteConfigMessage();
        }

        public void WriteIOSettings()
        {
            MessagePacketizer.addBlockInfo(IOSettings.ConfigLocation, IOSettings.ConfigLength);
            MessagePacketizer.addIOSettings(FullConfiguration.NewConfigItems.NewConfigIOSettings);

            MessagePacket = MessagePacketizer.CompleteConfigMessage();
        }

        public void WriteNotifications() 
        {
            MessagePacketizer.addBlockInfo(Notifications.ConfigLocation, Notifications.ConfigLength);
            MessagePacketizer.addNotifications(FullConfiguration.NewConfigItems.NewConfigNotifications);

            MessagePacket = MessagePacketizer.CompleteConfigMessage();
        }

        public void WriteIridium()
        {
            MessagePacketizer.addBlockInfo(Iridium.ConfigLocation, Iridium.ConfigLength);
            MessagePacketizer.addIridium(FullConfiguration.NewConfigItems.NewConfigIridium);

            MessagePacket = MessagePacketizer.CompleteConfigMessage();
        }

        public void WriteCell()
        {
            MessagePacketizer.addBlockInfo(Cell.ConfigLocation, Cell.ConfigLength);
            MessagePacketizer.addCell(FullConfiguration.NewConfigItems.NewConfigCell);

            MessagePacket = MessagePacketizer.CompleteConfigMessage();
        }       
    }

    public class MultiConfiguration
    {
        public enum ConfigurationItemsEnum
        {
            DEFAULT_MODE = 0, ADVANCED_MODE_1, ADVANCED_MODE_2, ADVANCED_MODE_3, ADVANCED_MODE_4, ADVANCED_MODE_5,
            GEOFENCE_0, GEOFENCE_1, GEOFENCE_2, GEOFENCE_3, GEOFENCE_4, GEOFENCE_5, GEOFENCE_6, GEOFENCE_7, GEOFENCE_8, GEOFENCE_9,
            TIMEFENCE_0, TIMEFENCE_1, TIMEFENCE_2,
            IO_SETTINGS,
            NOTIFICATION_SETTINGS,
            IRIDIUM_CONFIGURATION,
            CELL_CONFIGURATION
        }

        private List<Message> MultiConfigList;
        public Queue<Message> MultiConfigQueue;


        public MultiConfiguration() : base()
        {
            MultiConfigQueue = new Queue<Message>();
            MultiConfigList = new List<Message>();
        }

        public void CompleteMultiConfig()
        {
            Packetizer CRCPacketizer = new Packetizer();

            //Each message within the multi configuration list will have the final CRC added to it then queued
            foreach (Message element in MultiConfigList)
            {          
               //Debug.WriteLine("Initial MultiConfig Packet: " + BitConverter.ToString(element.MessagePacket));
                element.MessagePacket = CRCPacketizer.CompleteMultiConfigMessage(element.MessagePacket);
                MultiConfigQueue.Enqueue(element);              
               //Debug.WriteLine("New MultiConfig Packet 2: " + BitConverter.ToString(element.MessagePacket));
               //Debug.WriteLine("MultiConfig Queue Count: " + MultiConfigQueue.Count);
            }
        }        
        
        ///<summary>
        ///<para> ModeNumber must be between 0-5 </para>
        //////<para> Default Mode is held within ModeNumber 0 </para>
        ///<para> Advanced Modes are held within ModeNumber 1-5 </para>
        ///</summary>
        public void WriteMode(ConfigurationItemsEnum ModeNumber)
        {
            Message ConfigMessage = new Message();
            ConfigMessage.MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            ConfigMessage.MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.WRITE_CONFIGURATION;
            ConfigMessage.MessagePacketizer.addCommandType();

            UInt16 ModeNum = 0;
            if (ModeNumber == ConfigurationItemsEnum.DEFAULT_MODE)
            {
                ModeNum = 0;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                ConfigMessage.MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode0, ModeNum);
            }
            else if (ModeNumber == ConfigurationItemsEnum.ADVANCED_MODE_1)
            {
                ModeNum = 1;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                ConfigMessage.MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode1, ModeNum);
            }
            else if (ModeNumber == ConfigurationItemsEnum.ADVANCED_MODE_2)
            {
                ModeNum = 2;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                ConfigMessage.MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode2, ModeNum);
            }
            else if (ModeNumber == ConfigurationItemsEnum.ADVANCED_MODE_3)
            {
                ModeNum = 3;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                ConfigMessage.MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode3, ModeNum);
            }
            else if (ModeNumber == ConfigurationItemsEnum.ADVANCED_MODE_4)
            {
                ModeNum = 4;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                ConfigMessage.MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode4, ModeNum);
            }
            else if (ModeNumber == ConfigurationItemsEnum.ADVANCED_MODE_5)
            {
                ModeNum = 5;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Mode.ConfigLocation + (UInt16)(Mode.ConfigLength * ModeNum)), Mode.ConfigLength);
                ConfigMessage.MessagePacketizer.addDefaultMode(FullConfiguration.NewConfigItems.NewConfigMode5, ModeNum);
            }          

            ConfigMessage.MessagePacket = ConfigMessage.MessagePacketizer.CompleteConfigMessage();
            //MultiConfigQueue.Enqueue(ConfigMessage);
            MultiConfigList.Add(ConfigMessage);
        }

        ///<summary>
        ///<para> GeofenceNumber must be between 0-9 </para>
        ///</summary>
        public void WriteGeofence(ConfigurationItemsEnum GeofenceNumber)
        {
            Message ConfigMessage = new Message();
            ConfigMessage.MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            ConfigMessage.MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.WRITE_CONFIGURATION;
            ConfigMessage.MessagePacketizer.addCommandType();


            UInt16 GeofenceNum = 0;
            if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_0)
            {
                GeofenceNum = 0;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                ConfigMessage.MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence0, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_1)
            {
                GeofenceNum = 1;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                ConfigMessage.MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence1, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_2)
            {
                GeofenceNum = 2;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                ConfigMessage.MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence2, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_3)
            {
                GeofenceNum = 3;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                ConfigMessage.MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence3, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_4)
            {
                GeofenceNum = 4;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                ConfigMessage.MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence4, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_5)
            {
                GeofenceNum = 5;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                ConfigMessage.MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence5, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_6)
            {
                GeofenceNum = 6;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                ConfigMessage.MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence6, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_7)
            {
                GeofenceNum = 7;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                ConfigMessage.MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence7, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_8)
            {
                GeofenceNum = 8;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                ConfigMessage.MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence8, GeofenceNum);

            }
            else if (GeofenceNumber == ConfigurationItemsEnum.GEOFENCE_9)
            {
                GeofenceNum = 9;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Geofence.ConfigLocation + (UInt16)(Geofence.ConfigLength * GeofenceNum)), Geofence.ConfigLength);
                ConfigMessage.MessagePacketizer.addGeofence(FullConfiguration.NewConfigItems.NewConfigGeofence9, GeofenceNum);

            }

            ConfigMessage.MessagePacket = ConfigMessage.MessagePacketizer.CompleteConfigMessage();
            MultiConfigList.Add(ConfigMessage);
        }

        ///<summary>
        ///<para> TimefenceNumber must be between 0-2 </para>
        ///</summary>
        public void WriteTimefence(ConfigurationItemsEnum TimefenceNumber)
        {
            Message ConfigMessage = new Message();
            ConfigMessage.MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            ConfigMessage.MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.WRITE_CONFIGURATION;
            ConfigMessage.MessagePacketizer.addCommandType();

            UInt16 TimefenceNum = 0;
            if (TimefenceNumber == ConfigurationItemsEnum.TIMEFENCE_0)
            {
                TimefenceNum = 0;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Timefence.ConfigLocation + ((UInt16)(Timefence.ConfigLength * TimefenceNum))), Timefence.ConfigLength);
                ConfigMessage.MessagePacketizer.addTimefence(FullConfiguration.NewConfigItems.NewConfigTimefence0, TimefenceNum);
            }
            else if (TimefenceNumber == ConfigurationItemsEnum.TIMEFENCE_1)
            {
                TimefenceNum = 1;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Timefence.ConfigLocation + ((UInt16)(Timefence.ConfigLength * TimefenceNum))), Timefence.ConfigLength);
                ConfigMessage.MessagePacketizer.addTimefence(FullConfiguration.NewConfigItems.NewConfigTimefence1, TimefenceNum);
            }
            else if (TimefenceNumber == ConfigurationItemsEnum.TIMEFENCE_2)
            {
                TimefenceNum = 2;
                ConfigMessage.MessagePacketizer.addBlockInfo((UInt16)(Timefence.ConfigLocation + ((UInt16)(Timefence.ConfigLength * TimefenceNum))), Timefence.ConfigLength);
                ConfigMessage.MessagePacketizer.addTimefence(FullConfiguration.NewConfigItems.NewConfigTimefence2, TimefenceNum);
            }

            ConfigMessage.MessagePacket = ConfigMessage.MessagePacketizer.CompleteConfigMessage();
            MultiConfigList.Add(ConfigMessage);
        }

        public void WriteIOSettings()
        {
            Message ConfigMessage = new Message();
            ConfigMessage.MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            ConfigMessage.MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.WRITE_CONFIGURATION;
            ConfigMessage.MessagePacketizer.addCommandType();
            ConfigMessage.MessagePacketizer.addBlockInfo(IOSettings.ConfigLocation, IOSettings.ConfigLength);
            ConfigMessage.MessagePacketizer.addIOSettings(FullConfiguration.NewConfigItems.NewConfigIOSettings);

            ConfigMessage.MessagePacket = ConfigMessage.MessagePacketizer.CompleteConfigMessage();
            MultiConfigList.Add(ConfigMessage);
        }

        public void WriteNotifications()
        {
            Message ConfigMessage = new Message();
            ConfigMessage.MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            ConfigMessage.MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.WRITE_CONFIGURATION;
            ConfigMessage.MessagePacketizer.addCommandType();
            ConfigMessage.MessagePacketizer.addBlockInfo(Notifications.ConfigLocation, Notifications.ConfigLength);
            ConfigMessage.MessagePacketizer.addNotifications(FullConfiguration.NewConfigItems.NewConfigNotifications);

            ConfigMessage.MessagePacket = ConfigMessage.MessagePacketizer.CompleteConfigMessage();
            MultiConfigList.Add(ConfigMessage);
        }

        public void WriteIridium()
        {
            Message ConfigMessage = new Message();
            ConfigMessage.MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            ConfigMessage.MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.WRITE_CONFIGURATION;
            ConfigMessage.MessagePacketizer.addCommandType();
            ConfigMessage.MessagePacketizer.addBlockInfo(Iridium.ConfigLocation, Iridium.ConfigLength);
            ConfigMessage.MessagePacketizer.addIridium(FullConfiguration.NewConfigItems.NewConfigIridium);

            ConfigMessage.MessagePacket = ConfigMessage.MessagePacketizer.CompleteConfigMessage();
            MultiConfigList.Add(ConfigMessage);
        }

        public void WriteCell()
        {
            Message ConfigMessage = new Message();
            ConfigMessage.MessagePacketizer.PacketType = MessageHandler.PacketTypeEnum.AppData;
            ConfigMessage.MessagePacketizer.CommandType = MessagePacketData.CommandTypeEnum.WRITE_CONFIGURATION;
            ConfigMessage.MessagePacketizer.addCommandType();
            ConfigMessage.MessagePacketizer.addBlockInfo(Cell.ConfigLocation, Cell.ConfigLength);
            ConfigMessage.MessagePacketizer.addCell(FullConfiguration.NewConfigItems.NewConfigCell);

            ConfigMessage.MessagePacket = ConfigMessage.MessagePacketizer.CompleteConfigMessage();
            MultiConfigList.Add(ConfigMessage);
        }   
    }

} 
