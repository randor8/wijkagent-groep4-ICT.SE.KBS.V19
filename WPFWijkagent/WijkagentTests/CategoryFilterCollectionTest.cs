using NUnit.Framework;
using System;
using System.Collections.Generic;
using WijkagentModels;
using WijkagentWPF.Filters;

namespace WijkagentTests
{
    [TestFixture]
    public class CategoryFilterCollectionTest
    {
        [SetUp]
        public void Setup()
        {
            CategoryFilterCollection.Instance.ShowAll();
        }

        public List<Offence> offences = new List<Offence>()
        {
            new Offence(new DateTime().ToLocalTime(), "", new Location(1.1, 1.1), OffenceCategories.Cybercrime),
            new Offence(new DateTime().ToLocalTime(), "", new Location(1.1, 1.1), OffenceCategories.Drugs),
            new Offence(new DateTime().ToLocalTime().AddDays(1), "", new Location(1.1, 1.1), OffenceCategories.Cybercrime)
        };

        [Test]
        public void ApplyOn_NoFilters_ReturnAllOffences()
        {
            Assert.AreEqual(offences, CategoryFilterCollection.Instance.ApplyOn(offences));
        }

        [Test]
        public void ApplyOn_CybercrimeFilter_ReturnCybercrimeOffences()
        {
            CategoryFilterCollection.Instance.ToggleCategory(OffenceCategories.Cybercrime);
            Assert.AreEqual(offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime)), CategoryFilterCollection.Instance.ApplyOn(offences));
        }

        [Test]
        public void ApplyOn_CybercrimeAndFraudeFilter_ReturnCybercrimeAndFraudeOffences()
        {
            CategoryFilterCollection.Instance.ToggleCategory(OffenceCategories.Cybercrime);
            CategoryFilterCollection.Instance.ToggleCategory(OffenceCategories.Fraude);
            Assert.AreEqual(offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime) || x.Category.Equals(OffenceCategories.Fraude)), CategoryFilterCollection.Instance.ApplyOn(offences));
        }

        [Test]
        public void ShowAll_FraudeFilterActive_ReturnAllOffences()
        {
            CategoryFilterCollection.Instance.ToggleCategory(OffenceCategories.Fraude);
            Assert.AreEqual(offences.FindAll(x => x.Category.Equals(OffenceCategories.Fraude)), CategoryFilterCollection.Instance.ApplyOn(offences));
            CategoryFilterCollection.Instance.ShowAll();
            Assert.AreEqual(offences, CategoryFilterCollection.Instance.ApplyOn(offences));
        }
    }
}
