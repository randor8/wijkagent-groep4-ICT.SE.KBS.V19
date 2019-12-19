using NUnit.Framework;
using System;
using System.Collections.Generic;
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

        [Test]
        public void SentDirectMessage_IDisCorrect_returnsTrue()
        {
            //arrange
            //act
            bool result = Scraper.SendDirectMessage(1206933407655243778);
            //asset
            Assert.IsTrue(result);
        }

    }
}
