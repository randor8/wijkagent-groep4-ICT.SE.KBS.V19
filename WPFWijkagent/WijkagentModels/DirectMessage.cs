using System;
using System.Collections.Generic;
using System.Text;

namespace WijkagentModels
{
    public class DirectMessage
    {
        public long SenderID { get; set; }
        public long MessageID { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public DirectMessage(long id, long messageId, string text, DateTime time)
        {
            SenderID = id;
            MessageID = messageId;
            Content = text;
            CreatedAt = time;
        }
    }
}
