using NUnit.Framework;
using NUnitTests.Utils;
using Jalapeno.Config;
using Jalapeno;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System;

namespace NUnitTests.TestSuites
{
    [TestFixture(Category = "GPS")]
    class GPSSmokeTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            TestTools.WriteDefault();   //Before running this fixture, load a default configuration.
        }

        [SetUp]
        public void ReadConfig()
        {
            TestTools.ReadConfig();     //Read configuration every time
            //var config = Program.CurrentSession.TG300.CurrentConfiguration;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {

        }

        [Test]
        public void ConfigureGPSforOneSecond()
        {
            Thread.Sleep(2000);
            FullConfiguration.ConfigItems.NewConfigMode0 = Program.CurrentSession.TG300.CurrentConfiguration.DefaultMode;

            FullConfiguration.ConfigItems.NewConfigMode0.setGpsFixInterval(1);
            Debug.WriteLine("before: " + BitConverter.ToString(FullConfiguration.ConfigItems.NewConfigMode0.ToByteArray()));
            FullConfiguration.ConfigItems.NewConfigMode0.setOperationFlags(Mode.GPSModeEnum.MOTION_ONLY, false, true, Mode.CellModeEnum.STANDBY);
            Debug.WriteLine("after: " + BitConverter.ToString(FullConfiguration.ConfigItems.NewConfigMode0.ToByteArray()));

            Program.CurrentSession.WriteConfig(Jalapeno.Messaging.Messages.WriteConfigurationMessage.ConfigurationItemsEnum.DEFAULT_MODE);
            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.ConfigItems.NewConfigMode0.ToByteArray(), Program.CurrentSession.TG300.CurrentConfiguration.DefaultMode.ToByteArray()));
        }

        [Test]
        public void ConfigureGPSforTwoDays()
        {

        }
    }
}
