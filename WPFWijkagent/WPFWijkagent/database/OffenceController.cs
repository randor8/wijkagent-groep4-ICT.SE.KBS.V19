using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WijkagentModels;

namespace WijkagentWPF.database
{
    public class OffenceController
    {
        private DBContext _dbContext { get; set; }

        public OffenceController()
        {
            _dbContext = new DBContext();
        }

        /// <summary>
        /// saves an offence and its location when location.id == 0
        /// </summary>
        /// <param name="offence">offence obj that needs saving</param>
        /// <returns> returns the ID of the inserted Offence</returns>
        public int SetOffence(Offence offence)
        {
            if (offence.Location.ID == 0)
            {
                offence.Location.ID = new LocationController().SetLocation(offence.Location);
            }
            SqlCommand query = new SqlCommand("INSERT INTO Offence (DateTime, Description, LocationID, Category) " +
                "OUTPUT INSERTED.ID " +
                "VALUES(@DateTime, @Description, @LocationID, @Category)");

            //set values we want to insert
            query.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime);
            query.Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@LocationID", System.Data.SqlDbType.Int);
            query.Parameters.Add("@Category", System.Data.SqlDbType.VarChar);

            query.Parameters["@DateTime"].Value = offence.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            query.Parameters["@Description"].Value = offence.Description;
            query.Parameters["@LocationID"].Value = offence.Location.ID;
            query.Parameters["@Category"].Value = offence.Category.ToString();

            return _dbContext.ExecuteInsertQuery(query);
        }

        /// <summary>
        /// converts a database row to an offence object
        /// </summary>
        /// <param name="offenceData">array of offence data fields (5 expected)</param>
        /// <returns>offence object</returns>
        private Offence ObjectArrayToOffenc(object[] offenceData)
        {
            //split the datetime fields from the string
            string[] dateTimeStrings = offenceData[1].ToString().Split(DBContext.DateTimeSeparator);
            //parse the datetime fields
            int.TryParse(dateTimeStrings[0], out int dd);
            int.TryParse(dateTimeStrings[1], out int mm);
            int.TryParse(dateTimeStrings[2], out int yyyy);
            int.TryParse(dateTimeStrings[3], out int hh);
            int.TryParse(dateTimeStrings[4], out int min);
            int.TryParse(dateTimeStrings[5], out int ss);

            OffenceCategories category;
            //cast to enum value
            try
            {
                category = (OffenceCategories)Enum.Parse(typeof(OffenceCategories), offenceData[4].ToString());
            }
            catch (ArgumentException)
            {
                category = OffenceCategories.Null;
            }

            return new Offence(
                new DateTime(yyyy, mm, dd, hh, min, ss),
                offenceData[2].ToString(),
                new LocationController().GetLocation((int)offenceData[3]),
                category,
                (int)offenceData[0]);
        }

        /// <summary>
        /// Gets the databse offence that belongs to the id
        /// </summary>
        /// <param name="ID">offence ID</param>
        /// <returns>the requested offence as an object</returns>
        public Offence GetOffence(int ID)
        {
            SqlCommand query = new SqlCommand("SELECT ID, DateTime, Description, LocationID, Category FROM Offence WHERE ID = @ID");
            query.Parameters.Add("@ID", System.Data.SqlDbType.Int);
            query.Parameters["@ID"].Value = ID;
            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);

            if (rows.Count == 1)
            {
                return ObjectArrayToOffenc(rows[0]);
            }
            return null;
        }

        /// <summary>
        /// gets all the db offences
        /// </summary>
        /// <returns>obj list filled with the offences objects</returns>
        public List<Offence> GetOffences()
        {
            SqlCommand query = new SqlCommand("SELECT ID, DateTime, Description, LocationID, Category FROM Offence");
            List<Offence> offences = new List<Offence>();

            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);

            //add all the offences
            if (rows.Count > 0)
            {
                rows.ForEach(offenceData => offences.Add(ObjectArrayToOffenc(offenceData)));
            }

            return offences;
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
