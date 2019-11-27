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
    public class DBContext<T> where T: IModels
    {
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
        private static ForwardedPort _port { get; set; }

        private static SqlConnection _connection { get; set; }
        
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
            _port = new ForwardedPortLocal("127.0.0.1", 1433, "localhost", 1433);

            //try to get a ssh connecten and add the portforward
            try
            {
                _client.Connect();
                _client.AddForwardedPort(_port);
            
                _port.Exception += delegate (object sender, ExceptionEventArgs e)
                {
                    DBStatus = "no portforward can be made on the server!";
                    DBContext<IModels>.CloseSshConnection();
                };
            
                _port.Start();

            } catch(SshConnectionException sshEx)
            {
                DBStatus = "no ssh connection can be made to the ubuntu server please inform the system admin.";
                DBContext<IModels>.CloseSshConnection();
            } catch(ObjectDisposedException objEx)
            {
                DBStatus = "the ssh connection to the ubuntu server has been disposed and cannot be used";
                DBContext<IModels>.CloseSshConnection();
            }
        }

        /// <summary>
        /// gets the connection string present in the app.config.
        /// </summary>
        /// <returns>app.config connection string</returns>
        private string GetConnectionString() => ConfigurationManager.ConnectionStrings["Wijkagent"].ConnectionString;

        public DBContext()
        {
            string connectionString = GetConnectionString();
            if(DBContext<IModels>._client != null && DBContext<IModels>._client.IsConnected)
            {
                DBContext<IModels>.GetSshConnection();
            }
            _connection = new SqlConnection(connectionString);
            //REMOVE:
            _connection.Open();

            var command = new SqlCommand("ALTER TABLE Offence ADD FOREIGN KEY (LocationID) REFERENCES Location(ID)", _connection);
            var command2 = new SqlCommand("ALTER TABLE SocialMediaMessage ADD FOREIGN KEY (LocationID) REFERENCES Location(ID)", _connection);
            var command3 = new SqlCommand("ALTER TABLE SocialMediaMessage ADD FOREIGN KEY (OffenceID) REFERENCES Offence(ID)", _connection);
            var res = command.ExecuteNonQuery();

            _connection.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(T item)
        {
            item.ID++;
            _connection.Open();
            
            var command = new SqlCommand("INSERT INTO Offence VALUES(:val1, ..)", _connection);
            var res = command.ExecuteReader();
            
            _connection.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public object[] GetItem(int ID)
        {
            _connection.Open();

            var command = new SqlCommand("SELECT * FROM Offence WHERE ID = :ID", _connection);
            command.CreateParameter();
            var res = command.ExecuteReader();
            object[] row = new object[res.FieldCount];
            if (res.HasRows)
            {
                res.GetValues(row);
            }
            _connection.Dispose();
            return row;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object[] GetItems()
        {
            _connection.Open();

            var command = new SqlCommand("SELECT * FROM Offence WHERE ID = :ID", _connection);
            var res = command.ExecuteReader();
            object[] row = new object[res.FieldCount];
            if (res.HasRows)
            {
                res.GetValues(row);
            }
            _connection.Dispose();
            return row;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CloseSshConnection()
        {
            _port.Stop();
            _client.Dispose();
        }
    }
}
