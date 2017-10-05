using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Jalapeno.Config;
using Jalapeno.Messaging;
using Jalapeno;
using NUnitTests.Utils;

namespace NUnitTests.JalapenoSmokeTest
{
    [TestFixture]
    class ConfigPacketSmokeTest
    {
        //[SetUp]
        //public void RunBeforeAnyTests()
        //{
        //    TestTools.ReadConfig();
        //}

        //[TearDown]
        //public void RunAfterAnyTests()
        //{
        //    //TestTools.WaitAssert(true, Program.CurrentSession.TG300.Connected);
        //}

        //[Test]
        //public void CanBuildModePacket()
        //{

        //}
        //[Test]
        //public void CanBuildGeofencePacket()
        //{

        //}
        //[Test]
        //public void CanBuildTimefencePacket()
        //{
        //    //This will not work because the config CRC packet will be different everytime there is a different base config. 

        //    Time Start = new Time(5, 30);
        //    Time End = new Time(8, 0);
        //    Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_DAILY_SCHEDULE, Start, End);

        //    FullConfiguration.ConfigItems.NewConfigTimefence0 = NewTimefence;

        //    //Program.CurrentSession.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

        //    var TimefencePacket = new byte[] { 0xAA, 0x55, 0xFF, 0xFF, 0xFF, 0xFF, 0x04, 0x1A, 0x00, 0x05, 0x02, 0x4A, 0x2A, 0x00, 0x00, 0x0E, 0x02, 0x11, 0x54, 0x65, 0x73, 0x74, 0x4E, 0x61, 0x6D, 0x65, 0x02, 0x58, 0x4D, 0x00, 0x00, 0x80, 0x70, 0x00, 0x00, 0x02, 0x09, 0xFF, 0xCC }; //AA-55-FF-FF-FF-FF-04-1A-00-05-02-4A-2A-00-00-0E-02-11-54-65-73-74-4E-61-6D-65-02-58-4D-00-00-80-70-00-00-02-09-FF-CC

        //    Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(FullConfiguration.ConfigItems.NewConfigIOSettings.ToByteArray(), TimefencePacket));

        //}
        //[Test]
        //public void CanBuildIOSettingsPacket()
        //{
        //    var IOSettingsPacket = new byte[] { 0xAA, 0x55, 0xFF, 0xFF, 0xFF, 0xFF, 0x04, 0x14, 0x00, 0x05, 0x02, 0x1B, 0x94, 0x00, 0x00, 0x43, 0x02, 0x0B, 0x00, 0x00, 0x00, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x03, 0x20, 0x03, 0xA1, 0xAF, 0xFF, 0xCC };  //AA-55-FF-FF-FF-FF-04-14-00-05-02-1B-94-00-00-43-02-0B-00-00-00-C0-00-00-00-00-03-20-03-A1-AF-FF-CC
        //}
        //[Test]
        //public void CanBuildNotificationsPacket()
        //{

        //}
        //[Test]
        //public void CanBuildIridiumPacket()
        //{

        //}
        //[Test]
        //public void CanBuildCellPacket()
        //{

        //}
        
    }
}
