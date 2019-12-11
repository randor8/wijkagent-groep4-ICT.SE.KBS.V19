using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using WijkagentModels;
using WijkagentWPF;

namespace WijkagentTests
{
    [TestFixture]
    public class CategoryFilterTest
    {
        private static readonly CategoryFilter filter1 = new CategoryFilter(OffenceCategories.Cybercrime);
        private static readonly CategoryFilter filter2 = new CategoryFilter(OffenceCategories.Cybercrime);
        private static readonly CategoryFilter filter3 = new CategoryFilter(OffenceCategories.Drugs);

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
        public bool Equals_AllCases(CategoryFilter filter1, CategoryFilter filter2)
        {
            return filter1.Equals(filter2);
        }

        [Test]
        public void ApplyOn_Cybercrime_OriginalListUnaltered()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(1, new DateTime().ToLocalTime(), "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(2, new DateTime().ToLocalTime(), "", new Location(1, 1.1, 1.1), OffenceCategories.Drugs)
            };
            CategoryFilter filter = new CategoryFilter(OffenceCategories.Cybercrime);
            filter.ApplyOn(_offences);
            Assert.IsTrue(_offences.Count == 2);
        }

        [Test]
        public void ApplyOn_Cybercrime_ReturnListWithOffencesWithCybercrimeAsCategory()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(1, new DateTime().ToLocalTime(), "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(2, new DateTime().ToLocalTime(), "", new Location(1, 1.1, 1.1), OffenceCategories.Drugs)
            };
            CategoryFilter filter = new CategoryFilter(OffenceCategories.Cybercrime);
            List<Offence> filtered = filter.ApplyOn(_offences);
            Assert.AreEqual(_offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime)), filtered);
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
