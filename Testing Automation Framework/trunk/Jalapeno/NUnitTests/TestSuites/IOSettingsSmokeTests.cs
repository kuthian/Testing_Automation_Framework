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
using NUnitTests.Utils;

namespace NUnitTests.TestSuites
{
    [TestFixture (Category = "IO")]
    class IOSettingsSmokeTests
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
        public void CanAddIOSettings()
        {
            IOSettings NewIOSettings = new IOSettings(IOSettings.IOFlagsEnum.AUTOMATIC, 2, 2);

            FullConfiguration.NewConfigItems.NewConfigIOSettings = NewIOSettings;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.IO_SETTINGS);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigIOSettings.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigIOSettings.ToByteArray()));
        }

        [Test]
        public void CanAddNineOutputs()
        {
            IOSettings NewIOSettings = new IOSettings(IOSettings.IOFlagsEnum.AUTOMATIC, 0, 9);

            FullConfiguration.NewConfigItems.NewConfigIOSettings = NewIOSettings;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.IO_SETTINGS);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigIOSettings.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigIOSettings.ToByteArray()));
        }

        [Test]
        public void CanAddNineInputs()
        {
            IOSettings NewIOSettings = new IOSettings(IOSettings.IOFlagsEnum.AUTOMATIC, 9, 0);

            FullConfiguration.NewConfigItems.NewConfigIOSettings = NewIOSettings;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.IO_SETTINGS);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigIOSettings.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigIOSettings.ToByteArray()));
        }

        [Test]
        public void CanSetToManual()
        {
            FullConfiguration.NewConfigItems.NewConfigIOSettings.IOFlags = IOSettings.IOFlagsEnum.MANUAL;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.IO_SETTINGS);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigIOSettings.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigIOSettings.ToByteArray()));
        }

    }
}
