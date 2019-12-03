using System;
using NUnit.Framework;
using WijkagentModels;
using WijkagentWPF;
using WijkagentWPF.database;

namespace WijkagentTests
{
    [TestFixture]
    public class OffenceControllerTest
    {
        /// <summary>
        /// used in the test cases for seting the offence
        /// </summary>
        public static DateTime dateTime = new DateTime().ToLocalTime();
        public static Location location = new Location(1.1, 2.1);

        [Test]
        public void GetOffences_WithResults_ReturnsOffencesList()
        {
            depOffenceController offenceController = new depOffenceController();
            Assert.IsNotNull(offenceController.GetOffences());
        }

        [Test]
        public void SetOffence_WithLocationAsID_ReturnsID()
        {
            OffenceController offenceController = new OffenceController();
            Assert.IsNotNull(offenceController);
        }

        [Test]
        public void SetOffence_WithLocationAsObj_ReturnsID()
        {

        }
    }
}
