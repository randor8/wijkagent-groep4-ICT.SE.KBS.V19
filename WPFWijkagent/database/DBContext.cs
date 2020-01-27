using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace WijkagentWPF
{
    public class DBContext
    {
        /// <summary>
        /// seperator to use when spliting datetime fields from db
        /// </summary>
        public static char[] DateTimeSeparator = new char[] { ' ', '-', ':' };

        /// <summary>
        /// database errors wil be stored here
        /// </summary>
        public string DBStatus { get; private set; }


        /// <summary>
        /// gets the connection string present in the app.config.
        /// </summary>
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Wijkagent"].ConnectionString;

        /// <summary>
        /// connection to the database
        /// </summary>
        private SqlConnection _connection { get; set; }

        /// <summary>
        /// instantiates a new connetion to the db
        /// </summary>
        public DBContext()
        {
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// executes select statement
        /// </summary>
        /// <param name="SQLStatement">statement to execute</param>
        /// <returns>a list of database rows</returns>
        public List<object[]> ExecuteSelectQuery(SqlCommand SQLStatement)
        {
            try
            {
                _connection.Open();
                SQLStatement.Connection = _connection;
                SQLStatement.CommandTimeout = 4;
                SqlDataReader reader = SQLStatement.ExecuteReader();
                List<object[]> rows = new List<object[]>();

                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    rows.Add(row);
                }
                return rows;
            }
            catch (SqlException sqlEX)
            {
                //save the error and give empty result set
                DBStatus = sqlEX.Message;
                Logger.Log.ErrorEventHandler(this);
                return new List<Object[]>();
            }
            finally
            {
                // always close the database
                SQLStatement.Dispose();
                _connection.Close();
            }
        }

        /// <summary>
        /// executes a query and returns the inserted last column(id)
        /// </summary>
        /// <param name="SQLStatement">the statement that needs to executed</param>
        /// <returns>id of the inserted row</returns>
        public int ExecuteInsertQuery(SqlCommand SQLStatement)
        {
            try
            {
                _connection.Open();
                SQLStatement.Connection = _connection;
                SQLStatement.CommandTimeout = 4;
                return (int)SQLStatement.ExecuteScalar();

            }

            catch (SqlException sqlEX)
            {
                //save the error and give useles result
                DBStatus = sqlEX.Message;
                Logger.Log.ErrorEventHandler(this);
                return 0;
            }
            finally
            {
                // always close the database
                SQLStatement.Dispose();
                _connection.Close();
            }
        }

        /// <summary>
        /// executes a query and returns the modified rows count
        /// </summary>
        /// <param name="SQLStatement">the statement that needs to executed</param>
        /// <returns>number of rows altered</returns>
        public int ExecuteQuery(SqlCommand SQLStatement)
        {
            try
            {
                _connection.Open();
                SQLStatement.Connection = _connection;
                SQLStatement.CommandTimeout = 4;

                return SQLStatement.ExecuteNonQuery();
            }
            catch (SqlException sqlEX)
            {
                //save the error and give useles result
                DBStatus = sqlEX.Message;
                Logger.Log.ErrorEventHandler(this);
                return 0;
            }
            finally
            {
                SQLStatement.Dispose();
                _connection.Close();
            }
        }
    }
}
