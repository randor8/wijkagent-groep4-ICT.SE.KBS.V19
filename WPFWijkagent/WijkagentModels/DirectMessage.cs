using System;
using System.Collections.Generic;
using System.Text;

namespace WijkagentModels
{
    public class DirectMessage
    {
        public long _senderID { get; set; }
        public long _messageID { get; set; }
        public string _content { get; set; }
        public DateTime _createdAt { get; set; }

        public DirectMessage(long id, long messageId, string text, DateTime time)
        {
            _senderID = id;
            _messageID = messageId;
            _content = text;
            _createdAt = time;
        }
    }
}
