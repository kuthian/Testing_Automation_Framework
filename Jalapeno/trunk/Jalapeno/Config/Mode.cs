using System;
using System.Collections.Generic;
using Jalapeno.Utils;

namespace Jalapeno.Config
{
    public class Mode
    {
        public enum GPSModeEnum { MOTION_ONLY = 0, ALWAYS_ON, RESERVED, DISABLED};
        public GPSModeEnum GPSMode; 

        public enum CellModeEnum { DISABLED = 0, ON_DEMAND, STANDBY, IP_CONNECTED};
        public CellModeEnum CellMode;

        //All stored in Little Endian
        private byte[] GpsFixInterval; //UInt32
        private byte[] TransmitInterval; //UInt32
        private byte[] MailboxCheckInterval_NoMotion; //UInt32
        private byte[] MailboxCheckInterval_Motion; //UInt32
        private byte[] OperationFlags; //UInt16
        private byte[] OutputChangeFlags; //UInt16
        private byte[] OutputStateFlags; //UInt16
        private ModeHashcode TrueHash;
        private ModeHashcode FalseHash;
        private Queue<byte[]> DefaultModeArrayQueue;

        public static byte ConfigLength;
        public static UInt16 ConfigLocation;

        public Mode()
        {
            GpsFixInterval = new byte[4]; //UInt32
            TransmitInterval = new byte[4]; //UInt32
            MailboxCheckInterval_NoMotion = new byte[4]; //UInt32
            MailboxCheckInterval_Motion = new byte[4]; //UInt32
            OperationFlags = new byte[2]; //UInt16
            OutputChangeFlags = new byte[2]; //UInt16
            OutputStateFlags = new byte[2]; //UInt16
            TrueHash = new ModeHashcode();
            FalseHash = new ModeHashcode();
            DefaultModeArrayQueue = new Queue<byte[]>();
            ConfigLength = 46;
            ConfigLocation = 0;
        }
        
        public byte[] ToByteArray()
        {
            DefaultModeArrayQueue.Enqueue(GpsFixInterval);
            DefaultModeArrayQueue.Enqueue(TransmitInterval);
            DefaultModeArrayQueue.Enqueue(MailboxCheckInterval_NoMotion);
            DefaultModeArrayQueue.Enqueue(MailboxCheckInterval_Motion);
            DefaultModeArrayQueue.Enqueue(OperationFlags);
            DefaultModeArrayQueue.Enqueue(OutputChangeFlags);
            DefaultModeArrayQueue.Enqueue(OutputStateFlags);

            byte[] ReservedArray = new byte [8];
            DefaultModeArrayQueue.Enqueue(ReservedArray);

            DefaultModeArrayQueue.Enqueue(TrueHash.ToByteArray());
            DefaultModeArrayQueue.Enqueue(FalseHash.ToByteArray());
            
            return Tools.CreateByteArrayFromQueue(DefaultModeArrayQueue);
            
        }


        public void readMode(byte[] MessageReceived, int pos)
        {
            
            GpsFixInterval = Tools.ArrayFromArray(MessageReceived, pos, pos+3); 
            pos += 4;
            TransmitInterval = Tools.ArrayFromArray(MessageReceived, pos, pos + 3); 
            pos += 4;
            MailboxCheckInterval_NoMotion = Tools.ArrayFromArray(MessageReceived, pos, pos + 3); 
            pos += 4;
            MailboxCheckInterval_Motion = Tools.ArrayFromArray(MessageReceived, pos, pos + 3); 
            pos += 4;
            OperationFlags = Tools.ArrayFromArray(MessageReceived, pos, pos + 1); 
            pos += 2;
            OutputChangeFlags = Tools.ArrayFromArray(MessageReceived, pos, pos + 1); 
            pos += 2;
            OutputStateFlags = Tools.ArrayFromArray(MessageReceived, pos, pos + 1); 
            pos += 10; //skip reserved bytes
            TrueHash.readHashCode(MessageReceived, pos);
            pos += 8;
            FalseHash.readHashCode(MessageReceived, pos);
            pos += 8;


        }
        ///<summary>
        ///<para> GPS fix interval in seconds </para>
        ///<para> Minimum = 1 (second) </para>
        ///<para> Maximum = 172800 (2 days) </para>
        ///<para> Set this field to 0 to disable periodic GPS fixes </para>
        ///</summary>
        ///
        public void SetGpsFixInterval(UInt32 Interval)
        {
            GpsFixInterval = BitConverter.GetBytes(Interval);
        }
        
        ///<summary>
        ///<para> Periodic data transmit interval in seconds </para>
        ///<para> Minimum = 120 (2 minutes) </para>
        ///<para> Maximum = 172800 (2 days) </para>
        ///<para> Set this field to 0 to disable periodic transmissions</para>
        ///</summary>
        public void SetTransmitInterval(UInt32 Interval)
        {
            TransmitInterval = BitConverter.GetBytes(Interval);
        }

        ///<summary>
        ///<para> Mailbox check interval in seconds </para>
        ///<para> Minimum = 120 (2 minutes) </para>
        ///<para> Maximum = 172800 (2 days) </para>
        ///<para> Set this field to 0 to disable mailbox checks during no motion</para>
        ///</summary>
        public void SetMailboxCheckInterval_NoMotion(UInt32 Interval)
        {
            MailboxCheckInterval_NoMotion = BitConverter.GetBytes(Interval);
        }

        ///<summary>
        ///<para> Mailbox check interval in seconds </para>
        ///<para> Minimum = 120 (2 minutes) </para>
        ///<para> Maximum = 172800 (2 days) </para>
        ///<para> Setting this field to 0 means that the device will operate under the “no motion” setting during motion. In this case if the “no motion” setting is 0, then motion mailbox checks will be disabled as well. </para>
        ///</summary>
        public void SetMailboxCheckInterval_Motion(UInt32 Interval)
        {
            MailboxCheckInterval_Motion = BitConverter.GetBytes(Interval);
        }

        ///<summary>
        ///<para> Bits 0-1: GPS Mode </para>
        ///<para> Bit 2: GPS Logging Disable </para>
        ///<para> Bit 6:  Iridium Enable </para>
        ///<para> Bits 8-10: Cell Mode (TG-300 only)</para>
        ///<para> All other bytes set to 0</para>
        ///</summary>
        ///                     byte [1]                byte [0]
        ///   Bit Position: 7  6  5  4  3  2  1 0    7  6  5  4  3  2  1 0                
        ///LE Bit Position: 7  6  5  4  3  2  1 0 || 15 14 13 12 11 10 9 8 
        ///BE Bit Position: 15 14 13 12 11 10 9 8 || 7  6  5  4  3  2  1 0
        public void setOperationFlags(GPSModeEnum GpsMode, bool GPSLoggingDisable, bool IridiumEnable, CellModeEnum CellularMode)
        {
            //set to 0 as precaution
            Array.Clear(OperationFlags, 0, OperationFlags.Length);

            //Bits 0-1: GPS Mode
            switch(GpsMode)
            {
            case (GPSModeEnum.MOTION_ONLY): 
                {
                    OperationFlags[1] = Tools.ClearBit(OperationFlags[1], 0);
                    OperationFlags[1] = Tools.ClearBit(OperationFlags[1], 1);
                }
                break;
            case (GPSModeEnum.ALWAYS_ON): 
                {
                    OperationFlags[1] = Tools.SetBit(OperationFlags[1], 0);
                    OperationFlags[1] = Tools.ClearBit(OperationFlags[1], 1);
                }
                break;
            case (GPSModeEnum.RESERVED): 
                {
                    OperationFlags[1] = Tools.ClearBit(OperationFlags[1], 0);
                    OperationFlags[1] = Tools.SetBit(OperationFlags[1], 1);
                }
                break;
            case (GPSModeEnum.DISABLED): 
                {
                    OperationFlags[1] = Tools.SetBit(OperationFlags[1], 0);
                    OperationFlags[1] = Tools.SetBit(OperationFlags[1], 1);
                }
                break;
            }

            //Bit 2: GPS Logging Disable
            if(GPSLoggingDisable)
            {
                OperationFlags[1] = Tools.SetBit(OperationFlags[1], 2); // Disable GPS logging
            }
            else
            {
                OperationFlags[1] = Tools.ClearBit(OperationFlags[1], 2); // Disable GPS logging
            }

            //Bits 3-5: Reserved (Set to 0)
            OperationFlags[1] = Tools.ClearBit(OperationFlags[1], 3);
            OperationFlags[1] = Tools.ClearBit(OperationFlags[1], 4);
            OperationFlags[1] = Tools.ClearBit(OperationFlags[1], 5);

            //Bit 6:  Iridium Enable
            if (IridiumEnable)
            {
                OperationFlags[1] = Tools.SetBit(OperationFlags[1], 6);; //Enable Iridium
            }
            else
            {
                OperationFlags[1] = Tools.ClearBit(OperationFlags[1], 6); //Disable Iridium
            }
            //Bit 7: Reserved (Set to 0)
            OperationFlags[1] = Tools.ClearBit(OperationFlags[1], 7);

            ////////////
            //New Byte//
            ////////////

            
            //Bits 8-10: Cell Mode (TG-300 only)
            switch(CellularMode)
            {
            case (CellModeEnum.DISABLED): 
                {
                    OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 0);
                    OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 1);
                    OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 2);
                }

                break;
            case (CellModeEnum.ON_DEMAND): 
                {
                    OperationFlags[0] = Tools.SetBit(OperationFlags[0], 0);
                    OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 1);
                    OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 2);
                }
                break;
            case (CellModeEnum.STANDBY): 
                {
                    OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 0);
                    OperationFlags[0] = Tools.SetBit(OperationFlags[0], 1);
                    OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 2);
                }
                break;
            case (CellModeEnum.IP_CONNECTED):
                {
                    OperationFlags[0] = Tools.SetBit(OperationFlags[0], 0);
                    OperationFlags[0] = Tools.SetBit(OperationFlags[0], 1);
                }
                break;
            }

            //Bits 10-15: Reserved (Set to 0)
            OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 10);
            OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 11);
            OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 12);
            OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 13);
            OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 14);
            OperationFlags[0] = Tools.ClearBit(OperationFlags[0], 15);
        }

        public void setOperationFlags(UInt16 Flags)
        {
            OperationFlags = BitConverter.GetBytes(Flags);
        }

        ///<summary>
        ///<para> Bitfield of the enables for changing the Output State </para>
        ///<para> Bit i: Output[i]; </para>
        ///<para> 0 = Keep existing state (ignore OutputStateFlags) </para>
        ///<para> 1 = Apply state from OutputStateFlags Bit i</para>
        ///</summary>
        private void setOutputChangeFlags(bool[] OutputChange)
        {
            Array.Clear(OutputChangeFlags, 0, OutputChangeFlags.Length);

            for (int i = 0; i < 8; i++)
            {
                if (OutputChange[i])
                {
                    Tools.SetBit(OutputChangeFlags[0], i);
                }
                else
                {
                    Tools.ClearBit(OutputChangeFlags[0], i);
                }
            }

            for (int i = 0; i < 2; i++)
            {
                if (OutputChange[i + 8])
                {
                    Tools.SetBit(OutputChangeFlags[1], i);
                }
                else
                {
                    Tools.ClearBit(OutputChangeFlags[1], i);
                }
            }
        }

        public void setOutputChangeFlags(UInt16 Flags)
        {
            OutputChangeFlags = BitConverter.GetBytes(Flags);
        }

        ///<summary>
        ///<para> Bitfield of the desired Output State </para>
        ///<para> Bit i: Output[i]; </para>
        ///<para> 0 = LOW </para>
        ///<para> 1 = HIGH</para>
        ///</summary>
        public void setOutputStateFlags(bool[] OutputState)
        {
            Array.Clear(OutputStateFlags, 0, OutputStateFlags.Length);

            for (int i = 0; i < 8; i++)
            {
                if (OutputState[i])
                {
                    Tools.SetBit(OutputStateFlags[0], i);
                }
                else
                {
                    Tools.ClearBit(OutputStateFlags[0], i);
                }
            }

            for (int i = 0; i < 2; i++)
            {
                if (OutputState[i+8])
                {
                    Tools.SetBit(OutputStateFlags[1], i);
                }
                else
                {
                    Tools.ClearBit(OutputStateFlags[1], i);
                }
            }
        }

        public void setOutputStateFlags(UInt16 Flags)
        {
            OutputStateFlags = BitConverter.GetBytes(Flags);
        }
    
    }

    class ModeHashcode
    {

        public enum ModeTriggerEnum { GEOFENCE_0 = 0, GEOFENCE_1, GEOFENCE_2, GEOFENCE_3, GEOFENCE_4, GEOFENCE_5, GEOFENCE_6, GEOFENCE_7, GEOFENCE_8, GEOFENCE_9, TIMEFENCE_0, TIMEFENCE_1, TIMEFENCE_2, INPUT_0 = 20, INPUT_1, INPUT_2, INPUT_3, INPUT_4, INPUT_5, INPUT_6, INPUT_7, INPUT_8, INPUT_9, ON_MOTION };
        //ModeTriggerEnum ModeTrigger;

        private byte[] ModeHashcodeArray;

        public ModeHashcode()
        {
            ModeHashcodeArray = new byte[8];
        }

        //Sets corresponding bit in ModehashcodeArray. 
        //Does not convert to Little Endian, allowing this method 
        //to be called multiple times before ToByteArray() is called.
        public void setTrigger(ModeTriggerEnum Trigger)
        {
            //check to see if switch endian is working correctly
            ModeHashcodeArray = Tools.SwitchEndian(ModeHashcodeArray);

            int TriggerBitPosition = ((int)Trigger) % 8;
            int TriggerBytePosition = ((int)Trigger) / 8;

            ModeHashcodeArray[TriggerBytePosition] = Tools.SetBit(ModeHashcodeArray[0], TriggerBitPosition);

            ModeHashcodeArray = Tools.SwitchEndian(ModeHashcodeArray);
        }

        public void readHashCode(byte[] MessageReceived, int pos)
        {
            setModeHashcode(BitConverter.ToUInt64(MessageReceived, pos));
        }

        public void setModeHashcode(UInt64 Hashcode)
        {
            ModeHashcodeArray = BitConverter.GetBytes(Hashcode);
        }

        public byte[] ToByteArray()
        {
            return ModeHashcodeArray;
        }


    }
}
