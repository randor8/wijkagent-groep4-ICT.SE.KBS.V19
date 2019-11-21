using System;
using System.Collections.Generic;
using System.Text;

namespace WijkagentModels
{
    public class SocialMediaMessage
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public virtual Location LocationID { get; set; }

        public SocialMediaMessage(int id, DateTime time, string message, Location location)
        {
            ID = id;
            Time = time;
            Message = message;
            LocationID = location;
        }

        public override string ToString()
        {
            return $"Id: {ID}\nDate: {Time}\nContent: {Message}\nLocation: {LocationID.Latitude} : {LocationID.Longitude}";
        }
    }
}
