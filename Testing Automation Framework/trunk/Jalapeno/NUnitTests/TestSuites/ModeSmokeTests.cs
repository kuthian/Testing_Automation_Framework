using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jalapeno;
using Jalapeno.Config;
using Jalapeno.Messaging;
using Jalapeno.Messaging.Messages;
using NUnit.Framework;
using NUnitTests.Utils;
using System.Diagnostics;
using System.Threading;

namespace NUnitTests.TestSuites
{
    [TestFixture(Category = "Mode")]
    class ModeSmokeTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            TestTools.WriteDefault();
        }
        [SetUp]
        public void RunBeforeAnyTests()
        {
            Thread.Sleep(2000);
            TestTools.ReadConfig();
        }

        [Test]
        public void CanAddMode()
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
        public void ConfigureGPSforOneSecond()
        {
           
            FullConfiguration.ConfigItems.NewConfigMode0 = Program.CurrentSession.TG300.CurrentConfiguration.AdvancedModes0;

            FullConfiguration.ConfigItems.NewConfigMode0.setGpsFixInterval(1);
            FullConfiguration.ConfigItems.NewConfigMode0.setOperationFlags(Mode.GPSModeEnum.MOTION_ONLY, false, true, Mode.CellModeEnum.STANDBY);

            Program.CurrentSession.WriteConfig(Jalapeno.Messaging.Messages.WriteConfigurationMessage.ConfigurationItemsEnum.DEFAULT_MODE);
            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.ConfigItems.NewConfigMode0.ToByteArray(), Program.CurrentSession.TG300.CurrentConfiguration.DefaultMode.ToByteArray()));
        }
    }
}
