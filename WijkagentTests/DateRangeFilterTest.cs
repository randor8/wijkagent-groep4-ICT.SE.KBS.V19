using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using WijkagentModels;
using WijkagentWPF.Filters;

namespace WijkagentTests
{
    [TestFixture]
    public class DateRangeFilterTest
    {
        private static DateTime DateTime { get; } = DateTime.Now;
        private static readonly DateRangeFilter filter1 = new DateRangeFilter(DateTime, DateTime.AddDays(1));
        private static readonly DateRangeFilter filter2 = new DateRangeFilter(DateTime, DateTime.AddDays(1));
        private static readonly DateRangeFilter filter3 = new DateRangeFilter(DateTime.AddDays(1), DateTime.AddDays(1));
        private static readonly DateRangeFilter filter4 = new DateRangeFilter(DateTime, DateTime.AddDays(2));
        private static readonly DateRangeFilter filter5 = new DateRangeFilter(DateTime.AddDays(1), DateTime.AddDays(2));


        public static IEnumerable EqualsTestCases
        {
            get
            {
                yield return new TestCaseData(filter1, filter1).Returns(true);
                yield return new TestCaseData(filter1, filter2).Returns(true);
                yield return new TestCaseData(filter1, filter3).Returns(false);
                yield return new TestCaseData(filter1, filter4).Returns(false);
                yield return new TestCaseData(filter1, filter5).Returns(false);
            }
        }

        [Test, TestCaseSource("EqualsTestCases")]
        public bool Equals_AllCases(DateRangeFilter filter1, DateRangeFilter filter2)
        {
            return filter1.Equals(filter2);
        }

        [Test]
        public void ApplyOn_NoMatchingOffences_ReturnNoOffences()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(DateTime.AddDays(-5), "", new Location(1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(DateTime.AddDays(-1), "", new Location(1.1, 1.1), OffenceCategories.Cybercrime)
            };
            DateRangeFilter filter = new DateRangeFilter(DateTime.AddDays(-4), DateTime.AddDays(-2));
            Assert.AreEqual(_offences.FindAll(x => x.DateTime.Date >= DateTime.AddDays(-4) && x.DateTime.Date <= DateTime.AddDays(-2)), filter.ApplyOn(_offences));
        }

        [Test]
        public void ApplyOn_OneMatchingOffence_ReturnMatchingOffence()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(DateTime.AddDays(-3), "", new Location(1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(DateTime.AddDays(-1), "", new Location(1.1, 1.1), OffenceCategories.Cybercrime)
            };
            DateRangeFilter filter = new DateRangeFilter(DateTime.AddDays(-4), DateTime.AddDays(-2));
            Assert.AreEqual(_offences.FindAll(x => x.DateTime.Date >= DateTime.AddDays(-4) && x.DateTime.Date <= DateTime.AddDays(-2)), filter.ApplyOn(_offences));
        }

        [Test]
        public void ApplyOn_MultipleMatchingOffence_ReturnAllMatchingOffences()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(DateTime.AddDays(-3), "", new Location(1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(DateTime.AddDays(-2), "", new Location(1.1, 1.1), OffenceCategories.Cybercrime)
            };
            DateRangeFilter filter = new DateRangeFilter(DateTime.AddDays(-4), DateTime.AddDays(-2));
            Assert.AreEqual(_offences.FindAll(x => x.DateTime.Date >= DateTime.AddDays(-4) && x.DateTime.Date <= DateTime.AddDays(-2)), filter.ApplyOn(_offences));
        }
    }
}
