using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Tweetinvi;
using WijkagentModels;

namespace WijkagentWPF
{
    public class WitnessController
    {
        private Offence _Offence { get; set; }
        private AskForWitnessWindow witnessWindow;

        public WitnessController(Offence offence, MainWindow window)
        {
            witnessWindow = new AskForWitnessWindow(this, window);
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
        /// Insert the created hashtag in the database
        /// </summary>
        public void SendTweet(MainWindow window)
        {
            DBContext dBContext = new DBContext();
            Scraper scraper = new Scraper(_Offence);
            scraper.Connect();
            Tweet.PublishTweet(CreateWitnessMessage());
            SqlCommand query = new SqlCommand("UPDATE Offence SET Hashtag = @Hashtag WHERE ID = @OffenceID");

            //prepare values in statement 
            query.Parameters.Add("@OffenceID", System.Data.SqlDbType.Int);
            query.Parameters.Add("@Hashtag", System.Data.SqlDbType.NVarChar);

            query.Parameters["@Hashtag"].Value = $"Delict{_Offence.ID}";
            query.Parameters["@OffenceID"].Value = _Offence.ID;

            dBContext.ExecuteQuery(query);
            _Offence.CallHashtag = $"#Delict{_Offence.ID}";
            Scraper WitnessScraper = new Scraper(_Offence, true, window.Hashtag(_Offence)); ;
            WitnessScraper.UpdateSocialMediaMessages(1);
        }
    }
}
