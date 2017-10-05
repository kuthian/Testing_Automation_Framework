using System;
using Jalapeno.Utils;

namespace Jalapeno.Config
{
    public class IOSettings
    {
        public enum IOFlagsEnum { AUTOMATIC = 0, MANUAL}
        public IOFlagsEnum IOFlags;
        public int NumInputs { get; set; }
        public int NumOutputs { get; set; }
        public byte[] IOSettingsArray;

        public static byte ConfigLength;
        public static UInt16 ConfigLocation;

        //default constructor
        public IOSettings()
        {
            IOSettingsArray = new byte[2];
            NumInputs = 0;
            NumOutputs = 0;

            ConfigLength = 2;
            ConfigLocation = 577;

        }

        public IOSettings(IOFlagsEnum IOFlag, int NumberOfInputs, int NumberOfOutputs)
        {
            IOSettingsArray = new byte[2];

            IOFlags = IOFlag;
            NumInputs = NumberOfInputs;
            NumOutputs = NumberOfOutputs;

            IOSettingsArray[0] = (byte) IOFlag;
            Tools.IntToLowNibble(IOSettingsArray[1], NumberOfInputs);
            Tools.IntToHighNibble(IOSettingsArray[1], NumberOfOutputs);

            ConfigLength = 2;
            ConfigLocation = 577;
        }

        public void readIOSettings(byte[] MsgReceived, int pos)
        {
            IOSettingsArray[0] =  MsgReceived[pos];
            IOFlags = (IOFlagsEnum) IOSettingsArray[0];
            pos++;
            IOSettingsArray[1] = MsgReceived[pos];
            NumInputs = (IOSettingsArray[1] & 0x0F);
            NumOutputs = (IOSettingsArray[1] & 0xF0) >> 4;

            IOSettingsArray[0] = (byte) IOFlags;
            Tools.IntToLowNibble(IOSettingsArray[1], NumInputs);
            Tools.IntToHighNibble(IOSettingsArray[1], NumOutputs);
      
        }

        // created to keep consistency throughout config namespace
        public byte[] ToByteArray()
        {
            IOSettingsArray[0] = (byte) IOFlags;
            Tools.IntToLowNibble(IOSettingsArray[1], NumInputs);
            Tools.IntToHighNibble(IOSettingsArray[1], NumOutputs);

            return IOSettingsArray;
        }
    }
}
