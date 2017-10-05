using System;
using System.Diagnostics;
using Jalapeno.Messaging.Messages;

namespace Jalapeno
{
    public class Session
    {
        public String ChosenCOM;
        public SerialLink TG300;

        public Session(String COM)
        {
            ChosenCOM = COM;
            TG300 = new SerialLink();
            TG300.ConnectSerialPort(ChosenCOM);

        }

        ////////WRITING CONFIGURATIONS////////

        public void WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum ConfigItem)
        {
            WriteConfigurationMessage NewConfig = new WriteConfigurationMessage();
                        
            if ((WriteConfigurationMessage.ConfigurationItemsEnum.DEFAULT_MODE <= ConfigItem) && (ConfigItem <= WriteConfigurationMessage.ConfigurationItemsEnum.ADVANCED_MODE_5))
            {
                NewConfig.WriteMode(ConfigItem);
            }
            else if ((WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_0 <= ConfigItem) && (ConfigItem <= WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_9))
            {
                NewConfig.WriteGeofence(ConfigItem);
            }
            else if ((WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0 <= ConfigItem) && (ConfigItem <= WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_2))
            {
                NewConfig.WriteTimefence(ConfigItem);
            }
            else if (ConfigItem == WriteConfigurationMessage.ConfigurationItemsEnum.IO_SETTINGS)
            {
                NewConfig.WriteIOSettings();
            }
            else if (ConfigItem == WriteConfigurationMessage.ConfigurationItemsEnum.NOTIFICATION_SETTINGS)
            {
                NewConfig.WriteNotifications();
            }
            else if (ConfigItem == WriteConfigurationMessage.ConfigurationItemsEnum.IRIDIUM_CONFIGURATION)
            {
                NewConfig.WriteIridium();
            }
            else if (ConfigItem == WriteConfigurationMessage.ConfigurationItemsEnum.CELL_CONFIGURATION)
            {
                NewConfig.WriteCell();
            }

            TG300.MessagesToSend.Enqueue(NewConfig);
            Debug.WriteLine("Config Packet: " + BitConverter.ToString(NewConfig.MessagePacket));
        }

        public void WriteMultiConfig(params MultiConfiguration.ConfigurationItemsEnum[] ConfigItems)
        {
            Debug.WriteLine("...Updating Multi Configuration...\n");

            MultiConfiguration NewConfig = new MultiConfiguration();

            for (int i = 0; i < ConfigItems.Length; i++)
            {
                Debug.WriteLine("Current Config item:"+ ConfigItems[i] + "\n");
                if ((MultiConfiguration.ConfigurationItemsEnum.DEFAULT_MODE <= ConfigItems[i]) && (ConfigItems[i] <= MultiConfiguration.ConfigurationItemsEnum.ADVANCED_MODE_5))
                {
                    NewConfig.WriteMode(ConfigItems[i]);
                }
                else if ((MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_0 <= ConfigItems[i]) && (ConfigItems[i] <= MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_9))
                {
                    NewConfig.WriteGeofence(ConfigItems[i]);
                }
                else if ((MultiConfiguration.ConfigurationItemsEnum.TIMEFENCE_0 <= ConfigItems[i]) && (ConfigItems[i] <= MultiConfiguration.ConfigurationItemsEnum.TIMEFENCE_2))
                {
                    NewConfig.WriteTimefence(ConfigItems[i]);
                }
                else if (ConfigItems[i] == MultiConfiguration.ConfigurationItemsEnum.IO_SETTINGS)
                {
                    NewConfig.WriteIOSettings();
                }
                else if (ConfigItems[i] == MultiConfiguration.ConfigurationItemsEnum.IRIDIUM_CONFIGURATION)
                {
                    NewConfig.WriteIridium();
                }
                else if (ConfigItems[i] == MultiConfiguration.ConfigurationItemsEnum.CELL_CONFIGURATION)
                {
                    NewConfig.WriteCell();
                }
            }

            NewConfig.CompleteMultiConfig();

            lock (TG300.MessagesToSend)
            {
                int MultiConfigQueueLength = NewConfig.MultiConfigQueue.Count;

                for (int i = 0; i < MultiConfigQueueLength; i++)
                {
                    TG300.MessagesToSend.Enqueue(NewConfig.MultiConfigQueue.Dequeue()); 
                }
            }

        }

        ////////SENDING MESSAGES//////////

        public void ReadConfig()
        {
            ReadConfigurationMessage ConfigRequest = new ReadConfigurationMessage();
            TG300.MessagesToSend.Enqueue(ConfigRequest);           
        }

        public void RequestPosition()
        {
            RequestPositionMessage PositionRequest = new RequestPositionMessage();
            TG300.MessagesToSend.Enqueue(PositionRequest);
        }

        public void EnableRealTimeTracking()
        {
            EnableRealTimeTrackingMessage EnableRealTimeTrackingRequest = new EnableRealTimeTrackingMessage();
            TG300.MessagesToSend.Enqueue(EnableRealTimeTrackingRequest);
        }

        public void DisableRealTimeTracking()
        {
            DisableRealTimeTrackingMessage DisableRealTimeTrackingRequest = new DisableRealTimeTrackingMessage();
            TG300.MessagesToSend.Enqueue(DisableRealTimeTrackingRequest);
        }
        public void RequestDiagnostics()
        {
            DiagnosticStatusMessage DiagnosticStatusRequest = new DiagnosticStatusMessage();
            TG300.MessagesToSend.Enqueue(DiagnosticStatusRequest);
        }

        public void RequestIdentification()
        {
            IDRequestMessage IDRequest = new IDRequestMessage();
            TG300.MessagesToSend.Enqueue(IDRequest);
        }

        ////////MESSAGE TYPE LISTENER///////

        public void ListenForCommandType(Messaging.MessagePacketData.CommandTypeEnum CommandTypeToFlag)
        {
            TG300.MsgListener.EnableMsgListener();
            TG300.MsgListener.CommandTypeFlag = false;
            TG300.MsgListener.CommandTypeFlagged = CommandTypeToFlag;
        }

        public void ListenForPacketType(Messaging.MessageHandler.PacketTypeEnum PacketTypeToFlag)
        {
            TG300.MsgListener.EnableMsgListener();
            TG300.MsgListener.PacketTypeFlag = false;
            TG300.MsgListener.PacketTypeFlagged = PacketTypeToFlag;
        }
    }
}
