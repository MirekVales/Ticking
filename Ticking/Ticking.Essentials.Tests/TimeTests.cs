using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
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
    }
}
