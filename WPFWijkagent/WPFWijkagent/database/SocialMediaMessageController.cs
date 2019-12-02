using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF.database
{
    class SocialMediaMessageController
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
        /// <param name="location">location for this message</param>
        /// <param name="offenceID">associated id</param>
        /// <returns>returns the inserted id that has been added</returns>
        public int SetSocialMediaMessage(DateTime dateTime, string message, Location location, int offenceID)
        {
            int locationID = new LocationController().SetLocation(location.Latitude, location.Longitude);
            return SetSocialMediaMessage(dateTime, message, locationID, offenceID);
        }

        /// <summary>
        /// inserts the socialmedia message in the db
        /// </summary>
        /// <param name="dateTime">datetime for the media message</param>
        /// <param name="message">media 'message' part</param>
        /// <param name="locationID">locationID for this message</param>
        /// <param name="offenceID">associated id</param>
        /// <returns>returns the inserted id that has been added</returns>
        public int SetSocialMediaMessage(DateTime dateTime, string message, int locationID, int offenceID)
        {
            SqlCommand query = new SqlCommand("INSERT INTO SocialMediaMessage VALUES(:DateTime, :message, :LocationID, :OffenceID) output INSERTED.ID VALUES(:DateTime, :message, :LocationID, :OffenceID)");

            query.Parameters.Add(":DateTime", System.Data.SqlDbType.DateTime);
            query.Parameters.Add(":message", System.Data.SqlDbType.VarChar);
            query.Parameters.Add(":LocationID", System.Data.SqlDbType.Int);
            query.Parameters.Add(":OffenceID", System.Data.SqlDbType.Int);
            query.Parameters[":DateTime"].Value = dateTime;
            query.Parameters[":message"].Value = message;
            query.Parameters[":LocationID"].Value = locationID;
            query.Parameters[":OffenceID"].Value = offenceID;

            return _dbContext.ExecuteInsertQuery(query);
        }

        /// <summary>
        /// converts database row to an actual SocialMediaMessage object
        /// </summary>
        /// <param name="socialMediaMessageData">raw data from the database(4 fields)</param>
        /// <returns>the SocialMediaMessage object</returns>
        private SocialMediaMessage ObjectArrayToSocialMediaMessage(object[] socialMediaMessageData)
        {
            string[] dateTimeStrings = socialMediaMessageData[1].ToString().Split(DBContext.DateTimeSeparator);
            int dd, mm, yyyy, ss, min, hh;
            int.TryParse(dateTimeStrings[0], out dd);
            int.TryParse(dateTimeStrings[1], out mm);
            int.TryParse(dateTimeStrings[2], out yyyy);
            int.TryParse(dateTimeStrings[3], out ss);
            int.TryParse(dateTimeStrings[4], out min);
            int.TryParse(dateTimeStrings[5], out hh);
            return new SocialMediaMessage(
                (int)socialMediaMessageData[0],
                new DateTime(yyyy,mm,dd,hh,min,ss),
                socialMediaMessageData[2].ToString(),
                new LocationController().GetLocation((int)socialMediaMessageData[3]),
                new Offence() {
                    ID = (int)socialMediaMessageData[4] 
                });
        }

        /// <summary>
        /// gets the specified SocialMediaMessage from the db
        /// </summary>
        /// <param name="ID">SocialMediaMessage id you need</param>
        /// <returns>the new SocialMediaMessage object requested</returns>
        public SocialMediaMessage GetLocation(int ID)
        {
            SqlCommand query = new SqlCommand("SELECT * FROM SocialMediaMessage WHERE ID = :ID");
            query.Parameters.Add(":ID", System.Data.SqlDbType.Int);
            query.Parameters[":ID"].Value = ID;
            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);


            if (rows.Count == 1)
            {
                return ObjectArrayToSocialMediaMessage(rows[0]);
            }
            return null;
        }
    }
}
