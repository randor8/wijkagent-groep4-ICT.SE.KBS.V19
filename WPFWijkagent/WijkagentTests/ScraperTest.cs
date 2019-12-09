using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;
using WijkagentModels;
using Location = WijkagentModels.Location;

namespace WijkagentTests
{
    [TestFixture]
    class ScraperTest
    {
        /// <summary>
        /// Test to check the user and connection In the API
        /// </summary>
        [Test]
        public void GetUsername_ConnectionCorrect_ReturnsString()
        {
            //arrange
            //act
            String result = Scraper.GetUsername();
            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual("J. Oltmans", result);
        }
        /// <summary>
        /// Test to check whether the authentication is correct
        /// </summary>
        [Test]
        public void GetUser_AuthenticationDone_ReturnsValue()
        {
            //arrange
            //act
            object result = Scraper.GetUser();
            //assert
            Assert.IsNotNull(result);
        }
        /// <summary>
        /// A test to control whether the function returns a list filled with results
        /// </summary>
        [Test]
        public void GetSocialMediaMessages_offenceGiven_IsInstance()
        {
            //arrange
            Offence offence = new Offence(3,
                DateTime.Now,
                "een delict..",
                new Location(0, 52.37317168, 4.8936582),
                OffenceCategories.Cybercrime);

            Scraper scraper = new Scraper(offence);
            //act
            List<SocialMediaMessage> socials = scraper.GetSocialMediaMessages();
            //assert
            Assert.Greater(socials.Count, 0);

        }
    }
}
