using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Jalapeno.Config;
using Jalapeno.Messaging.Messages;
using Jalapeno;
using NUnitTests.Utils;

namespace NUnitTests.TestSuites
{
    [TestFixture]
    class RealTimeTrackingSmokeTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            TestTools.WriteDefault();
        }

        [Test]
        public void CanEnableRealTimeTracking()
        {
            Thread.Sleep(5000);

            Program.Session1.EnableRealTimeTracking();

            Program.Session1.ListenForCommandType(Jalapeno.Messaging.MessagePacketData.CommandTypeEnum.ENABLE_REALTIME_TRACKING_REPLY);

            Thread.Sleep(5000);

            Assert.IsTrue(Program.Session1.TG300.MsgListener.CommandTypeFlag);

            Program.Session1.DisableRealTimeTracking();
        }

        [Test]
        public void CanDisableRealTimeTracking()
        {
            Thread.Sleep(5000);

            Program.Session1.EnableRealTimeTracking();

            Thread.Sleep(5000);

            Program.Session1.DisableRealTimeTracking();

            Program.Session1.ListenForCommandType(Jalapeno.Messaging.MessagePacketData.CommandTypeEnum.DISABLE_REALTIME_TRACKING_REPLY);

            Thread.Sleep(5000);

            Assert.IsTrue(Program.Session1.TG300.MsgListener.CommandTypeFlag);

        }

        [Test]
        public void CanRequestPosition()
        {
            Thread.Sleep(5000);

            Program.Session1.RequestPosition();

            Program.Session1.ListenForCommandType(Jalapeno.Messaging.MessagePacketData.CommandTypeEnum.REQUEST_POSITION_REPLY);

            Thread.Sleep(5000);

            Assert.IsTrue(Program.Session1.TG300.MsgListener.CommandTypeFlag);

        }

    }
}
