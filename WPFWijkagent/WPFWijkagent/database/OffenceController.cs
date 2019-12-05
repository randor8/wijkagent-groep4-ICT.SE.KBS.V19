﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
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
        /// saves an offence and its associated location
        /// </summary>
        /// <param name="dateTime">offence datetime </param>
        /// <param name="description">offence description</param>
        /// <param name="location">location object associated</param>
        /// <param name="category">offence categorie (enum)</param>
        /// <returns> returns the ID of the inserted Offence</returns>
        public int SetOffence(DateTime dateTime, string description, Location location, OffenceCategories category)
        {

            int locationID = new LocationController().SetLocation(location.Latitude, location.Longitude);
            return SetOffence(dateTime, description, locationID, category);
        }

        /// <summary>
        /// saves an offence 
        /// </summary>
        /// <param name="dateTime">offence datetime </param>
        /// <param name="description">offence description</param>
        /// <param name="location">location object associated</param>
        /// <param name="category">offence categorie (enum)</param>
        /// <returns> returns the ID of the inserted Offence</returns>
        public int SetOffence(DateTime dateTime, string description, int locationID, OffenceCategories category)
        {
            SqlCommand query = new SqlCommand("INSERT INTO Offence VALUES(@DateTime, @Description, @LocationID, @Category) output INSERTED.ID VALUES(@DateTime, @Description, @LocationID, @Category)");
            
            //set values we want to insert
            query.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime);
            query.Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
            query.Parameters.Add("@LocationID", System.Data.SqlDbType.Int);
            query.Parameters.Add("@Category", System.Data.SqlDbType.VarChar);

            query.Parameters["@DateTime"].Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            query.Parameters["@Description"].Value = description;
            query.Parameters["@LocationID"].Value = locationID;
            query.Parameters["@Category"].Value = category.ToString();

            return _dbContext.ExecuteInsertQuery(query);
        }

        /// <summary>
        /// converts a database row to an offence object
        /// </summary>
        /// <param name="offenceData">array of offence data fields (5 expected)</param>
        /// <returns>offence object</returns>
        private Offence ObjectArrayToOffenc(object[] offenceData)
        {
            string[] dateTimeStrings = offenceData[1].ToString().Split(DBContext.DateTimeSeparator);
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

            return new Offence()
            {
                ID = (int)offenceData[0],
                DateTime = new DateTime(yyyy, mm, dd, hh, min, ss),
                Description = offenceData[2].ToString(),
                LocationID = new LocationController().GetLocation((int)offenceData[3]),
                Category = category
            };
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

    }
}