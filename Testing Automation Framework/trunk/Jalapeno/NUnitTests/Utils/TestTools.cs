using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Jalapeno.Config;
using Jalapeno.Messaging.Messages;
using Jalapeno.Messaging;
using Jalapeno;
using NUnit.Framework;

namespace NUnitTests.Utils
{
    class TestTools
    {
        public static void ReadConfigWaitAssertIsEqual(object Expected, object Actual)
        {
            Debug.WriteLine("Doing ReadConfigWaitAssert");

            int Count = 0;

            while (!FullConfiguration.ConfigIsCurrent && Count < 10)
            {
                Program.Session1.ReadConfig();
                Thread.Sleep(4000);
                Debug.WriteLine("CurrentConfig Flag is " + FullConfiguration.ConfigIsCurrent);
                Count++;
            }

            Debug.WriteLine("CurrentConfig Flag is " + FullConfiguration.ConfigIsCurrent);

            Count = 0;

            while (Expected != Actual && Count < 10)
            {
                Thread.Sleep(500);
                Count++;
            }

            Debug.WriteLine("Device is still connected");

            Assert.AreEqual(Expected, Actual); 
        }

        public static void ReadConfigWaitAssertIsTrue(bool ExpectedToBeTrue)
        {
            Debug.WriteLine("Doing ReadConfigWaitAssert");
             
            int Count = 0;

            Debug.WriteLine("Expected byte array :            " + BitConverter.ToString(FullConfiguration.NewConfigItems.NewConfigCell.ToByteArray()));
            Debug.WriteLine("Stored byte array before config: " + BitConverter.ToString(Program.Session1.TG300.CurrentConfiguration.ConfigCell.ToByteArray()));

            while (!FullConfiguration.ConfigIsCurrent && Count < 10)
            {
                Program.Session1.ReadConfig();
                Thread.Sleep(4000);
                Debug.WriteLine("CurrentConfig Flag is " + FullConfiguration.ConfigIsCurrent);
                Count++;
            }

            Debug.WriteLine("CurrentConfig Flag is " + FullConfiguration.ConfigIsCurrent);

            Count = 0;

            while (!ExpectedToBeTrue && Count < 20)
            {
                Thread.Sleep(500);
                Count++;
                Debug.WriteLine("Assert Flag is " + ExpectedToBeTrue);
            }

            Debug.WriteLine("Expected byte array:            " + BitConverter.ToString(FullConfiguration.NewConfigItems.NewConfigCell.ToByteArray()));
            Debug.WriteLine("Stored byte array after config: " + BitConverter.ToString(Program.Session1.TG300.CurrentConfiguration.ConfigCell.ToByteArray()));

            Assert.IsTrue(ExpectedToBeTrue);
        }

        public static void WaitAssert(object Expected, object Actual)
        {
            int Count = 0;
            while (Expected != Actual && Count < 10)
            {
                Thread.Sleep(500);
                Count++;
            }

        }

        public static void ReadConfig()
        {
            Debug.WriteLine("Doing ReadConfig");

            int Count = 0;

            while (!FullConfiguration.ConfigIsCurrent && Count < 10)
            {
                Program.Session1.ReadConfig();
                Thread.Sleep(5000);
                Debug.WriteLine("CurrentConfig Flag is " + FullConfiguration.ConfigIsCurrent);
                Count++;
            }

            Debug.WriteLine("CurrentConfig Flag is " + FullConfiguration.ConfigIsCurrent);
        }

        public static void WriteDefault()
        {
            TestTools.ReadConfig();

            Mode DefaultMode = new Mode();

            DefaultMode.SetGpsFixInterval(120);
            DefaultMode.setOperationFlags(Mode.GPSModeEnum.MOTION_ONLY, false, true, Mode.CellModeEnum.STANDBY);
            DefaultMode.SetMailboxCheckInterval_NoMotion(300);
            DefaultMode.SetMailboxCheckInterval_Motion(120);
            DefaultMode.SetTransmitInterval(120);

            FullConfiguration.NewConfigItems.NewConfigMode0 = DefaultMode;

            FullConfiguration.NewConfigItems.NewConfigTimefence0.TimefenceType = Timefence.TimefenceTypeEnum.TIMEFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigTimefence1.TimefenceType = Timefence.TimefenceTypeEnum.TIMEFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigTimefence2.TimefenceType = Timefence.TimefenceTypeEnum.TIMEFENCE_DISABLED;

            FullConfiguration.NewConfigItems.NewConfigGeofence0.GeofenceType = Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigGeofence1.GeofenceType = Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigGeofence2.GeofenceType = Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigGeofence3.GeofenceType = Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigGeofence4.GeofenceType = Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigGeofence5.GeofenceType = Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigGeofence6.GeofenceType = Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigGeofence7.GeofenceType = Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigGeofence8.GeofenceType = Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED;
            FullConfiguration.NewConfigItems.NewConfigGeofence9.GeofenceType = Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED;


            FullConfiguration.NewConfigItems.NewConfigCell.setAPN("");
            FullConfiguration.NewConfigItems.NewConfigCell.setPassword("");

            FullConfiguration.NewConfigItems.NewConfigCell.setSmsNumbers(null, null, null, null);

            Program.Session1.WriteMultiConfig(MultiConfiguration.ConfigurationItemsEnum.DEFAULT_MODE, MultiConfiguration.ConfigurationItemsEnum.CELL_CONFIGURATION, MultiConfiguration.ConfigurationItemsEnum.TIMEFENCE_0, MultiConfiguration.ConfigurationItemsEnum.TIMEFENCE_1, MultiConfiguration.ConfigurationItemsEnum.TIMEFENCE_2, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_0, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_1, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_2, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_3, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_4, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_5, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_6, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_7, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_8, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_9);
        }

    }
}
