using System;
using System.Collections.Generic;
using System.Text;

namespace WijkagentModels
{
    public class SocialMediaMessage
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public virtual Location LocationID { get; set; }
    }
}
