using System;
using Jalapeno.Utils;

namespace Jalapeno.Config
{
    public class Iridium
    {
        private byte[] EncryptionKey;

        public static byte ConfigLength;
        public static UInt16 ConfigLocation;

        public Iridium()
        {
            EncryptionKey = new byte[32];

            ConfigLength = 32;
            ConfigLocation = 590;
        }

        public Iridium(byte[] NewEncryptionKey)
        {
            EncryptionKey = new byte[32];
            EncryptionKey = NewEncryptionKey;

            ConfigLength = 32;
            ConfigLocation = 590;
        }
        public void readIridium(byte[] MsgReceived, int pos)
        {
            EncryptionKey = Tools.ArrayFromArray(MsgReceived, pos, pos + 31); 
        }

        public void UpdateIridiumEncryptionKey(byte[] NewEncryptionKey)
        {
            EncryptionKey = NewEncryptionKey;
        }

        public byte[] ToByteArray()
        {
            return EncryptionKey;
        }
    }
}
