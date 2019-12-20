using System;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using WijkagentWPF.database;
using System.Configuration;
using System.Windows;

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
        private readonly string _customerKey = ConfigurationManager.AppSettings.Get("customerKey");
        private readonly string _customerKeySecret = ConfigurationManager.AppSettings.Get("customerKeySecret");
        private readonly string _accessToken = ConfigurationManager.AppSettings.Get("accesToken");
        private readonly string _accessTokenSecret = ConfigurationManager.AppSettings.Get("accesTokenSecret");
        #endregion;

        public Scraper(Offence offence)
        {
            Offence = offence;
            _searchParameters = new SearchTweetsParameters(" ")
            {
                GeoCode = new GeoCode(offence.Location.Latitude, offence.Location.Longitude, 1, DistanceMeasure.Kilometers),
                Lang = LanguageFilter.Dutch,
                MaximumNumberOfResults = 10,
                Until = new DateTime(
                    offence.DateTime.Year,
                    offence.DateTime.Month,
                    offence.DateTime.Day + 1),
                Since = new DateTime(
                    offence.DateTime.Year,
                    offence.DateTime.Month,
                    offence.DateTime.Day,
                    offence.DateTime.Hour - 1,
                    offence.DateTime.Minute,
                    offence.DateTime.Second)
            };
        }

        /// <summary>
        /// This function authenticates the user for the Twitter API
        /// </summary>
        private void Connect()
        {
            Auth.SetUserCredentials(_customerKey, _customerKeySecret, _accessToken, _accessTokenSecret);
        }

        /// <summary>
        /// This function uses the search parameters attribute to find tweets that fit the parameters
        /// </summary>
        /// <param name="offence"></param>
        /// <returns>list of social media messages </returns>
        public void SetSocialMediaMessages()
        {
            Connect();
            var tweets = Search.SearchTweets(_searchParameters);
            foreach (var tweet in tweets)
            {
                SetSocialMediaMessage(tweet);
            }
        }

        /// <summary>
        /// Checks and sets a specific value to the DB and adds a Social Media Message
        /// </summary>
        /// <param name="tweet">tweetenvi tweet object</param>
        private void SetSocialMediaMessage(ITweet tweet)
        {
            int locationId = Offence.Location.ID;
            LocationController locationController = new LocationController();
            SocialMediaMessageController socialMediaMessageController = new SocialMediaMessageController();

            if (tweet.Coordinates != null)
            {
                locationId = locationController.SetLocation(new Location(tweet.Coordinates.Latitude, tweet.Coordinates.Longitude));
            }
            int messageID = socialMediaMessageController.SetSocialMediaMessage(
                tweet.CreatedAt,
                tweet.Text,
                tweet.CreatedBy.Name,
                tweet.CreatedBy.ScreenName,
                locationId,
                Offence.ID,
                tweet.Id
            );

            SocialMediaImageController imageController = new SocialMediaImageController();
            foreach (var media in tweet.Media)
            {
                imageController.SetSocialMediaImage(
                    messageID,
                    media.MediaURLHttps
                );
            }
        }

        /// <summary>
        /// Function checks if new social Media Messages have been posted and adds them to the DB
        /// </summary>
        public void UpdateSocialMediaMessages()
        {
            Connect();

            var tweets = Search.SearchTweets(_searchParameters);
            SocialMediaMessageController mediaMessageController = new SocialMediaMessageController();

            foreach (var tweet in tweets)
            {
                if (mediaMessageController.GetSocialMediaMessage(tweet.Id) == null)
                {
                    SetSocialMediaMessage(tweet);
                }
            }
        }
    }
}
