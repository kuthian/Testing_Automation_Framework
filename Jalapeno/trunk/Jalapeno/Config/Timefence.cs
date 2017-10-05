using System;
using System.Collections.Generic;
using ZylSerialPort;
using Jalapeno.Utils;

namespace Jalapeno.Config
{
    //---------------------------------------------------------------------------------------------
    // TIMESTAMPS
    //---------------------------------------------------------------------------------------------
    //All date and time values in the system are reported as UTC seconds since January 6, 1980. 
    //This UTC value currently accounts for leap seconds up to and including the June 30th 2015 update.
    //Examples:
    //01-January-2000 00:00:00 = 630720000
    //01-January-2001 00:00:00 = 662342400
    //01-January-2013 00:00:00 = 1041033600
    //Total Size of a single Timefence config = 17 bytes;
    //---------------------------------------------------------------------------------------------
    // ...
    //---------------------------------------------------------------------------------------------

    public class Timefence
    {
        public String TimefenceName;
        public enum TimefenceTypeEnum { TIMEFENCE_DISABLED = 0, TIMEFENCE_START_AND_END, TIMEFENCE_DAILY_SCHEDULE, TIMEFENCE_WEEKLY_SCHEDULE};
        public TimefenceTypeEnum TimefenceType;
        public Time StartTime;
        public Time EndTime;
        private Queue<byte[]> TimefenceArrayQueue;

        public static byte ConfigLength;
        public static UInt16 ConfigLocation;

        public Timefence()
        {
            TimefenceArrayQueue = new Queue<byte[]>();
            TimefenceName = "Default";
            TimefenceType = TimefenceTypeEnum.TIMEFENCE_DISABLED;
            StartTime = new Time();
            EndTime = new Time();

            ConfigLength = 17;
            ConfigLocation = 526;
        }
        public Timefence(String TimeName, TimefenceTypeEnum TimeType, Time Start, Time End)
        {
            TimefenceArrayQueue = new Queue<byte[]>();
            TimefenceName = TimeName;
            TimefenceType = TimeType;
            StartTime = Start;
            EndTime =  End;
        }

        public byte[] ToByteArray()
        {
            //Limit the name array to 8 bytes
            byte[] NameArray = new byte[8];

            NameArray = Tools.StringToByteArray(NameArray, TimefenceName); //error when Timefence name is less than 8 chars

            TimefenceArrayQueue.Enqueue(NameArray);
                       
            //Add timefence type, Start time, and End time to queue. 
            TimefenceArrayQueue.Enqueue(Tools.IntToSingleByteArray( (int) TimefenceType));
            

            TimefenceArrayQueue.Enqueue(BitConverter.GetBytes(StartTime.TimeInSeconds));
            
            TimefenceArrayQueue.Enqueue(BitConverter.GetBytes(EndTime.TimeInSeconds));
            

            //Compile queue into complete byte array;
            byte[] TimefenceByteArray = Tools.CreateByteArrayFromQueue(TimefenceArrayQueue);

            return TimefenceByteArray;
        }

        public void readTimefence(byte[] MsgReceived, int pos)
        {
            TimefenceName = SerialPort.ASCIIByteArrayToString(Tools.ArrayFromArray(MsgReceived, pos, pos + 7)); 
            //Debug.WriteLine("Timefence Name: " + SerialPort.ASCIIByteArrayToString(Tools.ArrayFromArray(MsgReceived, pos, pos + 7)) + ", Name Array: " + BitConverter.ToString(Tools.ArrayFromArray(MsgReceived, pos, pos + 7))+ "\n");
            pos += 8;
            TimefenceType = (TimefenceTypeEnum) MsgReceived[pos];
            pos++;
            StartTime.UpdateTimeInSeconds(BitConverter.ToUInt32(MsgReceived, pos));
            pos += 4;
            EndTime.UpdateTimeInSeconds(BitConverter.ToUInt32(MsgReceived, pos));

            //Debug.WriteLine("Timefence Type: " + BitConverter.ToString(Tools.IntToSingleByteArray((int)TimefenceType)));
            //Debug.WriteLine("Start Time: " + StartTime.Hour + ":" + StartTime.Minute);
            //Debug.WriteLine("End Time: " + EndTime.Hour + ":" + EndTime.Minute + "\n");
        }


    }

    public class Time
    {
        public UInt32 TimeInSeconds;
        public int Hour;
        public int Minute; 
        public enum WeekdayEnum {SUNDAY = 0, MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY};
        private static DateTime BaseTime = new DateTime(1980, 1, 6);

        public Time()
        {
            TimeInSeconds = 0;
            Hour = 0;
            Minute = 0;
        }

        public Time (DateTime NewDate)
        {
            Hour = NewDate.Hour;
            Minute = NewDate.Minute;

            TimeInSeconds = (UInt32)((NewDate.Ticks - BaseTime.Ticks) / 10e6);
        }

        public Time(int Hr, int Min, int Year, int Month, int Day)
        {
            DateTime NewDate = new DateTime(Year, Month, Day, Hr, Min, 0);

            Hour = Hr;
            Minute = Min;

            TimeInSeconds = (UInt32) ((NewDate.Ticks - BaseTime.Ticks) / 10e6);
        }


        public Time(int Hr, int Min, WeekdayEnum Day)
        {
            Hour = Hr;
            Minute = Min;
            TimeInSeconds = 0;
            TimeInSeconds += (uint) Hour*3600; // seconds in an hour
            TimeInSeconds += (uint) Min * 60; // seconds in a minute
            TimeInSeconds += (uint) ((int) Day) * 86400; //seconds in a day
        }

        public Time(int Hr, int Min)
        {
            Hour = Hr;
            Minute = Min;

            TimeInSeconds = 0;
            TimeInSeconds += (uint)Hour * 3600; // seconds in an hour
            TimeInSeconds += ((uint)(Min * 60)); // seconds in a minute
            TimeInSeconds += 1187578800;
        }

        public void UpdateTimeInSeconds(UInt32 Seconds)
        {
            TimeInSeconds = Seconds;
        }
    }
}
