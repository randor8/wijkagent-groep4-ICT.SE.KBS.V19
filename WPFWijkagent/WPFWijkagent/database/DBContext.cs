using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;
using System.Configuration;
using System.Data.SqlClient;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace WijkagentWPF
{
    public class DBContext
    {
        /// <summary>
        /// current status of the SSH connection.
        /// </summary>
        public static string SSHStatus { get; private set; }
        
        /// <summary>
        /// current status of the database.
        /// </summary>
        public static string DBStatus { get; private set; }

        /// <summary>
        /// client that is connected to the ubuntu server
        /// </summary>
        private static SshClient _client { get; set; }

        /// <summary>
        /// port we need to keep open to connect to the server.
        /// </summary>
        private static ForwardedPort _port = new ForwardedPortLocal("127.0.0.1", 1433, "localhost", 1433);

        /// <summary>
        /// gets the connection string present in the app.config.
        /// </summary>
        private string _connectionString = ConfigurationManager.ConnectionStrings["Wijkagent"].ConnectionString;

        /// <summary>
        /// 
        /// </summary>
        private SqlConnection _connection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DBContext()
        {
            if (DBContext<IModels>._client != null && DBContext<IModels>._client.IsConnected)
            {
                DBContext<IModels>.GetSshConnection();
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
                    DBContext<IModels>.CloseSshConnection();
                };
            
                _port.Start();

            } catch(SshConnectionException)
            {
                SSHStatus = "no ssh connection can be made to the ubuntu server please inform the system admin.";
                DBContext<IModels>.CloseSshConnection();
            } catch(ObjectDisposedException)
            {
                SSHStatus = "the ssh connection to the ubuntu server has been disposed and cannot be used";
                DBContext<IModels>.CloseSshConnection();
            }
        }


        /// <summary>
        /// closes the the portforward and the associated ssh connection
        /// NOTE: no connection can be made to the db for the entire application 
        /// </summary>
        public static void CloseSshConnection()
        {
            _port.Stop();
            _client.Dispose();
        }

        public List<object[]> ExecuteSelectQuery(SqlCommand SQLStatement)
        {
            _connection.Open();
            SqlDataReader reader = SQLStatement.ExecuteReader();
            List<object[]> rows = new List<object[]>();
            object[] row = new object[reader.FieldCount];

            while(reader.Read()){
                reader.GetValues(row);
            }

            _connection.Close();
            return rows;
        }
        
        public int ExecuteQuery(SqlCommand SQLStatement)
        {
            _connection.Open();
            SqlDataReader reader = SQLStatement.ExecuteReader();

            _connection.Close();
            return reader.RecordsAffected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="reader"></param>
        private void DBRowToObject(ref T item, SqlDataReader reader)
        {
            object[] row = new object[reader.FieldCount];
            reader.GetValues(row);
            //fill object with clumns from row 
            for(int i = 0; i < row.Length; i++)
            {
                Type propertyType = reader.GetFieldType(i);
                string propertyName = propertyType.Name;

                if (propertyType == "".GetType())
                {
                    var propertyValue = reader.GetFieldValue<string>(i);
                    item.GetType().GetProperty(propertyName).SetValue(item, propertyValue);
                } else if (propertyType == 0.GetType())
                {
                    var propertyValue = reader.GetFieldValue<int>(i);
                    item.GetType().GetProperty(propertyName).SetValue(item, propertyType);
                
                } else if (propertyType.IsClass)
                {    
                    if (propertyType == DateTime.Now.GetType())
                    {
                        var propertyValue = reader.GetFieldValue<DateTime>(i);
                        item.GetType().GetProperty(propertyName).SetValue(item, propertyType);
                    } else
                    {
                        //TODO: we have a related obj
                        var foreignKeyId= reader.GetFieldValue<int>(i);
                        IModels instance = (IModels)Activator.CreateInstance(propertyType);
                        var relObj = GetItem(foreignKeyId, ref instance);
                        item.GetType().GetProperty(propertyName).SetValue(item,  propertyType);
                    }
                }
            }
        }
    }
}
