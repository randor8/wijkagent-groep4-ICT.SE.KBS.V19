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
        public Offence OffenceID { get; set; }

        public SocialMediaMessage(int id, DateTime time, string message, Location location, Offence offence = null)
        {
            ID = id;
            DateTime = time;
            Message = message;
            LocationID = location;
            OffenceID = offence;
        }

        public override string ToString()
        {
            return $"Id: {ID}\nDate: {DateTime}\nContent: {Message}\nLocation: {LocationID.Latitude} : {LocationID.Longitude}";
        }
    }
}
