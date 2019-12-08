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
        [TestCase("Alles tonen")]
        public void wpf_cb_categories_SelectionChanged_ValidList_ChangeListBox(string category)
        {
            List<Offence> offences = WijkagentWPF.MainWindowController.GetOffences();
            List<Offence> offenceListItems = WijkagentWPF.MainWindowController.GetOffencesByCategory(category);

            Assert.IsTrue(offenceListItems.SequenceEqual(offences));
        }
    }
}
