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
    [TestFixture(Category = "Iridium")]
    class IridiumSmokeTests
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

        [Test]
        public void CanEditEncryptionKey()
        {
            var IridiumKey = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00, 0x00 };

            FullConfiguration.NewConfigItems.NewConfigIridium.UpdateIridiumEncryptionKey(IridiumKey);

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.IRIDIUM_CONFIGURATION);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.NewConfigItems.NewConfigIridium.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigIridium.ToByteArray()));
        }
    }
}
