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
            var reader = command.ExecuteReader();

            _connection.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public void GetItem(int ID, ref T item)
        {
            _connection.Open();

            var command = new SqlCommand("SELECT * FROM Offence WHERE ID = :ID", _connection);

            //add bind params
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                foreach (object col in row)
                {
                    DataToObject(ref item, reader);
                    //item.GetType().GetProperty(reader.GetFieldType(0).Name).SetValue(item, reader.GetValue(0));
                }
            }
            _connection.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="reader"></param>
        private void DataToObject(ref T item, SqlDataReader reader)
        {
            object[] row = new object[reader.FieldCount];
            reader.GetValues(row);
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
                        var relObj = GetItem(foreignKeyId, new propertyType());
                        item.GetType().GetProperty(propertyName).SetValue(item,  propertyType);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<T> GetItems(T obj)
        {
            _connection.Open();

            List<T> results = new List<T>();
            var command = new SqlCommand("SELECT * FROM Offence WHERE ID = :ID", _connection);
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DataToObject(ref obj, reader);
                    results.Add(obj);
                }
            }
            _connection.Dispose();
            return results;
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
