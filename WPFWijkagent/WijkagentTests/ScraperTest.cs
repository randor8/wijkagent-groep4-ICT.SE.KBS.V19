using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;
using WijkagentModels;

namespace WijkagentTests
{
    [TestFixture]
    class ScraperTest
    {
        [Test]
        public void GetUsername_ConnectionCorrect_ReturnsString()
        {
            //arrange
            Scraper scraper = new Scraper();
            //act
            String result = scraper.GetUsername();
            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual("J. Oltmans", result);
        }

        [Test]
        public void GetUser_AuthenticationDone_ReturnsValue()
        {
            //arrange
            Scraper scraper = new Scraper();
            //act
            object result = scraper.GetUser();
            //assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetSocialMediaMessages_offenceGiven_ReturnsList()
        {
            //arrange
            Offence offence = new Offence()
            {
                ID = 3,
                Time = new DateTime().ToLocalTime(),
                Description = "een delict..",
                LocationID = new Location(52.501127, 6.0789937)
            };
            //act
            Scraper.Connect();
            Tweet.PublishTweet("Let's do the Time Warp Again!!");
            List<SocialMediaMessage> result = Scraper.GetSocialMediaMessages(offence);
            
            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }
    }
}
