using NUnit.Framework;
using NUnitTests.Utils;
using Jalapeno.Config;
using Jalapeno;
using System.Collections;

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
            var config = Program.Session1.TG300.CurrentConfiguration;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            
        }

        [Test]
        public void ConfigureGPSforOneSecond()
        {
            var InitialMode0 = FullConfiguration.NewConfigItems.NewConfigMode0 = Program.Session1.TG300.CurrentConfiguration.AdvancedModes0;
    
            InitialMode0.SetGpsFixInterval(120);
            Program.Session1.WriteConfig(Jalapeno.Messaging.Messages.WriteConfigurationMessage.ConfigurationItemsEnum.DEFAULT_MODE);
            TestTools.ReadConfig();
            var FinalMode0 = Program.Session1.TG300.CurrentConfiguration.AdvancedModes0;

            var ov1 = InitialMode0.ToByteArray();
            var ov2 = FinalMode0.ToByteArray();
            bool res = StructuralComparisons.Equals(ov1, ov2);
            Assert.IsTrue(Equals(ov1, ov2));
        }

        [Test]
        public void ConfigureGPSforTwoDays()
        {

        }
    }
}
