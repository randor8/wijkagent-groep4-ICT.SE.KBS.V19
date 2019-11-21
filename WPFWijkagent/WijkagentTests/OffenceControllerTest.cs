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

        [TestCase("", "")]
        public void SetOffence_WithFormData_Returns(string description, string category)
        {
            OffenceController offenceController = new OffenceController();
            offenceController.SetOffenceData(description, category, dateTime);

            Assert.AreEqual(offenceController.Offence.Description, description);
            Assert.AreEqual(offenceController.Offence.Category, category);
            Assert.AreEqual(offenceController.Offence.DateTime, dateTime);
        }
        
        [TestCase()]
        public void SetOffence_WithLocation_Returns()
        {
            OffenceController offenceController = new OffenceController();
            offenceController.SetOffenceData(location);

            Assert.AreEqual(offenceController.Offence.LocationID, location);
        }
        
        [TestCase("", "")]
        public void SetOffence_WithAllFields_Returns(string description, string category)
        {
            OffenceController offenceController = new OffenceController();
            offenceController.SetOffenceData(description, category, dateTime, location);

            Assert.AreEqual(offenceController.Offence.Description, description);
            Assert.AreEqual(offenceController.Offence.Category, category);
            Assert.AreEqual(offenceController.Offence.DateTime, dateTime);
            Assert.AreEqual(offenceController.Offence.LocationID, location);
        }

    }
}
