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
        private Location location = new Location(52.501127, 6.0789937);

        private Offence offence_1 = new Offence(DateTime.Now, new Location(52.501127, 6.0789937))
        {
            ID = 2,
            Description = "een delict.."
        };

        private Offence offence_2 = new Offence(DateTime.Now, new Location(53.504127, 6.0789437))
        {
            ID = 3,
            Description = "een delict.."
        };

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

        //[Test]
        //public void DisplayMessages_OffencesFound_ReturnsString()
        //{
        //    //arrange
        //    List<OffenceListItem> items = new List<OffenceListItem>() { new OffenceListItem(offence_1), new OffenceListItem(offence_2) };
        //    SocialMediaDialogueController sd = new SocialMediaDialogueController(Pins, items);
        //    //act
        //    string result = "";
        //    //Assert
        //    Assert.GreaterOrEqual(0, result.Length);
        //}

        //[Test]
        //public void MethodName_SituationGiven_ExpectedReturn()
        //{
        //    //arrange

        //    //act

        //    //Assert
        //}
    }
}
