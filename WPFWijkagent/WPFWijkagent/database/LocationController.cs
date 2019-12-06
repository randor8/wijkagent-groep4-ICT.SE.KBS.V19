using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;
using System.Data.SqlClient;

namespace WijkagentWPF.database
{
    public class LocationController
    {

        private DBContext _dbContext { get; set; }

        public LocationController()
        {
            _dbContext = new DBContext();
        }

        /// <summary>
        /// saves the location in the database
        /// </summary>
        /// <param name="latitude">location latitude</param>
        /// <param name="longitude">location longitude</param>
        /// <returns>the id of the inserted location</returns>
        public int SetLocation(double latitude, double longitude)
        {
            SqlCommand query = new SqlCommand("INSERT INTO Location (Latitude, Longitude) OUTPUT INSERTED.ID VALUES(@Latitude, @Longitude)");

            query.Parameters.Add("@Latitude", System.Data.SqlDbType.Float);
            query.Parameters.Add("@Longitude", System.Data.SqlDbType.Float);
            query.Parameters["@Latitude"].Value = latitude;
            query.Parameters["@Longitude"].Value = longitude;
            
            return _dbContext.ExecuteInsertQuery(query);
        }

        /// <summary>
        /// gets the specified location from the db
        /// </summary>
        /// <param name="ID">location id you need</param>
        /// <returns>the location object requested</returns>
        public Location GetLocation(int ID)
        {
            SqlCommand query = new SqlCommand("SELECT ID, Latitude, Longitude FROM Location WHERE ID = @ID");
            query.Parameters.Add("@ID", System.Data.SqlDbType.Int);
            query.Parameters["@ID"].Value = ID;
            List<object[]>  rows = _dbContext.ExecuteSelectQuery(query);


            if (rows.Count == 1)
            {
                object[] row = rows[0];
                return new Location((double)row[1], (double)row[2])
                {
                    ID = ID,
                };
            }
            return null;
        }

    }
}
