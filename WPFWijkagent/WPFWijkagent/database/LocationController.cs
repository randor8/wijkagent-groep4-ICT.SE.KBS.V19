using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;
using System.Data.SqlClient;

namespace WijkagentWPF.database
{
    class LocationController
    {

        private DBContext _dbContext { get; set; }

        public LocationController()
        {
            _dbContext = new DBContext();
        }
        //INSERT INTO Location VALUES()
        public bool SetLocation()
        {
            SqlCommand query = new SqlCommand("");
            query.Parameters.Add(":ID", System.Data.SqlDbType.Int);
            query.Parameters[":ID"].Value = ID;
            return _dbContext.ExecuteQuery(query) == 0 ? false : true;
        }

        public Location GetLocation(int ID)
        {
            SqlCommand query = new SqlCommand("");
            query.Parameters.Add(":ID", System.Data.SqlDbType.Int);
            query.Parameters[":ID"].Value = ID;
            List<object[]>  rows = _dbContext.ExecuteSelectQuery(query);
            if (rows.Count == 1)
            {
                object[] row = rows[0];
                return new Location()
                {
                    ID = ID,
                    Latitude = (double)row[0],
                    Longitude = (double)row[1] 
                };
            }
        }

    }
}
