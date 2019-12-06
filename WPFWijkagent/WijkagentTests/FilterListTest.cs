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
    }
}
