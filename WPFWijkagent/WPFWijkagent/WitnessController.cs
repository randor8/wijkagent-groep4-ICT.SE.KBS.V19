using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;
using WijkagentModels;

namespace WijkagentWPF
{
    public class WitnessController
    {
        private Offence _Offence { get; set; }
        private AskForWitnessWindow witnessWindow;

        public WitnessController(Offence offence)
        {
            witnessWindow = new AskForWitnessWindow(this);
            _Offence = offence;
            witnessWindow.txtb_omschrijving.Text = _Offence.Description;
            witnessWindow.ShowDialog();
        }

        /// <summary>
        /// create a costum message to post to twitter using the Offence information and user input
        /// </summary>
        /// <returns>A custom message ready for twitter.</returns>
        public string CreateWitnessMessage()
        {
            return new string($"Delict {_Offence.ID},\n{witnessWindow.txtb_omschrijving.Text}" +
                $" op {_Offence.DateTime} in de buurt van " +
                $"https://bing.com/maps/default.aspx?cp={_Offence.Location.Latitude}~{_Offence.Location.Longitude}&lvl=20&style=h&sp=point.{_Offence.Location.Latitude}_{_Offence.Location.Longitude}_Delict{_Offence.ID}" +
                $"\nTweet of retweet met de hashtag #Delict{_Offence.ID} als u meer weet.");
        }

        /// <summary>
        /// connect to the twitter bot and publish the custom tweet.
        /// </summary>
        public void SendTweet()
        {
            Scraper.Connect();
            Tweet.PublishTweet(CreateWitnessMessage());
        }
    }
}
