using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Jalapeno.Config;
using Jalapeno.Messaging;
using Jalapeno.Messaging.Messages;
using Jalapeno;
using System.Diagnostics;
using NUnitTests.Utils;
using ZylSerialPort;


namespace NUnitTests.JalapenoSmokeTest
{
    [TestFixture(Category = "Jalapeno")]
    class WriteConfigSmokeTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            TestTools.WriteDefault();
        }

        [SetUp]
        public void RunBeforeAnyTests()
        {
            TestTools.ReadConfig();
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
            //TestTools.WaitAssert(true, Program.CurrentSession.TG300.Connected);
        }


        [Test]
        public void CanWriteMode()
        {
            FullConfiguration.NewConfigItems.NewConfigMode0.setOperationFlags(Mode.GPSModeEnum.MOTION_ONLY, false, true, Mode.CellModeEnum.STANDBY);
            FullConfiguration.NewConfigItems.NewConfigMode0.SetGpsFixInterval(300);
            FullConfiguration.NewConfigItems.NewConfigMode0.SetMailboxCheckInterval_NoMotion(300);
            FullConfiguration.NewConfigItems.NewConfigMode0.SetMailboxCheckInterval_Motion(600);
            FullConfiguration.NewConfigItems.NewConfigMode0.SetTransmitInterval(300);

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.DEFAULT_MODE);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigMode0.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.DefaultMode.ToByteArray()));
        }

        [Test]
        public void CanWriteGeofence()
        {
            Location GeoLoc = new Location(78.134493, -14.944785, -9.492188, 79.804688);
            Geofence NewGeofence = new Geofence("TestName", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);

            FullConfiguration.NewConfigItems.NewConfigGeofence0 = NewGeofence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_0);

            TestTools.ReadConfig();

            Assert.AreEqual(true, StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence0.ToByteArray()));
        }

        [Test]
        public void CanWriteTimefence()
        {
            Time Start = new Time(5, 30);
            Time End = new Time(8, 0);
            Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_DAILY_SCHEDULE, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));
        }

        [Test]
        public void CanWriteIOSettings()
        {
            IOSettings NewIOSettings = new IOSettings(IOSettings.IOFlagsEnum.AUTOMATIC, 2, 2);

            FullConfiguration.NewConfigItems.NewConfigIOSettings = NewIOSettings;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.IO_SETTINGS);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigIOSettings.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigIOSettings.ToByteArray()));
        }

        [Test]
        public void CanWriteNotifications()
        {
           
            FullConfiguration.NewConfigItems.NewConfigNotifications.setNotificationHash(Notifications.NotificationTypeEnum.ON_MOTION);
            FullConfiguration.NewConfigItems.NewConfigNotifications.setNotificationHash(Notifications.NotificationTypeEnum.LOW_BATTERY);
            FullConfiguration.NewConfigItems.NewConfigNotifications.setLowBatteryThresh(8);
            FullConfiguration.NewConfigItems.NewConfigNotifications.setNotificationPath(Notifications.NotificationPathTypeEnum.BOTH);

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.NOTIFICATION_SETTINGS);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigNotifications.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigNotifications.ToByteArray()));
        }

        [Test]
        public void CanWriteCell()
        {
            FullConfiguration.NewConfigItems.NewConfigCell.setAPN("APN");
            FullConfiguration.NewConfigItems.NewConfigCell.setUsername("Username");
            FullConfiguration.NewConfigItems.NewConfigCell.setPassword("Password");
            FullConfiguration.NewConfigItems.NewConfigCell.setSmsNumbers("1234567899", "1234567899", "1234567899", "1234567899");

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.CELL_CONFIGURATION);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigCell.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigCell.ToByteArray()));
        }
        [Test]
        public void CanWriteIridium()
        {
            var IridiumKey = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, 0x00 };

            FullConfiguration.NewConfigItems.NewConfigIridium.UpdateIridiumEncryptionKey(IridiumKey);

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.IRIDIUM_CONFIGURATION);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigIridium.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigIridium.ToByteArray()));
        }

    }
}
