using System;
using System.Collections.Generic;
using ZylSerialPort;

namespace Jalapeno.Utils
{
    class Tools
    {
        ///<summary>
        ///<para> Returns the lower byte of a UInt16 </para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte getLowByte(UInt16 Value)
        {
            return (byte) (Value & 0xFF);
        }

        ///<summary>
        ///<para> Returns the lower byte of a UInt16 </para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte getHighByte(UInt16 Value)
        {
            return (byte) ((Value >> 8) & 0xFF);
        }

        ///<summary>
        ///<para> Creates byte[] from all queued byte[] in Queue</para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte[] CreateByteArrayFromQueue(Queue<byte[]> ByteArrayQueue)
        {
            var ByteArrayList = new List<byte>();
            int QueueCount = ByteArrayQueue.Count;
            for (int i = 0 ; i < QueueCount; i++)
            {
                ByteArrayList.AddRange(ByteArrayQueue.Dequeue());
            }

            byte[] newArray = ByteArrayList.ToArray();

            ByteArrayQueue.Clear();
            return newArray;
        }

        ///<summary>
        ///<para> Takes byte and converts it to a byte[1] array </para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte[] ByteToSingleByteArray(byte SingleByte)
        {
            byte[] SingleByteArray = new byte[1];
            SingleByteArray[0] = SingleByte;

            return SingleByteArray;
        }

        ///<summary>
        ///<para> Takes int and converts it to a byte[1] array </para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte[] IntToSingleByteArray(int SingleInt)
        {
            byte[] SingleByteArray = new byte[1];
            SingleByteArray[0] = (byte) SingleInt;

            return SingleByteArray;
        }
        ///<summary>
        ///<para> Takes Big Endian byte[] and converts it to a Little Endian byte[]</para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte[] SwitchEndian(byte[] BigEndianByteArray)
        {
            byte[] LittleEndianByteArray = new byte[BigEndianByteArray.Length];

            for (int i = 0; i < BigEndianByteArray.Length; i++)
            {
                LittleEndianByteArray[i] = BigEndianByteArray[BigEndianByteArray.Length - 1 - i];
            }
            return LittleEndianByteArray;
        }

        ///<summary>
        ///<para> Clears specified bit in specified byte</para>
        ///</summary>
        //////<returns>byte value</returns>
        public static byte ClearBit(byte CurrentByte, int Position)
        {
            int NewByte = Convert.ToInt32(CurrentByte);
            NewByte &= ~(1 << Position);
            return Convert.ToByte(NewByte);
        }

        ///<summary>
        ///<para> Sets specified bit in specified byte</para>
        ///</summary>
        //////<returns>byte value</returns>
        public static byte SetBit(byte CurrentByte, int Position)
        {
            int NewByte = Convert.ToInt32(CurrentByte);
            NewByte |= (1 << Position);
            return Convert.ToByte(NewByte);
        }


        ///<summary>
        ///<para> Places an integer into the low nibble of a byte </para>
        ///</summary>
        //////<returns>byte value</returns>
        public static void IntToLowNibble(byte LowNibble, int IntToConvert)
        {
            if (IntToConvert == 0)
            {
                LowNibble = Tools.ClearBit(LowNibble, 0);
                LowNibble = Tools.ClearBit(LowNibble, 1);
                LowNibble = Tools.ClearBit(LowNibble, 2);
                LowNibble = Tools.ClearBit(LowNibble, 3);
            }
            else if (IntToConvert == 1)
            {
                LowNibble = Tools.SetBit(LowNibble, 0);
                LowNibble = Tools.ClearBit(LowNibble, 1);
                LowNibble = Tools.ClearBit(LowNibble, 2);
                LowNibble = Tools.ClearBit(LowNibble, 3);
            }
            else if (IntToConvert == 2)
            {
                LowNibble = Tools.ClearBit(LowNibble, 0);
                LowNibble = Tools.SetBit(LowNibble, 1);
                LowNibble = Tools.ClearBit(LowNibble, 2);
                LowNibble = Tools.ClearBit(LowNibble, 3);
            }
            else if (IntToConvert == 3)
            {
                LowNibble = Tools.SetBit(LowNibble, 0);
                LowNibble = Tools.SetBit(LowNibble, 1);
                LowNibble = Tools.ClearBit(LowNibble, 2);
                LowNibble = Tools.ClearBit(LowNibble, 3);
            }
            else if (IntToConvert == 4)
            {
                LowNibble = Tools.ClearBit(LowNibble, 0);
                LowNibble = Tools.ClearBit(LowNibble, 1);
                LowNibble = Tools.SetBit(LowNibble, 2);
                LowNibble = Tools.ClearBit(LowNibble, 3);
            }
            else if (IntToConvert == 5)
            {
                LowNibble = Tools.SetBit(LowNibble, 0);
                LowNibble = Tools.ClearBit(LowNibble, 1);
                LowNibble = Tools.SetBit(LowNibble, 2);
                LowNibble = Tools.ClearBit(LowNibble, 3);
            }
            else if (IntToConvert == 6)
            {
                LowNibble = Tools.ClearBit(LowNibble, 0);
                LowNibble = Tools.SetBit(LowNibble, 1);
                LowNibble = Tools.SetBit(LowNibble, 2);
                LowNibble = Tools.ClearBit(LowNibble, 3);
            }
            else if (IntToConvert == 7)
            {
                LowNibble = Tools.SetBit(LowNibble, 0);
                LowNibble = Tools.SetBit(LowNibble, 1);
                LowNibble = Tools.SetBit(LowNibble, 2);
                LowNibble = Tools.ClearBit(LowNibble, 3);
            }
            else if (IntToConvert == 8)
            {
                LowNibble = Tools.ClearBit(LowNibble, 0);
                LowNibble = Tools.ClearBit(LowNibble, 1);
                LowNibble = Tools.ClearBit(LowNibble, 2);
                LowNibble = Tools.SetBit(LowNibble, 3);
            }
            else if (IntToConvert == 9)
            {
                LowNibble = Tools.SetBit(LowNibble, 0);
                LowNibble = Tools.ClearBit(LowNibble, 1);
                LowNibble = Tools.ClearBit(LowNibble, 2);
                LowNibble = Tools.SetBit(LowNibble, 3);
            }
            else if (IntToConvert == 10)
            {
                LowNibble = Tools.ClearBit(LowNibble, 0);
                LowNibble = Tools.SetBit(LowNibble, 1);
                LowNibble = Tools.ClearBit(LowNibble, 2);
                LowNibble = Tools.SetBit(LowNibble, 3);
            }
            else if (IntToConvert == 11)
            {
                LowNibble = Tools.SetBit(LowNibble, 0);
                LowNibble = Tools.SetBit(LowNibble, 1);
                LowNibble = Tools.ClearBit(LowNibble, 2);
                LowNibble = Tools.SetBit(LowNibble, 3);
            }
            else if (IntToConvert == 12)
            {
                LowNibble = Tools.ClearBit(LowNibble, 0);
                LowNibble = Tools.ClearBit(LowNibble, 1);
                LowNibble = Tools.SetBit(LowNibble, 2);
                LowNibble = Tools.SetBit(LowNibble, 3);
            }
            else if (IntToConvert == 13)
            {
                LowNibble = Tools.SetBit(LowNibble, 0);
                LowNibble = Tools.ClearBit(LowNibble, 1);
                LowNibble = Tools.SetBit(LowNibble, 2);
                LowNibble = Tools.SetBit(LowNibble, 3);
            }
            else if (IntToConvert == 14)
            {
                LowNibble = Tools.ClearBit(LowNibble, 0);
                LowNibble = Tools.SetBit(LowNibble, 1);
                LowNibble = Tools.SetBit(LowNibble, 2);
                LowNibble = Tools.SetBit(LowNibble, 3);
            }
            else if (IntToConvert == 15)
            {
                LowNibble = Tools.SetBit(LowNibble, 0);
                LowNibble = Tools.SetBit(LowNibble, 1);
                LowNibble = Tools.SetBit(LowNibble, 2);
                LowNibble = Tools.SetBit(LowNibble, 3);
            }
        }

        ///<summary>
        ///<para> Places an integer into the high nibble of a byte </para>
        ///</summary>
        //////<returns>byte value</returns>
        public static void IntToHighNibble(byte HighNibble, int IntToConvert)
        {
            if (IntToConvert == 0)
            {
                HighNibble = Tools.ClearBit(HighNibble, 4);
                HighNibble = Tools.ClearBit(HighNibble, 5);
                HighNibble = Tools.ClearBit(HighNibble, 6);
                HighNibble = Tools.ClearBit(HighNibble, 7);
            }
            else if (IntToConvert == 1)
            {
                HighNibble = Tools.SetBit(HighNibble, 4);
                HighNibble = Tools.ClearBit(HighNibble, 5);
                HighNibble = Tools.ClearBit(HighNibble, 6);
                HighNibble = Tools.ClearBit(HighNibble, 7);
            }
            else if (IntToConvert == 2)
            {
                HighNibble = Tools.ClearBit(HighNibble, 4);
                HighNibble = Tools.SetBit(HighNibble, 5);
                HighNibble = Tools.ClearBit(HighNibble, 6);
                HighNibble = Tools.ClearBit(HighNibble, 7);
            }
            else if (IntToConvert == 3)
            {
                HighNibble = Tools.SetBit(HighNibble, 4);
                HighNibble = Tools.SetBit(HighNibble, 5);
                HighNibble = Tools.ClearBit(HighNibble, 6);
                HighNibble = Tools.ClearBit(HighNibble, 7);
            }
            else if (IntToConvert == 4)
            {
                HighNibble = Tools.ClearBit(HighNibble, 4);
                HighNibble = Tools.ClearBit(HighNibble, 5);
                HighNibble = Tools.SetBit(HighNibble, 6);
                HighNibble = Tools.ClearBit(HighNibble, 7);
            }
            else if (IntToConvert == 5)
            {
                HighNibble = Tools.SetBit(HighNibble, 4);
                HighNibble = Tools.ClearBit(HighNibble, 5);
                HighNibble = Tools.SetBit(HighNibble, 6);
                HighNibble = Tools.ClearBit(HighNibble, 7);
            }
            else if (IntToConvert == 6)
            {
                HighNibble = Tools.ClearBit(HighNibble, 4);
                HighNibble = Tools.SetBit(HighNibble, 5);
                HighNibble = Tools.SetBit(HighNibble, 6);
                HighNibble = Tools.ClearBit(HighNibble, 7);
            }
            else if (IntToConvert == 7)
            {
                HighNibble = Tools.SetBit(HighNibble, 4);
                HighNibble = Tools.SetBit(HighNibble, 5);
                HighNibble = Tools.SetBit(HighNibble, 6);
                HighNibble = Tools.ClearBit(HighNibble, 7);
            }
            else if (IntToConvert == 8)
            {
                HighNibble = Tools.ClearBit(HighNibble, 4);
                HighNibble = Tools.ClearBit(HighNibble, 5);
                HighNibble = Tools.ClearBit(HighNibble, 6);
                HighNibble = Tools.SetBit(HighNibble, 7);
            }
            else if (IntToConvert == 9)
            {
                HighNibble = Tools.SetBit(HighNibble, 4);
                HighNibble = Tools.ClearBit(HighNibble, 5);
                HighNibble = Tools.ClearBit(HighNibble, 6);
                HighNibble = Tools.SetBit(HighNibble, 7);
            }
            else if (IntToConvert == 10)
            {
                HighNibble = Tools.ClearBit(HighNibble, 4);
                HighNibble = Tools.SetBit(HighNibble, 5);
                HighNibble = Tools.ClearBit(HighNibble, 6);
                HighNibble = Tools.SetBit(HighNibble, 7);
            }
            else if (IntToConvert == 11)
            {
                HighNibble = Tools.SetBit(HighNibble, 4);
                HighNibble = Tools.SetBit(HighNibble, 5);
                HighNibble = Tools.ClearBit(HighNibble, 6);
                HighNibble = Tools.SetBit(HighNibble, 7);
            }
            else if (IntToConvert == 12)
            {
                HighNibble = Tools.ClearBit(HighNibble, 4);
                HighNibble = Tools.ClearBit(HighNibble, 5);
                HighNibble = Tools.SetBit(HighNibble, 6);
                HighNibble = Tools.SetBit(HighNibble, 7);
            }
            else if (IntToConvert == 13)
            {
                HighNibble = Tools.SetBit(HighNibble, 4);
                HighNibble = Tools.ClearBit(HighNibble, 5);
                HighNibble = Tools.SetBit(HighNibble, 6);
                HighNibble = Tools.SetBit(HighNibble, 7);
            }
            else if (IntToConvert == 14)
            {
                HighNibble = Tools.ClearBit(HighNibble, 4);
                HighNibble = Tools.SetBit(HighNibble, 5);
                HighNibble = Tools.SetBit(HighNibble, 6);
                HighNibble = Tools.SetBit(HighNibble, 7);
            }
            else if (IntToConvert == 15)
            {
                HighNibble = Tools.SetBit(HighNibble, 4);
                HighNibble = Tools.SetBit(HighNibble, 5);
                HighNibble = Tools.SetBit(HighNibble, 6);
                HighNibble = Tools.SetBit(HighNibble, 7);
            }
        }

        ///<summary>
        ///<para> Create a BCD byte[] from a given string </para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte[] StringToBCD(String StringToConvert)
        {
            byte[] BCDArray = Tools.FullArray(8);
            var StringChars = StringToConvert.ToCharArray();

            int Count = 0;
            int ByteCount = 0;

            foreach(char element in StringChars)
            {                
                if ((Count%2 == 0) && (Count !=1))
                {
                    Tools.IntToHighNibble(BCDArray[ByteCount], element - '0');

                }
                else
                {

                    Tools.IntToLowNibble(BCDArray[ByteCount], element - '0');
                    ByteCount++;
                }

                Count++;
            }
            return BCDArray;
        }

        ///<summary>
        ///<para> Creates a truncated array that includes IndexEnd </para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte[] ArrayFromArray(byte[] OriginalArray, int IndexStart, int IndexEnd)
        {
            int NewArrayLength = IndexEnd-IndexStart + 1;
            byte[] NewArray = new byte[NewArrayLength];

            for(int i = 0; i < NewArrayLength; i++)
            {
                NewArray[i] = OriginalArray[IndexStart+i];
            }

            return NewArray;
        }

        ///<summary>
        ///<para> Adds String to the beginning of an existing Array</para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte[] StringToByteArray(byte[] OriginalArray, String StringToCopy)
        {
            byte[] StringToCopyArray = SerialPort.StringToASCIIByteArray(StringToCopy);

            for (int i = 0; i < StringToCopyArray.Length ; i++)
            {
                OriginalArray[i] = StringToCopyArray[i];
            }

            return OriginalArray;       
        }

        ///<summary>
        ///<para> Creates a Zero array of size ArraySize </para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte[] EmptyArray(int ArraySize)
        {
            byte[] ZeroArray = new byte[ArraySize];

            return ZeroArray;
        }

        ///<summary>
        ///<para> Creates a 0xFF array of size ArraySize </para>
        ///</summary>
        //////<returns>byte[] value</returns>
        public static byte[] FullArray(int ArraySize)
        {
            byte[] FFArray = new byte[ArraySize];

            for (int i = 0; i < FFArray.Length; i++)
            {
                FFArray[i] = 0xFF;
            }

            return FFArray;
        }

    }
}
