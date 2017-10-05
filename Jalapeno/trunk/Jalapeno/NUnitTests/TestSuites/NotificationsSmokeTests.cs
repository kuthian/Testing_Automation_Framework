using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Jalapeno.Config;
using Jalapeno.Messaging.Messages;
using Jalapeno;
using NUnitTests.Utils;

namespace NUnitTests.TestSuites
{
    [TestFixture (Category = "Notifications")]
    class NotificationsSmokeTests
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
        public void CanAddNotifications()
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
        public void CanChangeLowVoltageLevel()
        {
            FullConfiguration.NewConfigItems.NewConfigNotifications = Program.Session1.TG300.CurrentConfiguration.ConfigNotifications;

            FullConfiguration.NewConfigItems.NewConfigNotifications.setLowBatteryThresh(8);

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.NOTIFICATION_SETTINGS);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigNotifications.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigNotifications.ToByteArray()));
        }

        [Test]
        public void CanSelectServerDestinationOnly()
        {
            FullConfiguration.NewConfigItems.NewConfigNotifications = Program.Session1.TG300.CurrentConfiguration.ConfigNotifications;

            FullConfiguration.NewConfigItems.NewConfigNotifications.setNotificationPath(Notifications.NotificationPathTypeEnum.SERVER);

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.NOTIFICATION_SETTINGS);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigNotifications.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigNotifications.ToByteArray()));
        }

        [Test]
        public void CanSelectSMSDestination()
        {
            FullConfiguration.NewConfigItems.NewConfigNotifications = Program.Session1.TG300.CurrentConfiguration.ConfigNotifications;

            FullConfiguration.NewConfigItems.NewConfigNotifications.setNotificationPath(Notifications.NotificationPathTypeEnum.SMS);

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.NOTIFICATION_SETTINGS);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigNotifications.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigNotifications.ToByteArray()));
        }

        [Test]
        public void CanSelectServerAndSMSDestination()
        {
            FullConfiguration.NewConfigItems.NewConfigNotifications = Program.Session1.TG300.CurrentConfiguration.ConfigNotifications;

            FullConfiguration.NewConfigItems.NewConfigNotifications.setNotificationPath(Notifications.NotificationPathTypeEnum.BOTH);

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.NOTIFICATION_SETTINGS);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigNotifications.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigNotifications.ToByteArray()));
        }
    }
}
