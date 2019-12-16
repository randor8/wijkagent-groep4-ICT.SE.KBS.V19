using NUnit.Framework;
using System;
using System.Collections.Generic;
using WijkagentModels;
using WijkagentWPF;
using Location = WijkagentModels.Location;

namespace WijkagentTests
{
    [TestFixture]
    class SocialMediaDialogueTest
    {
        private Offence offence_1 = new Offence(
            2,
            new DateTime().ToLocalTime(),
            "een delict..",
            new Location(0, 52.501127, 6.0789937), 
            OffenceCategories.Null);
        private Offence offence_2 = new Offence(
            3,
            new DateTime().ToLocalTime(),
            "nog een delict..",
            new Location(0, 53.504127, 6.0789437), 
            OffenceCategories.Null);
        private Location location = new Location(0, 52.501127, 6.0789937);

        /// <summary>
        /// Tests of the retrieveOffence functions finds the right offence
        /// </summary>
        [Test]
        public void RetrieveOffence_OffenceGiven_ReturnsCorrectOffence()
        {
            //arrange
            List<Offence> items = new List<Offence>() { offence_1, offence_2 };
            DelictDialog sd = new DelictDialog(location, items);
            //act
            int id = sd.RetrieveOffence().ID;
            //Assert
            Assert.AreEqual(offence_1.ID, id);
        }
    }
}
