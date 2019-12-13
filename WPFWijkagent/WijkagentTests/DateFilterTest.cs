using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using WijkagentModels;
using WijkagentWPF;

namespace WijkagentTests
{
    [TestFixture]
    public class DateTimeFilter
    {
        private static DateTime DateTime { get; } = DateTime.Now;
        private static readonly DateFilter filter1 = new DateFilter(DateTime);
        private static readonly DateFilter filter2 = new DateFilter(DateTime);
        private static readonly DateFilter filter3 = new DateFilter(DateTime.AddDays(1));

        public static IEnumerable EqualsTestCases
        {
            get
            {
                yield return new TestCaseData(filter1, filter1).Returns(true);
                yield return new TestCaseData(filter1, filter2).Returns(true);
                yield return new TestCaseData(filter1, filter3).Returns(false);
            }
        }

        [Test, TestCaseSource("EqualsTestCases")]
        public bool Equals_AllCases(DateFilter filter1, DateFilter filter2)
        {
            return filter1.Equals(filter2);
        }

        [Test]
        public void ApplyOn_Now_OriginalListUnaltered()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(1, DateTime, "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(2, DateTime.AddDays(1), "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime)
            };
            DateFilter filter = new DateFilter(DateTime);
            filter.ApplyOn(_offences);
            Assert.IsTrue(_offences.Count == 2);
        }

        [Test]
        public void ApplyOn_Cybercrime_ReturnListWithOffencesWithCybercrimeAsCategory()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(1, DateTime, "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(2, DateTime.AddDays(1), "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime)
            };
            DateFilter filter = new DateFilter(DateTime);
            List<Offence> filtered = filter.ApplyOn(_offences);
            Assert.AreEqual(_offences.FindAll(x => x.DateTime.Date.Equals(DateTime.Date)), filtered);
        }

        [Test]
        public void Equals_AddSameFilterToHashSet_FilterExistsOnce()
        {
            HashSet<IFilter> filterSet = new HashSet<IFilter> { filter1 };
            Assert.IsTrue(filterSet.Count == 1);
            filterSet.Add(filter1);
            Assert.IsTrue(filterSet.Count == 1);
        }

        [Test]
        public void Equals_AddDifferentFilterWithSameCategoryToHashSet_FilterExistsOnce()
        {
            HashSet<IFilter> filterSet = new HashSet<IFilter> { filter1 };
            Assert.IsTrue(filterSet.Count == 1);
            filterSet.Add(filter2);
            Assert.IsTrue(filterSet.Count == 1);
        }
    }
}
