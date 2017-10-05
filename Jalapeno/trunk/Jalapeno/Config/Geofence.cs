using System;
using System.Collections.Generic;
using ZylSerialPort;
using Jalapeno.Utils;

namespace Jalapeno.Config
{
    //---------------------------------------------------------------------------------------------
    // GEOFENCE
    //---------------------------------------------------------------------------------------------
    // Latitude and longitude are U32, Fixed-point integer of degrees latitude/longitude 
    // scaled by 1,000,000 and offset by +90.0
    // Use 0xFFFFFFFF for undefined value
    //
    //Total Size of a single config = 25 bytes;
    //---------------------------------------------------------------------------------------------
    // ...
    //---------------------------------------------------------------------------------------------

    public class Geofence
    {
        public String GeofenceName { get; set; }
        public enum GeofenceTypeEnum { GEOFENCE_DISABLED = 0, GEOFENCE_CIRCULAR = 1, GEOFENCE_RECTANGLE = 2 };
        public GeofenceTypeEnum GeofenceType { get; set; }
        public Location GeofenceLocation { get; set; }
        private Queue<byte[]> GeofenceArrayQueue;

        public static byte ConfigLength;
        public static UInt16 ConfigLocation;


        public Geofence()
        {
            GeofenceArrayQueue = new Queue<byte[]>();
            GeofenceLocation = new Location();
            GeofenceName = "Default";

            ConfigLength = 25;
            ConfigLocation = 276;
        }
        public Geofence(String GeoName, GeofenceTypeEnum GeoType, Location GeoLocation)
        {
            GeofenceArrayQueue = new Queue<byte[]>();
            GeofenceName = GeoName;
            GeofenceType = GeoType;
            GeofenceLocation = GeoLocation;
            ConfigLength = 25;
            ConfigLocation = 276;
        }

        public byte[] ToByteArray()
        {
            //Limit the name array to 8 bytes, and add to queue
            byte[] NameArray = new byte[8];
            NameArray = Tools.StringToByteArray(NameArray, GeofenceName);
            GeofenceArrayQueue.Enqueue(NameArray);

            //Add geofence type and location to queue. 
            GeofenceArrayQueue.Enqueue(Tools.IntToSingleByteArray((int)GeofenceType));
            GeofenceArrayQueue.Enqueue(BitConverter.GetBytes(GeofenceLocation.Lat_North));
            GeofenceArrayQueue.Enqueue(BitConverter.GetBytes(GeofenceLocation.Lat_South));
            GeofenceArrayQueue.Enqueue(BitConverter.GetBytes(GeofenceLocation.Long_West));
            GeofenceArrayQueue.Enqueue(BitConverter.GetBytes(GeofenceLocation.Long_East));

            //Compile queue into complete byte array;
            byte[] GeofenceByteArray = Tools.CreateByteArrayFromQueue(GeofenceArrayQueue);

            return GeofenceByteArray;
        }

        public void readGeofence(byte[] MsgReceived, int pos)
        {
            GeofenceName = SerialPort.ASCIIByteArrayToString(Tools.ArrayFromArray(MsgReceived, pos, pos + 7)); 
            pos += 8;
            GeofenceType = (GeofenceTypeEnum)MsgReceived[pos];
            pos++;
            GeofenceLocation.UpdateLocation(BitConverter.ToUInt32(MsgReceived, pos), BitConverter.ToUInt32(MsgReceived, pos + 4), BitConverter.ToUInt32(MsgReceived, pos + 8), BitConverter.ToUInt32(MsgReceived, pos + 12));
        }

    }
    public class Location
    {
        public UInt32 Lat_North { get; set; }
        public UInt32 Lat_South { get; set; }
        public UInt32 Long_West { get; set; }
        public UInt32 Long_East { get; set; }

        public Location()
        {
            Lat_North = 0;
            Lat_South = 0;
            Long_West = 0;
            Long_East = 0;
        }

        public Location(double Lat_N, double Lat_S, double Long_W, double Long_E)
        {
            Lat_North = (UInt32) ((Lat_N + 90) * 1e6);
            Lat_South = (UInt32) ((Lat_S + 90) * 1e6);
            Long_West = (UInt32) ((Long_W + 180) * 1e6);
            Long_East = (UInt32) ((Long_E + 180) * 1e6);
        }
        

        public Location(UInt32 Lat_N, UInt32 Lat_S, UInt32 Long_W, UInt32 Long_E)
        {
            Lat_North = Lat_N;
            Lat_South = Lat_S;
            Long_West = Long_W;
            Long_East = Long_E;
        }

        public void UpdateLocation(UInt32 Lat_N, UInt32 Lat_S, UInt32 Long_W, UInt32 Long_E)
        {
            Lat_North = Lat_N;
            Lat_South = Lat_S;
            Long_West = Long_W;
            Long_East = Long_E;
        }
    }
}