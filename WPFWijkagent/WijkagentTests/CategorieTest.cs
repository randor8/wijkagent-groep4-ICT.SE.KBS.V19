using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WijkagentModels;
using WijkagentWPF;

namespace WijkagentTests
{
    [TestFixture]
    public class CategorieTest
    {
        OffenceController offenceController = new OffenceController();
        [TestCase("Alles tonen")]
        public void wpf_cb_categories_SelectionChanged_ValidList_ChangeListBox(string category)
        {
            List<Offence> offences = offenceController.GetOffences();
            List<Offence> offenceListItems = offenceController.GetOffenceDataByCategory(category, offenceController.GetOffences());

            Assert.IsTrue(offenceListItems.SequenceEqual(offences));
        }
    }
}
