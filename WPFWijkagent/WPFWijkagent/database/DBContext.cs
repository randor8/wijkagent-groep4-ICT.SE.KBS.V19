using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace WijkagentWPF
{
    public class DBContext
    {
        /// <summary>
        /// current status of the SSH connection.
        /// </summary>
        public static string SSHStatus { get; private set; }

        /// <summary>
        /// client that is connected to the ubuntu server
        /// </summary>
        private static SshClient _client { get; set; }

        /// <summary>
        /// port we need to keep open to connect to the server.
        /// </summary>
        private static readonly ForwardedPort _port = new ForwardedPortLocal("127.0.0.1", 1433, "localhost", 1433);

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
            //check if we need to start the ssh connection
            if (DBContext._client != null && DBContext._client.IsConnected)
            {
                DBContext.GetSshConnection();
            }
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// connects to ubuntu server and adds a portforward to the mssql server
        /// NOTE: has to be executed once before a connection can be made to the mssql server
        /// </summary>
        private static void GetSshConnection()
        {
            string host = ConfigurationManager.AppSettings.Get("server_ssh_host");
            string user = ConfigurationManager.AppSettings.Get("server_ssh_user");
            string password = ConfigurationManager.AppSettings.Get("server_ssh_password");

            _client = new SshClient(host, user, password);
            //try to get a ssh connecten and add the portforward
            try
            {
                _client.Connect();
                _client.AddForwardedPort(_port);

                _port.Exception += delegate (object sender, ExceptionEventArgs e)
                {
                    SSHStatus = "no portforward can be made on the server!";
                    DBContext.CloseSshConnection();
                };

                _port.Start();

            }
            catch (SshConnectionException)
            {
                SSHStatus = "no ssh connection can be made to the ubuntu server please inform the system admin.";
                DBContext.CloseSshConnection();
            }
            catch (ObjectDisposedException)
            {
                SSHStatus = "the ssh connection to the ubuntu server has been disposed and cannot be used";
                DBContext.CloseSshConnection();
            }
        }


        /// <summary>
        /// closes the the portforward and the associated ssh connection
        /// NOTE: no connection can be made to the db for the entire application if ssh is closed
        /// </summary>
        public static void CloseSshConnection()
        {
            _port.Stop();
            _client.Dispose();
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

                int id = (int)SQLStatement.ExecuteScalar();

                return id;
            }

            catch (SqlException sqlEX)
            {
                //save the error and give useles result
                DBStatus = sqlEX.Message;
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

                int rows = SQLStatement.ExecuteNonQuery();

                return rows;
            }
            catch (SqlException sqlEX)
            {
                //save the error and give useles result
                DBStatus = sqlEX.Message;
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
