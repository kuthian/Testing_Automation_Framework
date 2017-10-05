using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Jalapeno.Config;
using Jalapeno.Messaging.Messages;
using Jalapeno;
using System.Diagnostics;
using NUnitTests.Utils;


namespace NUnitTests.TestSuites
{
    [TestFixture (Category = "Geofence")] //("Jalapeno", Category = "Geofence")
    class GeofenceSmokeTests
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
        public void CanEstablishUSBConnection()
        {
            Thread.Sleep(2000);
            
            Assert.IsTrue(Program.Session1.TG300.Connected);
        }

        [Test]
        public void CanAddRectangularGeofence()
        {
            Location GeoLoc = new Location(78.134493, -14.944785, -9.492188, 79.804688);
            Geofence NewGeofence = new Geofence("Test", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);

            FullConfiguration.NewConfigItems.NewConfigGeofence0 = NewGeofence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_0);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence0.ToByteArray()));
        }

        [Test]
        public void CanAddCircularGeofence()
        {
            Location GeoLoc = new Location(78.134493, -14.944785, -9.492188, 79.804688);
            Geofence NewGeofence = new Geofence("Test", Geofence.GeofenceTypeEnum.GEOFENCE_CIRCULAR, GeoLoc);

            FullConfiguration.NewConfigItems.NewConfigGeofence0 = NewGeofence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_0);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence0.ToByteArray()));
        }

        [Test]
        public void CanModifyGeofence()
        {
            Location GeoLoc = new Location(78.134493, -14.944785, -9.492188, 79.804688);
            Geofence NewGeofence = new Geofence("Test", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);

            FullConfiguration.NewConfigItems.NewConfigGeofence0 = NewGeofence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_0);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence0.ToByteArray()));
        }

        [Test]
        public void CanDeleteGeofence()
        {
            Location GeoLoc = new Location(78.134493, -14.944785, -9.492188, 79.804688);
            Geofence NewGeofence = new Geofence("TestName", Geofence.GeofenceTypeEnum.GEOFENCE_DISABLED, GeoLoc);

            FullConfiguration.NewConfigItems.NewConfigGeofence0 = NewGeofence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_0);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence0.ToByteArray()));
        }

        [Test]
        public void RectangularGeofence_InvalidEntry_SouthAboveNorth()
        {
            Location GeoLoc = new Location(-45.134493, -14.944785, -9.492188, 79.804688);
            Geofence NewGeofence = new Geofence("TestName", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);

            FullConfiguration.NewConfigItems.NewConfigGeofence0 = NewGeofence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_0);

            TestTools.ReadConfig();

            Assert.IsFalse(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence0.ToByteArray()));
        }

        [Test]
        public void RectangularGeofence_InvalidEntry_SouthSameAsNorth()
        {
            Location GeoLoc = new Location(78.134493, 78.134493, -9.492188, 79.804688);
            Geofence NewGeofence = new Geofence("TestName", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);

            FullConfiguration.NewConfigItems.NewConfigGeofence0 = NewGeofence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_0);

            TestTools.ReadConfig();

            Assert.IsFalse(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence0.ToByteArray()));
        }

        [Test]
        public void RectangularGeofence_InvalidEntry_EastSameAsWest()
        {
            Location GeoLoc = new Location(78.134493, -14.944785, 79.804688, 79.804688);
            Geofence NewGeofence = new Geofence("TestName", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);

            FullConfiguration.NewConfigItems.NewConfigGeofence0 = NewGeofence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_0);

            TestTools.ReadConfig();

            Assert.IsFalse(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence0.ToByteArray()));
        }

        [Test]
        public void Geofence_InvalidEntry_NameWithZeroChars()
        {
            Location GeoLoc = new Location(78.134493, -14.944785, 79.804688, 79.804688);
            Geofence NewGeofence = new Geofence("", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);

            FullConfiguration.NewConfigItems.NewConfigGeofence0 = NewGeofence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.GEOFENCE_0);

            TestTools.ReadConfig();

            Assert.IsFalse(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence0.ToByteArray()));
        }


        [Test]
        public void CanAddTenGeofences()
        {
            Location GeoLoc = new Location(78.134493, -14.944785, -9.492188, 79.804688);
            Geofence NewGeofence0 = new Geofence("Test0", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);
            Geofence NewGeofence1 = new Geofence("Test1", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);
            Geofence NewGeofence2 = new Geofence("Test2", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);
            Geofence NewGeofence3 = new Geofence("Test3", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);
            Geofence NewGeofence4 = new Geofence("Test4", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);
            Geofence NewGeofence5 = new Geofence("Test5", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);
            Geofence NewGeofence6 = new Geofence("Test6", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);
            Geofence NewGeofence7 = new Geofence("Test7", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);
            Geofence NewGeofence8 = new Geofence("Test8", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);
            Geofence NewGeofence9 = new Geofence("Test9", Geofence.GeofenceTypeEnum.GEOFENCE_RECTANGLE, GeoLoc);

            FullConfiguration.NewConfigItems.NewConfigGeofence0 = NewGeofence0;
            FullConfiguration.NewConfigItems.NewConfigGeofence1 = NewGeofence1;
            FullConfiguration.NewConfigItems.NewConfigGeofence2 = NewGeofence2;
            FullConfiguration.NewConfigItems.NewConfigGeofence3 = NewGeofence3;
            FullConfiguration.NewConfigItems.NewConfigGeofence4 = NewGeofence4;
            FullConfiguration.NewConfigItems.NewConfigGeofence5 = NewGeofence5;
            FullConfiguration.NewConfigItems.NewConfigGeofence6 = NewGeofence6;
            FullConfiguration.NewConfigItems.NewConfigGeofence7 = NewGeofence7;
            FullConfiguration.NewConfigItems.NewConfigGeofence8 = NewGeofence8;
            FullConfiguration.NewConfigItems.NewConfigGeofence9 = NewGeofence9;


            Program.Session1.WriteMultiConfig(MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_0, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_1, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_2, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_3, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_4, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_5, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_6, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_7, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_8, MultiConfiguration.ConfigurationItemsEnum.GEOFENCE_9);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence0.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence0.ToByteArray()));
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence1.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence1.ToByteArray()));
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence2.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence2.ToByteArray()));
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence3.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence3.ToByteArray()));
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence4.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence4.ToByteArray()));
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence5.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence5.ToByteArray()));
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence6.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence6.ToByteArray()));
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence7.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence7.ToByteArray()));
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence8.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence8.ToByteArray()));
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewGeofence9.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigGeofence9.ToByteArray()));

        }
    }
}
