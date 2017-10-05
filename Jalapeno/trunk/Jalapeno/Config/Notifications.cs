using System;
using System.Collections.Generic;
using Jalapeno.Utils;

namespace Jalapeno.Config
{
    public class Notifications
    {
        public enum NotificationTypeEnum { GEOFENCE_0 = 0, GEOFENCE_1, GEOFENCE_2, GEOFENCE_3, GEOFENCE_4, GEOFENCE_5, GEOFENCE_6, GEOFENCE_7, GEOFENCE_8, GEOFENCE_9, TIMEFENCE_0, TIMEFENCE_1, TIMEFENCE_2, INPUT_0 = 20, INPUT_1, INPUT_2, INPUT_3, INPUT_4, INPUT_5, INPUT_6, INPUT_7, INPUT_8, INPUT_9, ON_MOTION, LOW_BATTERY };
        public enum NotificationPathTypeEnum {SERVER = 1, SMS, BOTH };
        NotificationPathTypeEnum NotificationPathType;

        private byte[] NotificationHash; //64-bit mask
        private byte[] NotificationPath; //U8, 1 = Server (IP or Iridium), 2 = SMS, 3 = Both
        private byte[] LowBatteryThresh; //U16, Tenths of Volts (i.e. 0.01 increments), stored in little endian
        private Queue<byte[]> NotificationsArrayQueue;

        public static byte ConfigLength;
        public static UInt16 ConfigLocation;

        //Default Notifications
        public Notifications()
        {
            NotificationHash = new byte[8];
            NotificationPath = new byte[1];
            LowBatteryThresh = new byte[2];
            NotificationsArrayQueue = new Queue<byte[]>();

            ConfigLength = 11;
            ConfigLocation = 579;
        }

        public Notifications(NotificationTypeEnum Type, NotificationPathTypeEnum PathType, UInt16 VoltageThresh)
        {
            NotificationHash = new byte[8];
            NotificationPath = new byte[1];
            LowBatteryThresh = new byte[2];
            NotificationsArrayQueue = new Queue<byte[]>();

            ConfigLength = 11;
            ConfigLocation = 579;

            setNotificationHash(Type);
            setNotificationPath(PathType);
            setLowBatteryThresh(VoltageThresh);
        }

        public void readNotifications(byte[] MsgReceived, int pos)
        {
            NotificationHash = Tools.ArrayFromArray(MsgReceived, pos, pos + 7);
            pos += 8;
            NotificationPathType = (NotificationPathTypeEnum) MsgReceived[pos];
            NotificationPath[0] = MsgReceived[pos];
            pos++;
            LowBatteryThresh = Tools.ArrayFromArray(MsgReceived, pos, pos + 1);
            pos += 2;

        }
        public byte [] ToByteArray()
        {
            NotificationsArrayQueue.Enqueue(NotificationHash);
            NotificationsArrayQueue.Enqueue(NotificationPath);
            NotificationsArrayQueue.Enqueue(LowBatteryThresh);

            return Tools.CreateByteArrayFromQueue(NotificationsArrayQueue);
        }

        public void setNotificationHash()
        {
            Array.Clear(NotificationHash, 0, NotificationHash.Length);
        }

        public void setNotificationHash(NotificationTypeEnum Type)
        {
            //NotificationHash stored in Little Endian, convert before making changes ?
            
            int NotificationBitPosition = ((int) Type) % 8; // 2
            int NotificationBytePosition = ((int) Type) / 8; //1

            NotificationHash[NotificationBytePosition] = Tools.SetBit(NotificationHash[NotificationBytePosition], NotificationBitPosition);

        }

        public void setNotificationPath(NotificationPathTypeEnum PathType)
        {
            NotificationPathType = PathType;
            Console.WriteLine("Pathtype Converted Array: " + BitConverter.ToString(Tools.IntToSingleByteArray((int) PathType)));
            NotificationPath = Tools.IntToSingleByteArray((int) PathType);
            Console.WriteLine("Notification Path Array: " + BitConverter.ToString(NotificationPath));
        }

        //VoltageThresh is given in Volts but stored in 0.01 increments
        public void setLowBatteryThresh(UInt16 VoltageThresh)
        {
            VoltageThresh *= 100;
            LowBatteryThresh = BitConverter.GetBytes(VoltageThresh);
        }
    }
}
