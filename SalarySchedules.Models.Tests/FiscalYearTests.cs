using System;
using NUnit.Framework;

namespace SalarySchedules.Models.Tests
{
    [TestFixture]
    [Category("Models: FiscalYear")]
    public class FiscalYearTests
    {
        static readonly string start = "14";
        static readonly string end = "15";

        [Test]
        public void New_DatesHaveNoValue()
        {
            var fiscalYear = new FiscalYear();

            Assert.False(fiscalYear.StartDate.HasValue);
            Assert.False(fiscalYear.EndDate.HasValue);
        }

        [Test]
        public void New_With2DigitStartAndEnd_Assumes21Century()
        {
            var fiscalYear = new FiscalYear(start, end);

            Assert.AreEqual(2014, fiscalYear.StartDate.Value.Year);
            Assert.AreEqual(2015, fiscalYear.EndDate.Value.Year);
        }

        [Test]
        public void FiscalYear_Starts_July1MidnightLocal()
        {
            var fiscalYear = new FiscalYear(start, end);
            var startDate = fiscalYear.StartDate.Value;

            Assert.AreEqual(7, startDate.Month);
            Assert.AreEqual(1, startDate.Day);
            Assert.AreEqual(0, startDate.Hour);
            Assert.AreEqual(0, startDate.Minute);
            Assert.AreEqual(0, startDate.Second);
            Assert.AreEqual(DateTimeKind.Local, startDate.Kind);
        }

        [Test]
        public void FiscalYear_Ends_BeforeJuly1MidnightLocal()
        {
            var fiscalYear = new FiscalYear(start, end);
            var endDate = fiscalYear.EndDate.Value;

            Assert.AreEqual(6, endDate.Month);
            Assert.AreEqual(30, endDate.Day);
            Assert.AreEqual(11, endDate.Hour);
            Assert.AreEqual(59, endDate.Minute);
            Assert.AreEqual(59, endDate.Second);
            Assert.AreEqual(DateTimeKind.Local, endDate.Kind);
        }
    }
}
