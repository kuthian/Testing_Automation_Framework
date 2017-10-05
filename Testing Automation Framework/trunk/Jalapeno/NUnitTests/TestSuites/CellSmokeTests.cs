using System.Collections;
using NUnit.Framework;
using Jalapeno.Config;
using Jalapeno.Messaging.Messages;
using Jalapeno;
using NUnitTests.Utils;

namespace NUnitTests.TestSuites
{
    [TestFixture(Category = "Cell")]
    class CellSmokeTests
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
        public void CanChangeApnName()
        {
            FullConfiguration.NewConfigItems.NewConfigCell.setAPN("jtm2m");

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.CELL_CONFIGURATION);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigCell.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigCell.ToByteArray()));
        }

        [Test]
        public void CanAddFourPhoneNumbers()
        {
            FullConfiguration.NewConfigItems.NewConfigCell.setSmsNumbers("1234567890", "1234567890", "1234567890", "1234567890");

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.CELL_CONFIGURATION);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigCell.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigCell.ToByteArray()));
        }
    }
}
