using NUnit.Framework;
using System;
using WijkagentWPF;
using Location = WijkagentModels.Location;

namespace WijkagentTests
{
    [TestFixture]
    public class MainWindowControllerTest
    {
        private static DateTime _dateTime = new DateTime().ToLocalTime();
        private static Location _location = new Location(1.1, 1.1);

        [SetUp]
        public void SetUp()
        {
            MainWindowController.ClearOffences();
            FilterList.ClearFilters();
        }

        /*[Test]
        public void GetOffencesByCategory_AllesTonen_ReturnsAllOffences()
        {
            MainWindowController.AddOffence("Offence1", OffenceCategories.Cybercrime, _dateTime, _location);
            MainWindowController.AddOffence("Offence2", OffenceCategories.Drugs, _dateTime, _location);

            FilterList.AddFilter(new CategoryFilter(OffenceCategories.Cybercrime));
            Assert.AreEqual(1, MainWindowController.FilterOffences().Count);
        }

        [Test]
        public void AddOffence_EmptyController_AddsOffenceToController()
        {
            MainWindowController.AddOffence("Offence1", OffenceCategories.Cybercrime, _dateTime, _location);

            Offence offence = MainWindowController.GetOffences()[0];
            Assert.AreEqual(_dateTime, offence.DateTime);
            Assert.AreEqual(_location, offence.LocationID);
        }

        [Test]
        public void GetOffences_EmptyController_ReturnsEmptyList()
        {
            Assert.IsEmpty(MainWindowController.GetOffences());
        }

        public void GetOffences_ControllerHasAnOffence_ReturnsListWithOneOffence()
        {
            MainWindowController.AddOffence("Offence1", OffenceCategories.Cybercrime, _dateTime, _location);

            Assert.AreEqual(1, WijkagentWPF.MainWindowController.GetOffences().Count);
        }*/
    }
}
