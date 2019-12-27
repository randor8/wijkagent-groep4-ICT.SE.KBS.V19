using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WijkagentModels;

namespace WijkagentWPF.database
{
    public class SocialMediaMessageController
    {
        private DBContext _dbContext { get; set; }

        public SocialMediaMessageController()
        {
            _dbContext = new DBContext();
        }

        /// <summary>
        /// inserts the socialmedia message in the db
        /// </summary>
        /// <param name="socialMediaMessage">SocialMediaMessage object</param>
        /// <returns>returns the inserted id that has been added</returns>
        public int SetSocialMediaMessage(SocialMediaMessage socialMediaMessage)
        {
            //when no location id reference is present.
            if (socialMediaMessage.Location.ID == 0)
            {
                socialMediaMessage.Location.ID = new LocationController().SetLocation(
                    new Location(
                    socialMediaMessage.Location.Latitude,
                    socialMediaMessage.Location.Longitude));
            }

            SqlCommand query = new SqlCommand("" +
                "INSERT INTO SocialMediaMessage (DateTime, Message, Username, Handle, locationID, TwitterID, MediaType, OffenceID) " +
                "OUTPUT INSERTED.ID " +
                "VALUES(@DateTime, @message, @user, @handle, @LocationID, @TwitterID, @MediaType, @OffenceID)");

            //prepare values in statement 
            query.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime);
            query.Parameters.Add("@message", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@user", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@handle", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@LocationID", System.Data.SqlDbType.Int);
            query.Parameters.Add("@TwitterID", System.Data.SqlDbType.BigInt);
            query.Parameters.Add("@OffenceID", System.Data.SqlDbType.Int);
            query.Parameters.Add("@MediaType", System.Data.SqlDbType.Int);

            query.Parameters["@DateTime"].Value = socialMediaMessage.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            query.Parameters["@message"].Value = socialMediaMessage.Message;
            query.Parameters["@user"].Value = socialMediaMessage.User;
            query.Parameters["@handle"].Value = socialMediaMessage.Handle;
            query.Parameters["@LocationID"].Value = socialMediaMessage.Location.ID;
            query.Parameters["@TwitterID"].Value = socialMediaMessage.TwitterID;
            query.Parameters["@OffenceID"].Value = socialMediaMessage.Offence.ID;
            query.Parameters["@MediaType"].Value = socialMediaMessage.MediaType;

            return _dbContext.ExecuteInsertQuery(query);
        }

        /// <summary>
        /// converts database row to an actual SocialMediaMessage object
        /// </summary>
        /// <param name="socialMediaMessageData">raw data from the database(7 fields)</param>
        /// <returns>the SocialMediaMessage object</returns>
        private SocialMediaMessage ObjectArrayToSocialMediaMessage(object[] socialMediaMessageData)
        {
            object[] array = socialMediaMessageData;
            //get datetime fields
            string[] dateTimeStrings = socialMediaMessageData[0].ToString().Split(DBContext.DateTimeSeparator);

            //parse datetime fields
            int.TryParse(dateTimeStrings[0], out int dd);
            int.TryParse(dateTimeStrings[1], out int mm);
            int.TryParse(dateTimeStrings[2], out int yyyy);
            int.TryParse(dateTimeStrings[3], out int hh);
            int.TryParse(dateTimeStrings[4], out int min);
            int.TryParse(dateTimeStrings[5], out int ss);

            return new SocialMediaMessage(
                new DateTime(yyyy, mm, dd, hh, min, ss),
                socialMediaMessageData[1].ToString(),
                socialMediaMessageData[2].ToString(),
                socialMediaMessageData[3].ToString(),
                new LocationController().GetLocation((int)socialMediaMessageData[4]),
                (long) socialMediaMessageData[6], 
                new OffenceController().GetOffence((int)socialMediaMessageData[5]));
        }

        /// <summary>
        /// gets the specified SocialMediaMessage from the db
        /// </summary>
        /// <param name="ID">SocialMediaMessage id you need</param>
        /// <returns>the new SocialMediaMessage object requested</returns>
        public SocialMediaMessage GetSocialMediaMessage(long tweetID)
        {
            SqlCommand query = new SqlCommand("SELECT DateTime, Message, Username, Handle, LocationID, OffenceID, TwitterID " +
                "FROM SocialMediaMessage WHERE TwitterID = @TwitterID");
            query.Parameters.Add("@TwitterID", System.Data.SqlDbType.BigInt);
            query.Parameters["@TwitterID"].Value = tweetID;
            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);

            if (rows.Count > 0)
            {
                return ObjectArrayToSocialMediaMessage(rows[0]);
            }
            return null;
        }

        /// <summary>
        /// gets the specified SocialMediaMessages associated to the offence from the db
        /// </summary>
        /// <param name="ID">Offence id you need</param>
        /// <returns>related socialmediamessages in a list</returns>
        public List<SocialMediaMessage> GetOffenceSocialMediaMessages(Offence offence, int mediaType = 0)
        {
            SqlCommand query = new SqlCommand("SELECT DateTime, Message, Username, Handle, LocationID, OffenceID, TwitterID " +
                "FROM SocialMediaMessage WHERE OffenceID = @OffenceID AND MediaType = @MediaType");

            query.Parameters.Add("@OffenceID", System.Data.SqlDbType.Int);
            query.Parameters.Add("@MediaType", System.Data.SqlDbType.Int);

            query.Parameters["@OffenceID"].Value = offence.ID;
            query.Parameters["@MediaType"].Value = mediaType;
            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);
            List<SocialMediaMessage> socialMediaMessages = new List<SocialMediaMessage>();

            if (rows.Count > 0)
            {
                 rows.ForEach(smm => socialMediaMessages.Add(ObjectArrayToSocialMediaMessage(smm)));
            }
            return socialMediaMessages;
        }

        /// <summary>
        /// Checks if Message already existst in the database
        /// </summary>
        /// <returns>true is the message exists, false if message does not exist</returns>
        public bool MessageExists(String message)
        {
            SqlCommand query = new SqlCommand("SELECT TOP(1) OffenceID FROM SendMessage WHERE Message = @Message");
            query.Parameters.Add("@Message", System.Data.SqlDbType.NVarChar);
            query.Parameters["@Message"].Value = message;
            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);

            if (rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create a Search hashtag fot the offence
        /// </summary>
        /// <param name="offence">the offence for the search hastag</param>
        /// <returns></returns>
        public string Hashtag(Offence offence)
        {
            SqlCommand query = new SqlCommand("SELECT Hashtag FROM Offence WHERE ID = @OffenceID");
            query.Parameters.Add("@OffenceID", System.Data.SqlDbType.Int);
            query.Parameters["@OffenceID"].Value = offence.ID;
            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);

            if (rows.Count == 1 && rows[0].GetValue(0).ToString().Length > 0)
            {
                string text = rows[0].GetValue(0).ToString();
                offence.CallHashtag = text;
                return text;
            }

            else
            {
                return $"#Delict{offence.ID}";
            }
        }

        /// <summary>
        /// Change the hastag for the given offence in the database
        /// </summary>
        /// <param name="offence">the offence that needs to be changed</param>
        public void UpdateHashtag(Offence offence)
        {
            SqlCommand query = new SqlCommand("UPDATE Offence SET Hashtag = @Hashtag WHERE ID = @OffenceID");

            //prepare values in statement 
            query.Parameters.Add("@OffenceID", System.Data.SqlDbType.Int);
            query.Parameters.Add("@Hashtag", System.Data.SqlDbType.NVarChar);

            query.Parameters["@Hashtag"].Value = $"Delict{offence.ID}";
            query.Parameters["@OffenceID"].Value = offence.ID;

            _dbContext.ExecuteQuery(query);
        }
    }
}
