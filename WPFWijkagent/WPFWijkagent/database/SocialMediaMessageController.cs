using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
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
        /// inserts the socialmedia message and location in the db
        /// </summary>
        /// <param name="dateTime">datetime for the media message</param>
        /// <param name="message">media 'message' part</param>
        /// <param name="user">media message username</param>
        /// <param name="handle">media message hande (@ name)</param>
        /// <param name="location">location for this message</param>
        /// <param name="offenceID">associated id</param>
        /// <returns>returns the inserted id that has been added</returns>
        public int SetSocialMediaMessage(DateTime dateTime, string message, string user, string handle, Location location, int offenceID)
        {
            int locationID = new LocationController().SetLocation(location.Latitude, location.Longitude);
            return SetSocialMediaMessage(dateTime, message, user, handle, locationID, offenceID);
        }

        /// <summary>
        /// inserts the socialmedia message in the db
        /// </summary>
        /// <param name="dateTime">datetime for the media message</param>
        /// <param name="message">media 'message' part</param>
        /// <param name="user">media message username</param>
        /// <param name="handle">media message hande (@ name)</param>
        /// <param name="locationID">locationID for this message</param>
        /// <param name="offenceID">associated id</param>
        /// <returns>returns the inserted id that has been added</returns>
        public int SetSocialMediaMessage(DateTime dateTime, string message, string user, string handle, int locationID, int offenceID)
        {
            SqlCommand query = new SqlCommand("INSERT INTO SocialMediaMessage (DateTime, Message, Username, Handle, locationID, OffenceID) OUTPUT INSERTED.ID VALUES(@DateTime, @message, @user, @handle, @LocationID, @OffenceID)");

            query.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime);
            query.Parameters.Add("@message", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@user", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@handle", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@LocationID", System.Data.SqlDbType.Int);
            query.Parameters.Add("@OffenceID", System.Data.SqlDbType.Int);
            query.Parameters["@DateTime"].Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            query.Parameters["@message"].Value = message;
            query.Parameters["@user"].Value = user;
            query.Parameters["@handle"].Value = handle;
            query.Parameters["@LocationID"].Value = locationID;
            query.Parameters["@OffenceID"].Value = offenceID;

            return _dbContext.ExecuteInsertQuery(query);
        }

        /// <summary>
        /// converts database row to an actual SocialMediaMessage object
        /// </summary>
        /// <param name="socialMediaMessageData">raw data from the database(7 fields)</param>
        /// <returns>the SocialMediaMessage object</returns>
        private SocialMediaMessage ObjectArrayToSocialMediaMessage(object[] socialMediaMessageData)
        {
            string[] dateTimeStrings = socialMediaMessageData[1].ToString().Split(DBContext.DateTimeSeparator);
            int.TryParse(dateTimeStrings[0], out int dd);
            int.TryParse(dateTimeStrings[1], out int mm);
            int.TryParse(dateTimeStrings[2], out int yyyy);
            int.TryParse(dateTimeStrings[3], out int hh);
            int.TryParse(dateTimeStrings[4], out int min);
            int.TryParse(dateTimeStrings[5], out int ss);

            return new SocialMediaMessage(
                (int)socialMediaMessageData[0],
                new DateTime(yyyy, mm, dd, hh, min, ss),
                socialMediaMessageData[2].ToString(),
                socialMediaMessageData[3].ToString(),
                socialMediaMessageData[4].ToString(),
                new LocationController().GetLocation((int)socialMediaMessageData[5]),
                new OffenceController().GetOffence((int)socialMediaMessageData[6]));
        }

        /// <summary>
        /// gets the specified SocialMediaMessage from the db
        /// </summary>
        /// <param name="ID">SocialMediaMessage id you need</param>
        /// <returns>the new SocialMediaMessage object requested</returns>
        public SocialMediaMessage GetSocialMediaMessage(int ID)
        {
            SqlCommand query = new SqlCommand("SELECT ID, DateTime, Message, Username, Handle, LocationID, OffenceID FROM SocialMediaMessage WHERE ID = @ID");
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
            SqlCommand query = new SqlCommand("SELECT ID, DateTime, Message, Username, Handle, LocationID, OffenceID FROM SocialMediaMessage WHERE OffenceID = @OffenceID");
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
