using Microsoft.Maps.MapControl.WPF;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WijkagentModels;
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
            WijkagentWPF.MainWindowController.ClearOffences();
        }

        [Test]
        public void GetOffencesByCategory_AllesTonen_ReturnsAllOffences()
        {
            WijkagentWPF.MainWindowController.AddOffence("Offence1", OffenceCategories.Cybercrime, _dateTime, _location);
            WijkagentWPF.MainWindowController.AddOffence("Offence2", OffenceCategories.Drugs, _dateTime, _location);

            Assert.AreEqual(2, WijkagentWPF.MainWindowController.GetOffencesByCategory("Alles tonen").Count);
        }

        [Test]
        public void GetOffencesByCategory_Cybercrime_ReturnsOneOffence()
        {
            WijkagentWPF.MainWindowController.AddOffence("Offence1", OffenceCategories.Cybercrime, _dateTime, _location);
            WijkagentWPF.MainWindowController.AddOffence("Offence2", OffenceCategories.Drugs, _dateTime, _location);

            Assert.AreEqual(1, WijkagentWPF.MainWindowController.GetOffencesByCategory("Cybercrime").Count);
        }

        [Test]
        public void AddOffence_EmptyController_AddsOffenceToController()
        {
            WijkagentWPF.MainWindowController.AddOffence("Offence1", OffenceCategories.Cybercrime, _dateTime, _location);

            Offence offence = WijkagentWPF.MainWindowController.GetOffences()[0];
            Assert.AreEqual(_dateTime, offence.DateTime);
            Assert.AreEqual(_location, offence.LocationID);
        }

        [Test]
        public void GetOffences_EmptyController_ReturnsEmptyList()
        {
            Assert.IsEmpty(WijkagentWPF.MainWindowController.GetOffences());
        }

        public void GetOffences_ControllerHasAnOffence_ReturnsListWithOneOffence()
        {
            WijkagentWPF.MainWindowController.AddOffence("Offence1", OffenceCategories.Cybercrime, _dateTime, _location);

            Assert.AreEqual(1, WijkagentWPF.MainWindowController.GetOffences().Count);
        }
    }
}
