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
        [SetUp]
        public void Setup()
        {
            FilterList.ClearFilters();
        }

        [Test]
        public void AddFilter_EmptyFilterList_AddsFilterToList()
        {
            CategoryFilter CybercrimeFilter = new CategoryFilter(OffenceCategories.Cybercrime);
            FilterList.AddFilter(CybercrimeFilter);
            Assert.Contains(CybercrimeFilter, FilterList.GetFilters());
        }

        [Test]
        public void AddFilter_FilterListWith1DifferentFilter_AddsFilterToList()
        {
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Drugs));
            CategoryFilter CybercrimeFilter = new CategoryFilter(OffenceCategories.Cybercrime);
            FilterList.AddFilter(CybercrimeFilter);
            Assert.IsTrue(FilterList.GetFilters().Count == 2);
        }

        [Test]
        public void AddFilter_FilterListWithSameFilter_FilterNotAddedToList()
        {
            CategoryFilter CybercrimeFilter1 = new CategoryFilter(OffenceCategories.Cybercrime);
            FilterList.AddFilter(CybercrimeFilter1);
            Assert.Contains(CybercrimeFilter1, FilterList.GetFilters());
            CategoryFilter CybercrimeFilter2 = new CategoryFilter(OffenceCategories.Cybercrime);
            FilterList.AddFilter(CybercrimeFilter2);
            Assert.IsTrue(FilterList.GetFilters().Count == 1);
            Assert.AreEqual(CybercrimeFilter1, FilterList.GetFilters()[0]);
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
                new Offence() { Category = OffenceCategories.Cybercrime },
                new Offence() { Category = OffenceCategories.Drugs }
            };
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Cybercrime));
            Assert.AreEqual(_offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime)), FilterList.ApplyFilters(_offences));
        }

        [Test]
        public void ApplyFilters_ApplyCybercrimeAndDrugsFilter_ReturnCybercrimeAndDrugsOffences()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence() { Category = OffenceCategories.Cybercrime },
                new Offence() { Category = OffenceCategories.Drugs },
                new Offence() { Category = OffenceCategories.Fraude }
            };
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Cybercrime));
            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Drugs));
            List<Offence> filtered = _offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime) || x.Category.Equals(OffenceCategories.Drugs));
            List<Offence> filtered2 = FilterList.ApplyFilters(_offences);
            Assert.AreEqual(filtered, filtered2);
        }
    }
}
