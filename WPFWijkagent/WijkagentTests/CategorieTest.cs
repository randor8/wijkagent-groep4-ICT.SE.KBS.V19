using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<OffenceListItem> offences = offenceController.ConvertListOffenceToOffenceListItem(offenceController.GetOffences());
            List<OffenceListItem> offenceListItems = offenceController.ConvertListOffenceToOffenceListItem(offenceController.GetOffenceDataByCategory(category, offenceController.GetOffences()));

            Assert.IsTrue(offenceListItems.SequenceEqual(offences));
        }
    }
}
