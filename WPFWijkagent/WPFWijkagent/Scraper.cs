﻿using System;
using System.Collections.Generic;
using System.Configuration;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using WijkagentModels;
using WijkagentWPF.database;

namespace WijkagentWPF
{
    /// <summary>
    /// The class is responsible for fetching twitter messages and converting them to social Media Messages
    /// </summary>
    public class Scraper
    {
        public Offence Offence { get; private set; }

        private SearchTweetsParameters _searchParameters;
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
                Until = new DateTime(offence.DateTime.Year, offence.DateTime.Month, offence.DateTime.Day).AddDays(1),
                Since = new DateTime(offence.DateTime.Year, offence.DateTime.Month, offence.DateTime.Day, offence.DateTime.Hour, offence.DateTime.Minute, offence.DateTime.Second).AddHours(-1)
            };
        }

        /// <summary>
        /// Connects to twitter api and catches auth errors.
        /// </summary>
        public void Connect()
        {
            ExceptionHandler.SwallowWebExceptions = false;
            try
            {
                Auth.SetUserCredentials(_customerKey, _customerKeySecret, _accessToken, _accessTokenSecret);
                User.GetAuthenticatedUser(); //throws error when not authenticated correctly
            }
            catch (Exception) { }
        }

        /// <summary>
        /// This function uses the search parameters attribute to find tweets that fit the parameters.
        /// </summary>
        /// <param name="offence"></param>
        /// <returns>list of social media messages </returns>
        public void SetSocialMediaMessages()
        {
            try
            {
                Connect();
                var tweets = Search.SearchTweets(_searchParameters);
                foreach (var tweet in tweets)
                {
                    SetSocialMediaMessage(tweet);
                }
            }
            catch (Exception)
            {
                // Twitter API Request has failed; Bad request, network failure or unauthorized request
                Logger.Log.ErrorEventHandler(this);
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

            if (tweet.Coordinates != null) locationId = locationController.SetLocation(new WijkagentModels.Location(tweet.Coordinates.Latitude, tweet.Coordinates.Longitude));
            int messageID = socialMediaMessageController.SetSocialMediaMessage(tweet.CreatedAt, tweet.Text, tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, locationId, Offence.ID, tweet.Id);

            SocialMediaImageController imageController = new SocialMediaImageController();
            foreach (var media in tweet.Media)
            {
                imageController.SetSocialMediaImage(new SocialMediaImage
                {
                    SocialMediaMessageID = messageID,
                    URL = media.MediaURLHttps
                });
            }
        }

        /// <summary>
        /// Returns all the direct messages, that are known on the Twitter account
        /// </summary>
        /// <returns>Ienumerable containg all Dirext Messages</returns>
        public IEnumerable<IMessage> GetLatestDirectMessages()
        {
            try
            {
                Connect();
                IEnumerable<IMessage> LatestMessages = Message.GetLatestMessages();
                return LatestMessages;
            }
            catch (Exception)
            {
                Logger.Log.ErrorEventHandler(this);
                return null;
            }
        }

        /// <summary>
        /// Sents a direct message to a user via de twitter API
        /// </summary>
        /// <param name="input"> The content of the message</param>
        /// <param name="id">The id of the person who recieves the message</param>
        public void SentDirectMessage(string input, long id)
        {
            Connect();
            Message.PublishMessage(input, id);
        }

        /// <summary>
        /// Function checks if new social Media Messages have been posted and adds them to the DB
        /// </summary>
        public void UpdateSocialMediaMessages()
        {
            try
            {
                Connect();
                var tweets = Search.SearchTweets(_searchParameters);
                SocialMediaMessageController mediaMessageController = new SocialMediaMessageController();

                foreach (var tweet in tweets)
                {
                    if (mediaMessageController.GetTweetSocialMediaMessage(tweet.Id) == null)
                    {
                        SetSocialMediaMessage(tweet);
                    }
                }
            }
            catch (Exception)
            {
                // Twitter API Request has been failed; Bad request, network failure or unauthorized request
                Logger.Log.ErrorEventHandler(this);
            }
        }
    }
}
