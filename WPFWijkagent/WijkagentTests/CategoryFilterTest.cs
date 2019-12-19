using NUnit.Framework;
using System;
using System.Collections.Generic;
using WijkagentModels;
using WijkagentWPF;

namespace WijkagentTests
{
    [TestFixture]
    public class CategoryFilterTest
    {
        [Test]
        public void ApplyOn_Cybercrime_OriginalListUnaltered()
        {
            List<Offence> _offences = new List<Offence>
            {
                new Offence(new DateTime().ToLocalTime(), "", new Location(1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(new DateTime().ToLocalTime(), "", new Location(1.1, 1.1), OffenceCategories.Drugs)
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
                new Offence(new DateTime().ToLocalTime(), "", new Location(1.1, 1.1), OffenceCategories.Cybercrime),
                new Offence(new DateTime().ToLocalTime(), "", new Location(1.1, 1.1), OffenceCategories.Drugs)
            };
            CategoryFilter filter = new CategoryFilter(OffenceCategories.Cybercrime);
            List<Offence> filtered = filter.ApplyOn(_offences);
            Assert.AreEqual(_offences.FindAll(x => x.Category.Equals(OffenceCategories.Cybercrime)), filtered);
        }
    }
}
