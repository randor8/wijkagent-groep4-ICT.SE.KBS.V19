using System;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using WijkagentWPF.database;


namespace WijkagentModels
{
    /// <summary>
    /// The class is responsible for fetching twitter messages and converting them to social Media Messages
    /// </summary>
    public class Scraper
    {
        public Offence Offence { get; set; }

        private SearchTweetsParameters _searchParameters;

        // region containing the tokens & Keys required for the functionality of the TwitterAPI
        #region Keys&Tokens
        private static readonly string customerKey = "qwR0YaAerXPeXtrT99scdSnU1";
        private static readonly string customerKeySecret = "5LeIYLIUh8s0G8oPRSXGnyuIyGLGM6yaISz8kmoFZT0siQFXI9";
        private static readonly string accesToken = "1194224344144269312-cchFiH1xJGQVDsLryEhSWZVp17iTKx";
        private static readonly string accesTokenSecret = "YTiE5MZyYhD1tYchjFPT47QF3QqkO36UGL9tq9Yd3ivby";
        #endregion;

        public Scraper(Offence offence)
        {
            Offence = offence;
            _searchParameters = new SearchTweetsParameters(" ")
            {
                GeoCode = new GeoCode(offence.LocationID.Latitude, offence.LocationID.Longitude, 1, DistanceMeasure.Kilometers),
                Lang = LanguageFilter.Dutch,
                MaximumNumberOfResults = 10,
                Until = new DateTime(offence.DateTime.Year, offence.DateTime.Month, offence.DateTime.Day),
                Since = new DateTime(offence.DateTime.Year, offence.DateTime.Month, offence.DateTime.Day - 1)
            };
        }

        // containing Test Methods to test network Connectivity
        #region Test
        public static string GetUsername()
        {
            Console.WriteLine($"{DateTime.Now} Bot started");
            Connect();
            string data = User.GetAuthenticatedUser().ToString();
            return data;
        }

        public static object GetUser()
        {
            Console.WriteLine($"{DateTime.Now} Bot started");
            var exp = Auth.SetUserCredentials(customerKey, customerKeySecret, accesToken, accesTokenSecret);
            return exp;
        }
        #endregion

        // bassfunctie om de connectie te maken tussen de API en het autorizeren
        public static void Connect()
        {
            Auth.SetUserCredentials(customerKey, customerKeySecret, accesToken, accesTokenSecret);
        }

        /// <summary>
        /// This function uses the search parameters attribute to find tweets that 
        /// </summary>
        /// <param name="offence"></param>
        /// <returns>list of social media messages </returns>
        public void GetSocialMediaMessages()
        {
            Connect();
            Location location;
            var tweets = Search.SearchTweets(_searchParameters);
            foreach (var tweet in tweets)
            {
                if (tweet.Coordinates != null)
                {
                    location = new Location(0, tweet.Coordinates.Latitude, tweet.Coordinates.Longitude); 
                } else
                {
                    location = Offence.LocationID;
                }
                SocialMediaMessageController socialMediaMessageController = new SocialMediaMessageController();
                socialMediaMessageController.SetSocialMediaMessage(
                    tweet.CreatedAt, 
                    tweet.Text, 
                    tweet.CreatedBy.Name, 
                    tweet.CreatedBy.ScreenName, 
                    location, 
                    Offence.ID);
            }
        }
    }
}
