using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using WijkagentModels;
using WijkagentWPF;

namespace WijkagentTests
{
    [TestFixture]
    public class OffenceControllerTest
    {
        /// <summary>
        /// used in the test cases for seting the offence
        /// </summary>
        public static DateTime dateTime = new DateTime().ToLocalTime();
        public static Location location = new Location() {Latitude=1.1d, Longitude=2.1d};

        [Test]
        public void GetOffences_WithResults_ReturnsOffencesList()
        {
            OffenceController offenceController = new OffenceController();
            Assert.IsNotNull(offenceController.GetOffences());
        }

        [TestCase("een beschirjving", "diefstal")]
        public void SetOffence_WithFormData_DataSet(string description, string category)
        {
            OffenceController offenceController = new OffenceController();
            offenceController.SetOffenceData(description, OffenceCategories.categorie1, dateTime);

            Assert.AreEqual(offenceController.Offence.Description, description);
            Assert.AreEqual(offenceController.Offence.Category, category);
            Assert.AreEqual(offenceController.Offence.DateTime, dateTime);
        }
        

        [Test]
        public void SetOffence_WithLocation_DataSet()
        {
            OffenceController offenceController = new OffenceController();
            offenceController.SetOffenceData(location);

            Assert.AreEqual(offenceController.Offence.LocationID, location);
        }
        
        [TestCase("beschrijving", "categorie")]
        public void SetOffence_WithAllFields_DataSet(string description, string category)
        {
            OffenceController offenceController = new OffenceController();
            offenceController.SetOffenceData(description, OffenceCategories.categorie1, dateTime, location);

            Assert.AreEqual(offenceController.Offence.Description, description);
            Assert.AreEqual(offenceController.Offence.Category, category);
            Assert.AreEqual(offenceController.Offence.DateTime, dateTime);
            Assert.AreEqual(offenceController.Offence.LocationID, location);
        }

    }
}
