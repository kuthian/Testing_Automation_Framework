using System;
using System.Collections.Generic;
using Jalapeno.Utils;

namespace Jalapeno.Config
{
    public class Cell
    {
        private byte[] APN;
        private byte[] Username;
        private byte[] Password;
        private byte[] ServerAddress;
        private byte[] ServerPort;
        private byte[] ServerEncryptionKey;
        public byte[] SmsArray;
        private String[] SmsNumbers;
        private Queue<byte[]> CellArrayQueue;

        public static byte ConfigLength;
        public static UInt16 ConfigLocation;

        public Cell()
        {
            APN = new byte[30];
            Username= new byte[20];
            Password = new byte[20];
            ServerAddress = new byte[50];
            ServerPort = new byte[2];
            ServerEncryptionKey = new byte[32];
            SmsArray = new byte[32];
            SmsNumbers = new String[4];
            CellArrayQueue = new Queue<byte[]>();

            setSmsNumbers(null,null,null,null);

            ConfigLength = 186;
            ConfigLocation = 622;
        }

        public byte[] ToByteArray()
        {
            CellArrayQueue.Enqueue(APN);
            CellArrayQueue.Enqueue(Username);
            CellArrayQueue.Enqueue(Password);
            CellArrayQueue.Enqueue(ServerAddress);
            CellArrayQueue.Enqueue(ServerPort);
            CellArrayQueue.Enqueue(ServerEncryptionKey);
            CellArrayQueue.Enqueue(SmsArray);

            return Tools.CreateByteArrayFromQueue(CellArrayQueue);

        }

        public void  readCell(byte[] MsgReceived, int pos)
        {
            APN = Tools.ArrayFromArray(MsgReceived, pos, pos + 29);
            pos += 30;
            Username = Tools.ArrayFromArray(MsgReceived, pos, pos + 19);
            pos += 20;
            Password = Tools.ArrayFromArray(MsgReceived, pos, pos + 19);
            pos += 20;
            ServerAddress = Tools.ArrayFromArray(MsgReceived, pos, pos + 49);
            pos += 50;
            ServerPort = Tools.ArrayFromArray(MsgReceived, pos, pos + 1);
            pos += 2;
            ServerEncryptionKey = Tools.ArrayFromArray(MsgReceived, pos, pos + 31);
            pos += 32;
            SmsArray = Tools.ArrayFromArray(MsgReceived, pos, pos + 31);
            pos += 32;
        }

        public void setSmsArray()
        {
            foreach (String element in SmsNumbers)
            {
                if (element != null)
                {
                    CellArrayQueue.Enqueue(Tools.StringToBCD(element));
                }
                else
                {
                    CellArrayQueue.Enqueue(Tools.FullArray(8));
                }
            }

            SmsArray = Tools.CreateByteArrayFromQueue(CellArrayQueue);
        }

        public void setSmsNumbers(String Number1, String Number2, String Number3, String Number4)
        {
            
            SmsNumbers[0] = Number1;
            SmsNumbers[1] = Number2;
            SmsNumbers[2] = Number3;
            SmsNumbers[3] = Number4;

            setSmsArray();
        }

        public void setAPN(String ApnName)
        {
            Array.Clear(APN, 0, APN.Length);
            APN = Tools.StringToByteArray(APN, ApnName);
        }

        public void setUsername(String UserName)
        {
            Array.Clear(Username, 0, Username.Length);
            Username = Tools.StringToByteArray(Username, UserName);
        }

        public void setPassword(String PassName)
        {
            Array.Clear(Password, 0, Password.Length);
            Password = Tools.StringToByteArray(Password, PassName);
        }

        public void setServerAddress(String ServerAddressName)
        {
            Array.Clear(ServerAddress, 0, ServerAddress.Length);
            ServerAddress = Tools.StringToByteArray(ServerAddress, ServerAddressName);
        }

        public void setServerPort(UInt16 PortNumber)
        {
            ServerPort = BitConverter.GetBytes(PortNumber);
        }

        public void setServerEncryptionKey(byte[] NewEncryptionKey)
        {
            ServerEncryptionKey = NewEncryptionKey;
        }
        
        
    }
}
