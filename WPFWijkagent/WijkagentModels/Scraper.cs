using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace WijkagentModels
{
    public class Scraper
    {
        // region containing the tokens & Keys required for the functionality of the TwitterAPI
        #region Keys&Tokens
        private static readonly string customerKey = "qwR0YaAerXPeXtrT99scdSnU1";
        private static readonly string customerKeySecret = "5LeIYLIUh8s0G8oPRSXGnyuIyGLGM6yaISz8kmoFZT0siQFXI9";
        private static readonly string accesToken = "1194224344144269312-cchFiH1xJGQVDsLryEhSWZVp17iTKx";
        private static readonly string accesTokenSecret = "YTiE5MZyYhD1tYchjFPT47QF3QqkO36UGL9tq9Yd3ivby";
        #endregion;

        // containing Test Methods to test network Connectivity
        #region Test
        public string GetUsername()
        {
            Console.WriteLine($"{DateTime.Now} Bot started");
            Connect();
            string data = User.GetAuthenticatedUser().ToString();
            return data;
        }

        public object GetUser()
        {
            Console.WriteLine($"{DateTime.Now} Bot started");
            var exp = Auth.SetUserCredentials(customerKey, customerKeySecret, accesToken, accesTokenSecret);
            return exp;            
        }
        #endregion

        public static void Connect()
        {
            Auth.SetUserCredentials(customerKey, customerKeySecret, accesToken, accesTokenSecret);
        }

        public static List<SocialMediaMessage> GetSocialMediaMessages(Offence offence)
        {
            Connect();
            List<SocialMediaMessage> feed = new List<SocialMediaMessage>();
            var SearchParameters = new SearchTweetsParameters("")
            {
                GeoCode = new GeoCode(offence.LocationID.Latitude, offence.LocationID.Longitude, 0.5, DistanceMeasure.Kilometers),
                SearchType = SearchResultType.Recent,
                Lang = LanguageFilter.Dutch,
                MaximumNumberOfResults = 10,
                Until = offence.Time,
            };
            var tweets = Search.SearchTweets(SearchParameters);
            foreach (var tweet in tweets)
            {
                SocialMediaMessage s = new SocialMediaMessage((int)tweet.Id, tweet.CreatedAt, tweet.Text, new Location(tweet.Coordinates.Latitude, tweet.Coordinates.Longitude));
                Console.WriteLine(s);
                feed.Add(s);
            }
            return feed;
        }
    }
}
