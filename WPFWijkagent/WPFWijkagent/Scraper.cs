using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using WijkagentModels;

namespace WijkagentModels
{
    /// <summary>
    /// The class is responsible for fetching twitter messages and converting them to social Media Messages
    /// </summary>
    public class Scraper
    {
        public Offence Offence { get; set; }

        SearchTweetsParameters SearchParameters;

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
            DateTime dateTime = new DateTime(offence.DateTime.Year, offence.DateTime.Month, offence.DateTime.Day);
            SearchParameters = new SearchTweetsParameters("")
            {
                GeoCode = new GeoCode(offence.LocationID.Latitude, offence.LocationID.Longitude, 1, DistanceMeasure.Kilometers),
                Lang = LanguageFilter.Dutch,
                MaximumNumberOfResults = 10,
                Until = dateTime,
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
        public List<SocialMediaMessage> GetSocialMediaMessages()
        {
            Connect();
            List<SocialMediaMessage> feed = new List<SocialMediaMessage>();
            var tweets = Search.SearchTweets(SearchParameters);
            foreach (var tweet in tweets)
            {
                //TODO: Add User information
                Console.WriteLine(tweet.CreatedBy+"\n");
                feed.Add(new SocialMediaMessage((int)tweet.Id, tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, tweet.CreatedAt, tweet.Text, Offence.LocationID));
            }
            return feed;
        }
    }
}
