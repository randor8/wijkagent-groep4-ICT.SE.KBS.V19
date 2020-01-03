using System;
using System.Collections.Generic;
using System.Text;

namespace WijkagentModels
{
    public class DirectMessage
    {
        public long _senderID { get; set; }
        public string _content { get; set; }
        public DateTime _createdAt { get; set; }

        public DirectMessage(long id, string text, DateTime time)
        {
            _senderID = id;
            _content = text;
            _createdAt = time;
        }
    }
}
