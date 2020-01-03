using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    class Observer
    {
        public List<DirectMessage> _messages = new List<DirectMessage>();
        public long _Id { get; set; }

        public List<DirectMessage> UpdateConversation()
        {
            List<DirectMessage> messages = new List<DirectMessage>();
            var results = Scraper.GetLatestMessages()
                .Where(x => x.SenderId == _Id || x.RecipientId == _Id)
                .Select(x => new { x.SenderId, x.Text, x.CreatedAt });
            foreach (var item in results)
            {
                messages.Add(new DirectMessage(item.SenderId, item.Text, item.CreatedAt));
            }
            return messages;
        }

        public void CompareConversation(List<DirectMessage> UpdatedConversation )
        {
            int currentCount = _messages.Count;
            int newCount = UpdatedConversation.Count;
            if(newCount > currentCount && UpdatedConversation[newCount-1]._content != _messages[currentCount-1]._content)
            {
                _messages = UpdatedConversation;
            }
        }

    }
}
