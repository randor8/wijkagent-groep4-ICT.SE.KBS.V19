using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;
using WijkagentWPF;

namespace WijkagentTests
{
    [TestFixture]
    public class FilterListTest
    {
        private DateTime DateTime { get; } = DateTime.Now;

        [SetUp]
        public void Setup()
        {
            FilterList.ClearFilters();
        }

        [Test]
        public void AddFilter_AddCategoryFilterToEmptyList_AddsFilterToList()
        {
            CategoryFilter cybercrimeFilter = new CategoryFilter(OffenceCategories.Cybercrime);
            FilterList.AddFilter(cybercrimeFilter);
            Assert.Contains(cybercrimeFilter, FilterList.GetFilters());
        }

        [Test]
        public void AddFilter_AddDateFilterToEmptyList_AddsFilterToList()
        {
            DateFilter dateFilter = new DateFilter(DateTime);
            FilterList.AddFilter(dateFilter);
            Assert.Contains(dateFilter, FilterList.GetFilters());
        }

        [Test]
        public void AddFilter_FilterListWith1DifferentFilter_AddsFilterToList()
        {
            FilterList.AddFilter(new DateFilter(DateTime));
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Cybercrime));
            Assert.IsTrue(FilterList.GetFilters().Count == 2);
        }

        [Test]
        public void AddFilter_FilterListWithSameFilter_FilterNotAddedToList()
        {
            CategoryFilter cybercrimeFilter = new CategoryFilter(OffenceCategories.Cybercrime);
            FilterList.AddFilter(cybercrimeFilter);
            Assert.Contains(cybercrimeFilter, FilterList.GetFilters());
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Cybercrime));
            Assert.IsTrue(FilterList.GetFilters().Count == 1);
            Assert.AreEqual(cybercrimeFilter, FilterList.GetFilters()[0]);
        }

        [Test]
        public void RemoveFilter_ListWith2Filters_RemovedFilterNoLongerInList()
        {
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Cybercrime));
            CategoryFilter drugsFilter = new CategoryFilter(OffenceCategories.Drugs);
            FilterList.AddFilter(drugsFilter);
            Assert.IsTrue(FilterList.GetFilters().Count == 2);
            FilterList.RemoveFilter(drugsFilter);
            Assert.IsTrue(FilterList.GetFilters().Count == 1);
            Assert.IsFalse(FilterList.GetFilters().Contains(drugsFilter));
        }

        [Test]
        public void ClearFilters_ListWith2Filters_FilterListIsEmpty()
        {
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Cybercrime));
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Drugs));
            FilterList.ClearFilters();
            Assert.IsEmpty(FilterList.GetFilters());
        }

        [Test]
        public void ApplyFilters_ApplyCybercrimeFilter_ReturnCybercrimeOffences()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(new DateTime().ToLocalTime(), new Location(1.1, 1.1)) { Category = OffenceCategories.Cybercrime },
                new Offence(new DateTime().ToLocalTime(), new Location(1.1, 1.1)) { Category = OffenceCategories.Drugs }
            };
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Cybercrime));
            Assert.AreEqual(_offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime)), FilterList.ApplyFilters(_offences));
        }

        [Test]
        public void ApplyFilters_ApplyCybercrimeAndDrugsFilter_ReturnCybercrimeAndDrugsOffences()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(new DateTime().ToLocalTime(), new Location(1.1, 1.1)) { Category = OffenceCategories.Cybercrime },
                new Offence(new DateTime().ToLocalTime(), new Location(1.1, 1.1)) { Category = OffenceCategories.Drugs },
                new Offence(new DateTime().ToLocalTime(), new Location(1.1, 1.1)) { Category = OffenceCategories.Fraude }
            };
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Cybercrime));
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Drugs));
            List<Offence> filtered = _offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime) || x.Category.Equals(OffenceCategories.Drugs));
            Assert.AreEqual(filtered, FilterList.ApplyFilters(_offences));
        }

        [Test]
        public void ApplyFilters_ApplyDateFilter_ReturnOffencesFromDate()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(DateTime, new Location(1.1, 1.1)),
                new Offence(DateTime.AddDays(1), new Location(1.1, 1.1))
            };
            FilterList.AddFilter(new DateFilter(DateTime));
            List<Offence> filtered = _offences.FindAll(x => x.DateTime.Date.Equals(DateTime.Date));
            Assert.AreEqual(filtered, FilterList.ApplyFilters(_offences));
        }

        [Test]
        public void ApplyFilters_ApplyDateFilterAndCybercrimeFilter_ReturnCybercrimeOffencesOnRightDate()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(DateTime, new Location(1.1, 1.1)) { Category = OffenceCategories.Cybercrime },
                new Offence(DateTime.AddDays(1), new Location(1.1, 1.1)) { Category = OffenceCategories.Cybercrime },
                new Offence(DateTime, new Location(1.1, 1.1)) { Category = OffenceCategories.Drugs }
            };
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Cybercrime));
            FilterList.AddFilter(new DateFilter(DateTime));
            List<Offence> filtered = _offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime) && x.DateTime.Date.Equals(DateTime.Date));
            Assert.AreEqual(filtered, FilterList.ApplyFilters(_offences));
        }
    }
}
