using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Ticking.Essentials.Tests
{
    [TestClass]
    public class TimeTests
    {
        public TimeTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

        [TestMethod]
        [DataRow("05/06/2018", "04/06/2018")]
        public void FirstDayOfWeek(string date, string firstDayDate)
        {
            Assert.AreEqual(firstDayDate.ParseDateTime(), date.ParseDateTime().FirstDayOfWeek());
        }

        [TestMethod]
        [DataRow("05/06/2018", "10/06/2018")]
        public void LastDayOfWeek(string date, string firstDayDate)
        {
            Assert.AreEqual(firstDayDate.ParseDateTime(), date.ParseDateTime().LastDayOfWeek());
        }

        [TestMethod]
        [DataRow("05/06/2018", "01/06/2018")]
        public void FirstDayOfMonth(string date, string firstDayDate)
        {
            Assert.AreEqual(firstDayDate.ParseDateTime(), date.ParseDateTime().FirstDayOfMonth());
        }

        [TestMethod]
        [DataRow("05/06/2018", "30/06/2018")]
        public void LastDayOfMonth(string date, string firstDayDate)
        {
            Assert.AreEqual(firstDayDate.ParseDateTime(), date.ParseDateTime().LastDayOfMonth());
        }

        [TestMethod]
        [DataRow("05/06/2018", "01/01/2018")]
        public void FirstDayOfYear(string date, string firstDayDate)
        {
            Assert.AreEqual(firstDayDate.ParseDateTime(), date.ParseDateTime().FirstDayOfYear());
        }

        [TestMethod]
        [DataRow("05/06/2018", "31/12/2018")]
        public void LastDayOfYear(string date, string firstDayDate)
        {
            Assert.AreEqual(firstDayDate.ParseDateTime(), date.ParseDateTime().LastDayOfYear());
        }

        [TestMethod]
        [DataRow("05/06/2018", "30/06/2018", 0)]
        [DataRow("05/06/2018", "04/06/2020", 1)]
        [DataRow("05/06/2018", "30/06/2020", 2)]
        public void Age(string from, string to, int expectedAge)
        {
            Assert.AreEqual(expectedAge, from.ParseDateTime().Age(to.ParseDateTime()));
        }

        [TestMethod]
        [DataRow("05/06/2018 10:00", "05/06/2018 00:00")]
        public void WithoutTime(string date, string firstDayDate)
        {
            Assert.AreEqual(firstDayDate.ParseDateTime(), date.ParseDateTime().WithoutTime());
        }

        [TestMethod]
        public void Days()
        {
            var from = DateTime.Now;
            var to = from.AddDays(64);
            var days = from.Days(to);

            Assert.AreEqual(64, days.Count());
            Assert.AreEqual(from.WithoutTime(), days.First());
            Assert.AreEqual(to.AddDays(-1).WithoutTime(), days.Last());
        }

        [TestMethod]
        public void DaysInMonth()
        {
            var from = DateTime.Now;
            var daysInMonth = DateTime.DaysInMonth(from.Year, from.Month);
            var days = from.DaysInMonth();
            Assert.AreEqual(daysInMonth, days.Count());
            Assert.AreEqual(from.FirstDayOfMonth().WithoutTime(), days.First());
            Assert.AreEqual(from.LastDayOfMonth().WithoutTime(), days.Last());
        }

        [TestMethod]
        [DataRow("05/06/2018", "11/06/2018")]
        public void NextDateOfDay(string date, string firstDayDate)
        {
            Assert.AreEqual(firstDayDate.ParseDateTime(), date.ParseDateTime().NextDateOfDay(DayOfWeek.Monday));
        }
    }
}
