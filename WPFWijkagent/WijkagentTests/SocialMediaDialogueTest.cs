using Microsoft.Maps.MapControl.WPF;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;
using WijkagentWPF;
using Location = WijkagentModels.Location;

namespace WijkagentTests
{
    [TestFixture]
    class SocialMediaDialogueTest
    {
        private Offence offence_1 = new Offence()
        {
            ID = 2,
            DateTime = new DateTime().ToLocalTime(),
            Description = "een delict..",
            LocationID = new Location(52.501127, 6.0789937)
        };

        private Offence offence_2 = new Offence()
        {
            ID = 3,
            DateTime = new DateTime().ToLocalTime(),
            Description = "een delict..",
            LocationID = new Location(53.504127, 6.0789437)
        };

        Location location = new Location(52.501127, 6.0789937);

        /// <summary>
        /// Tests of the retrieveOffence functions finds the right offence
        /// </summary>
        [Test]
        public void RetrieveOffence_OffenceGiven_ReturnsCorrectOffence()
        {
            //arrange
            List<Offence> items = new List<Offence>() { offence_1, offence_2 };
            SocialMediaDialogueController sd = new SocialMediaDialogueController(location, items);
            //act
            int id = sd.RetrieveOffence().ID;
            //Assert
            Assert.AreEqual(offence_1.ID, id);
        }
    }
}
