using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF.database
{
    public class SendMessageController
    {
        private DBContext _dbContext { get; set; }

        public SendMessageController()
        {
            _dbContext = new DBContext();
        }

        /// <summary>
        /// Insert the sended messages into the SendMessage Table
        /// </summary>
        /// <param name="offence">the offence needed for the offence ID</param>
        /// <param name="message">the message that has to be inserted</param>
        public void SetSendMessage(Offence offence, String message)
        {
            SqlCommand insertMessage = new SqlCommand("INSERT INTO SendMessage (OffenceID, Message) OUTPUT INSERTED.ID" +
                " VALUES (@OffenceID, @Message)");
            insertMessage.Parameters.Add("@OffenceID", System.Data.SqlDbType.Int);
            insertMessage.Parameters.Add("@Message", System.Data.SqlDbType.NVarChar);

            insertMessage.Parameters["@OffenceID"].Value = offence.ID;
            insertMessage.Parameters["@Message"].Value = message;

            _dbContext.ExecuteInsertQuery(insertMessage);
        }


        /// <summary>
        /// Checks if Message already existst in the database
        /// </summary>
        /// <returns>true is the message exists, false if message does not exist</returns>
        public bool MessageExists(String message)
        {
            SqlCommand query = new SqlCommand("SELECT TOP(1) OffenceID FROM SendMessage WHERE Message = @Message");
            query.Parameters.Add("@Message", System.Data.SqlDbType.NVarChar);
            query.Parameters["@Message"].Value = message;
            List<object[]> rows = _dbContext.ExecuteSelectQuery(query);

            if (rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
