using System;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using WijkagentWPF.database;
using WijkagentModels;
using Tweetinvi.Exceptions;

namespace WijkagentWPF
{
    /// <summary>W
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

        /// <summary>
        /// connects to twitter api and catches auth errors
        /// </summary>
        public void Connect()
        {
            ExceptionHandler.SwallowWebExceptions = false;
            try
            {
                Auth.SetUserCredentials(customerKey, customerKeySecret, accesToken, accesTokenSecret);
                //throws error when not authenticated correctly
                User.GetAuthenticatedUser();
            }
            catch (TwitterException)
            {
                // Twitter API Request has been failed; Bad request, network failure or unauthorized request
                Logger.Log.ErrorEventHandler(this);
            }
        }

        /// <summary>
        /// This function uses the search parameters attribute to find tweets that 
        /// </summary>
        /// <param name="offence"></param>
        /// <returns>list of social media messages </returns>
        public void GetSocialMediaMessages()
        {
            Connect();
            
            WijkagentModels.Location location;
            var tweets = Search.SearchTweets(_searchParameters);
            foreach (var tweet in tweets)
            {
                if (tweet.Coordinates != null)
                {
                    location = new WijkagentModels.Location(0, tweet.Coordinates.Latitude, tweet.Coordinates.Longitude);
                }
                else
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
