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
                "INSERT INTO SocialMediaMessage (DateTime, Message, Username, Handle, locationID, MediaType, OffenceID) " +
                "OUTPUT INSERTED.ID " +
                "VALUES(@DateTime, @message, @user, @handle, @LocationID, @MediaType, @OffenceID)");

            //prepare values in statement 
            query.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime);
            query.Parameters.Add("@message", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@user", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@handle", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@LocationID", System.Data.SqlDbType.Int);
            query.Parameters.Add("@OffenceID", System.Data.SqlDbType.Int);
            query.Parameters.Add("@MediaType", System.Data.SqlDbType.Int);

            query.Parameters["@DateTime"].Value = socialMediaMessage.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            query.Parameters["@message"].Value = socialMediaMessage.Message;
            query.Parameters["@user"].Value = socialMediaMessage.User;
            query.Parameters["@handle"].Value = socialMediaMessage.Handle;
            query.Parameters["@LocationID"].Value = socialMediaMessage.Location.ID;
            query.Parameters["@OffenceID"].Value = socialMediaMessage.Offence.ID;
            query.Parameters["@MediaType"].Value = socialMediaMessage.Type;

            return _dbContext.ExecuteInsertQuery(query);
        }

        /// <summary>
        /// converts database row to an actual SocialMediaMessage object
        /// </summary>
        /// <param name="socialMediaMessageData">raw data from the database(7 fields)</param>
        /// <returns>the SocialMediaMessage object</returns>
        private SocialMediaMessage ObjectArrayToSocialMediaMessage(object[] socialMediaMessageData)
        {
            //get datetime fields
            string[] dateTimeStrings = socialMediaMessageData[1].ToString().Split(DBContext.DateTimeSeparator);

            //parse datetime fields
            int.TryParse(dateTimeStrings[0], out int dd);
            int.TryParse(dateTimeStrings[1], out int mm);
            int.TryParse(dateTimeStrings[2], out int yyyy);
            int.TryParse(dateTimeStrings[3], out int hh);
            int.TryParse(dateTimeStrings[4], out int min);
            int.TryParse(dateTimeStrings[5], out int ss);

            return new SocialMediaMessage(
                new DateTime(yyyy, mm, dd, hh, min, ss),
                socialMediaMessageData[2].ToString(),
                socialMediaMessageData[3].ToString(),
                socialMediaMessageData[4].ToString(),
                new LocationController().GetLocation((int)socialMediaMessageData[5]),
                new OffenceController().GetOffence((int)socialMediaMessageData[6]),
                (int)socialMediaMessageData[0]);
        }

        /// <summary>
        /// gets the specified SocialMediaMessage from the db
        /// </summary>
        /// <param name="ID">SocialMediaMessage id you need</param>
        /// <returns>the new SocialMediaMessage object requested</returns>
        public SocialMediaMessage GetSocialMediaMessage(int ID)
        {
            SqlCommand query = new SqlCommand("SELECT ID, DateTime, Message, Username, Handle, LocationID, OffenceID " +
                "FROM SocialMediaMessage WHERE ID = @ID");
            query.Parameters.Add("@ID", System.Data.SqlDbType.Int);
            query.Parameters["@ID"].Value = ID;
            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);

            if (rows.Count == 1)
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
        public List<SocialMediaMessage> GetOffenceSocialMediaMessages(int offenceID)
        {
            SqlCommand query = new SqlCommand("SELECT ID, DateTime, Message, Username, Handle, LocationID, OffenceID " +
                "FROM SocialMediaMessage WHERE OffenceID = @OffenceID");
            query.Parameters.Add("@OffenceID", System.Data.SqlDbType.Int);
            query.Parameters["@OffenceID"].Value = offenceID;
            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);
            List<SocialMediaMessage> socialMediaMessages = new List<SocialMediaMessage>();

            if (rows.Count > 0)
            {
                rows.ForEach(smm => socialMediaMessages.Add(ObjectArrayToSocialMediaMessage(smm)));
            }
            return socialMediaMessages;
        }
    }
}
