using System;
using System.Collections.Generic;
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
    [TestFixture]
    class IdentificationSmokeTest
    {
        //[OneTimeSetUp]
        //public void Init()
        //{
        //    TestTools.WriteDefault();
        //}
        [Test]
        public void CanRequestIdentification()
        {
            Thread.Sleep(5000);

            Program.Session1.ListenForCommandType(Jalapeno.Messaging.MessagePacketData.CommandTypeEnum.IDENTIFICATION_REPLY);
            Program.Session1.RequestIdentification();

            Thread.Sleep(5000);

            Assert.IsTrue(Program.Session1.TG300.MsgListener.CommandTypeFlag);
        }
    }
}
