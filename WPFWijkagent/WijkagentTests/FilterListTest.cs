using NUnit.Framework;
using System;
using System.Collections.Generic;
using WijkagentModels;
using WijkagentWPF;
using WijkagentWPF.Filters;

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
            CategoryFilterCollection.Instance.ShowAll();
        }

        [Test]
        public void AddFilter_AddCategoryFilterToEmptyList_AddsFilterToList()
        {
            FilterList.AddFilter(CategoryFilterCollection.Instance);
            Assert.Contains(CategoryFilterCollection.Instance, FilterList.GetFilters());
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
            FilterList.AddFilter(CategoryFilterCollection.Instance);
            Assert.IsTrue(FilterList.GetFilters().Count == 2);
        }

        [Test]
        public void AddFilter_FilterListWithSameFilter_FilterNotAddedToList()
        {
            DateFilter dateFilter = new DateFilter(DateTime);
            DateFilter dateFilter1 = new DateFilter(DateTime);
            FilterList.AddFilter(dateFilter);
            FilterList.AddFilter(dateFilter1);
            Assert.IsTrue(FilterList.GetFilters().Count == 1);
            Assert.AreEqual(dateFilter, FilterList.GetFilters()[0]);
        }

        [Test]
        public void RemoveFilter_ListWith2Filters_RemovedFilterNoLongerInList()
        {
            DateFilter dateFilter = new DateFilter(DateTime);
            CategoryFilterCollection.Instance.ToggleCategory(OffenceCategories.Cybercrime);
            FilterList.AddFilter(CategoryFilterCollection.Instance);
            FilterList.AddFilter(dateFilter);
            Assert.IsTrue(FilterList.GetFilters().Count == 2);
            FilterList.RemoveFilter(dateFilter);
            Assert.IsTrue(FilterList.GetFilters().Count == 1);
            Assert.IsFalse(FilterList.GetFilters().Contains(dateFilter));
        }

        [Test]
        public void ClearFilters_ListWith2Filters_FilterListIsEmpty()
        {
            FilterList.AddFilter(new DateFilter(DateTime));
            FilterList.AddFilter(new DateFilter(DateTime));
            FilterList.ClearFilters();
            Assert.IsEmpty(FilterList.GetFilters());
        }

        [Test]
        public void ApplyFilters_ApplyCybercrimeFilter_ReturnCybercrimeOffences()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(1, new DateTime().ToLocalTime(), "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(2, new DateTime().ToLocalTime(), "", new Location(1, 1.1, 1.1), OffenceCategories.Drugs)
            };
            FilterList.AddFilter(CategoryFilterCollection.Instance);
            CategoryFilterCollection.Instance.ToggleCategory(OffenceCategories.Cybercrime);
            Assert.AreEqual(_offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime)), FilterList.ApplyFilters(_offences));
        }

        [Test]
        public void ApplyFilters_ApplyCybercrimeAndDrugsFilter_ReturnCybercrimeAndDrugsOffences()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(1, new DateTime().ToLocalTime(), "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(2, new DateTime().ToLocalTime(), "", new Location(1, 1.1, 1.1), OffenceCategories.Drugs),
                new Offence(3, new DateTime().ToLocalTime(), "", new Location(1, 1.1, 1.1), OffenceCategories.Fraude)
            };
            FilterList.AddFilter(CategoryFilterCollection.Instance);
            CategoryFilterCollection.Instance.ToggleCategory(OffenceCategories.Cybercrime);
            CategoryFilterCollection.Instance.ToggleCategory(OffenceCategories.Drugs);
            List<Offence> filtered = _offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime) || x.Category.Equals(OffenceCategories.Drugs));
            Assert.AreEqual(filtered, FilterList.ApplyFilters(_offences));
        }

        [Test]
        public void ApplyFilters_ApplyDateFilter_ReturnOffencesFromDate()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(1, DateTime, "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(2, DateTime.AddDays(1), "", new Location(1, 1.1, 1.1), OffenceCategories.Drugs)
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
                new Offence(1, DateTime, "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(2, DateTime.AddDays(1), "", new Location(1, 1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(3, DateTime, "", new Location(1, 1.1, 1.1), OffenceCategories.Drugs)
            };
            FilterList.AddFilter(CategoryFilterCollection.Instance);
            CategoryFilterCollection.Instance.ToggleCategory(OffenceCategories.Cybercrime);
            FilterList.AddFilter(new DateFilter(DateTime));
            List<Offence> filtered = _offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime) && x.DateTime.Date.Equals(DateTime.Date));
            Assert.AreEqual(filtered, FilterList.ApplyFilters(_offences));
        }
    }
}
