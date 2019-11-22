using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;
using System.Data.SqlClient;

namespace WijkagentWPF
{
    public class DBContext<T> where T: IModels
    {
        private string GetConnectionString()
        {
            //TODO: replace this with secure way of initialization
            //OPT: maybe just a file with the data source, user, password
            //OPT: app.CONFIG with conn string details
            //...: something about property configuration
            return "Persist Security Info=True;Data Source=127.0.0.1; Password =0ntplofde_Cav1a!; User ID = sa";

        }

        public DBContext()
        {
            string connectionString = GetConnectionString();

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            Console.WriteLine("State: {0}", connection.State);
            Console.WriteLine("ConnectionString: {0}", connection.ConnectionString);
            connection.Dispose();
        }

        public void SetItem(T item)
        {
            item.ID ++;
        }

        public T GetItem()
        {
            throw new Exception("no items jet...");
        }
    }
}
