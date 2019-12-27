using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Tweetinvi;
using WijkagentModels;
using WijkagentWPF.database;

namespace WijkagentWPF
{
    public class WitnessController
    {
        public Offence _Offence { get; set; }
        private AskForWitnessWindow _WitnessWindow;
        private SocialMediaMessageController SocialMediaMessageController = new SocialMediaMessageController();
        private SendMessageController SendMessageController = new SendMessageController();

        public WitnessController(Offence offence, AskForWitnessWindow witnessWindow)
        {
            _Offence = offence;
            _WitnessWindow = witnessWindow;
        }

        /// <summary>
        /// create a costum message to post to twitter using the Offence information and user input
        /// </summary>
        /// <returns>A custom message ready for twitter.</returns>
        public string CreateWitnessMessage()
        {
            if (!_Offence.ID.Equals(null) || !_Offence.ID.Equals(0))
            {
                return new string($"Delict {_Offence.ID},\n{_WitnessWindow.txtb_omschrijving.Text}" +
                $" op {_Offence.DateTime} in de buurt van " +
                $"https://bing.com/maps/default.aspx?cp={_Offence.Location.Latitude}~{_Offence.Location.Longitude}&lvl=20&style=h&sp=point.{_Offence.Location.Latitude}_{_Offence.Location.Longitude}_Delict{_Offence.ID}" +
                $"\nTweet of retweet met de hashtag #Delict{_Offence.ID} als u meer weet.");
            }
            
            else
            {
                return null;
            }
        }

        

        /// <summary>
        /// Publish a tweet, Update the database  and update the socialmediamessages
        /// </summary>
        public void SendTweet()
        {
            DBContext dBContext = new DBContext();
            Scraper scraper = new Scraper(_Offence);
            scraper.Connect();

            Tweet.PublishTweet(CreateWitnessMessage());
            SocialMediaMessageController.UpdateHashtag(_Offence);
            SendMessageController.UpdateSendMessages(_Offence, this.CreateWitnessMessage());

            _Offence.CallHashtag = $"#Delict{_Offence.ID}";

            Scraper WitnessScraper = new Scraper(_Offence, true, SocialMediaMessageController.Hashtag(_Offence)); ;
            WitnessScraper.UpdateSocialMediaMessages(1);
        }
    }
}
