using System.Threading;
using NUnit.Framework;
using Jalapeno;
using NUnitTests.Utils;

namespace NUnitTests.TestSuites
{
    [TestFixture]
    class DiagnosticsSmokeTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            TestTools.WriteDefault();
        }
        [Test]
        public void CanRequestDiagnostics()
        {
            Thread.Sleep(5000);

            Program.Session1.RequestDiagnostics();

            Program.Session1.ListenForCommandType(Jalapeno.Messaging.MessagePacketData.CommandTypeEnum.DIAGNOSTIC_STATUS_REPLY);

            Thread.Sleep(10000);

            Assert.IsTrue(Program.Session1.TG300.MsgListener.CommandTypeFlag);
        }
    }
}
