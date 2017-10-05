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
    [TestFixture (Category = "Timefence")]
    class TimefenceSmokeTests
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
        public void CanAddDailyTimefence()
        {
            Time Start = new Time(5, 0);
            Time End = new Time(8, 0);
            Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_DAILY_SCHEDULE, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));

        }

        [Test]
        public void CanAddStartAndEndTimefence()
        {
            Time Start = new Time(17, 30, 2017, 8 , 23);
            Time End = new Time(19, 30, 2017, 8, 23);

            Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_START_AND_END, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));

        }

        [Test]
        public void CanAddWeeklyTimefence()
        {
            Time Start = new Time(17, 30, Time.WeekdayEnum.WEDNESDAY);
            Time End = new Time(17, 30, Time.WeekdayEnum.THURSDAY);

            Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_WEEKLY_SCHEDULE, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));
        }

        [Test]
        public void Timefence_InvalidEntry_NameWithZeroChars()
        {
            Time Start = new Time(19, 30, 2017, 8, 30);
            Time End = new Time(17, 30, 2017, 8, 30);

            Timefence NewTimefence = new Timefence("", Timefence.TimefenceTypeEnum.TIMEFENCE_START_AND_END, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.AreNotEqual(true, StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));

        }

        [Test]
        public void StartAndEndTimefence_InvalidEntry_EndBeforeStart()
        {
            Time Start = new Time(19, 30, 2017, 8, 30);
            Time End = new Time(17, 30, 2017, 8, 30);

            Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_START_AND_END, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.AreNotEqual(true, StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));

        }

        [Test]
        public void StartAndEndTimefence_InvalidEntry_EndSameAsStart()
        {
            Time Start = new Time(17, 30, 2017, 8, 30);
            Time End = new Time(17, 30, 2017, 8, 30);

            Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_START_AND_END, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.AreNotEqual(true, StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));

        }

        [Test]
        public void CanAddDailyTimefence_EndBeforeStart()
        {
            Time Start = new Time(19, 30);
            Time End = new Time(17, 30);
            Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_DAILY_SCHEDULE, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));
        }

        [Test]
        public void DailyTimefence_InvalidEntry_EndSameAsStart()
        {
            Time Start = new Time(17, 30);
            Time End = new Time(17, 30);
            Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_DAILY_SCHEDULE, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.AreNotEqual(true, StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));
        }

        [Test]
        public void CanAddWeeklyTimefence_EndBeforeStart()
        {
            Time Start = new Time(19, 30, Time.WeekdayEnum.THURSDAY);
            Time End = new Time(17, 30, Time.WeekdayEnum.WEDNESDAY);

            Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_WEEKLY_SCHEDULE, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));
        }

        [Test]
        public void WeeklyTimefence_InvalidEntry_EndSameAsStart()
        {
            Time Start = new Time(17, 30, Time.WeekdayEnum.THURSDAY);
            Time End = new Time(17, 30, Time.WeekdayEnum.THURSDAY);

            Timefence NewTimefence = new Timefence("TestName", Timefence.TimefenceTypeEnum.TIMEFENCE_WEEKLY_SCHEDULE, Start, End);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence;

            Program.Session1.WriteConfig(WriteConfigurationMessage.ConfigurationItemsEnum.TIMEFENCE_0);

            TestTools.ReadConfig();

            Assert.AreNotEqual(true, StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));
        }

        [Test]
        public void CanAddThreeDailyTimefence()
        {
            Time Start0 = new Time(5, 0);
            Time End0 = new Time(8, 0);
            Timefence NewTimefence0 = new Timefence("Test1", Timefence.TimefenceTypeEnum.TIMEFENCE_DAILY_SCHEDULE, Start0, End0);

            Time Start1 = new Time(8, 0);
            Time End1 = new Time(11, 0);
            Timefence NewTimefence1 = new Timefence("Test2", Timefence.TimefenceTypeEnum.TIMEFENCE_DAILY_SCHEDULE, Start1, End1);

            Time Start2 = new Time(11, 0);
            Time End2 = new Time(14, 0);
            Timefence NewTimefence2 = new Timefence("Test3", Timefence.TimefenceTypeEnum.TIMEFENCE_DAILY_SCHEDULE, Start2, End2);

            FullConfiguration.NewConfigItems.NewConfigTimefence0 = NewTimefence0;
            FullConfiguration.NewConfigItems.NewConfigTimefence1 = NewTimefence1;
            FullConfiguration.NewConfigItems.NewConfigTimefence2 = NewTimefence2;

            Program.Session1.WriteMultiConfig(MultiConfiguration.ConfigurationItemsEnum.TIMEFENCE_0, MultiConfiguration.ConfigurationItemsEnum.TIMEFENCE_1, MultiConfiguration.ConfigurationItemsEnum.TIMEFENCE_2);

            TestTools.ReadConfig();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence0.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence0.ToByteArray()));

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence1.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence1.ToByteArray()));

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(NewTimefence2.ToByteArray(), Program.Session1.TG300.CurrentConfiguration.ConfigTimefence2.ToByteArray()));
        }
    }
}
