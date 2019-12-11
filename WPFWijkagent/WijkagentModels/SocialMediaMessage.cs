using System;

namespace WijkagentModels
{
    public class SocialMediaMessage
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public string User { get; set; }
        public string Handle { get; set; }
        public virtual Location LocationID { get; set; }
        public Offence OffenceID { get; set; }

        public SocialMediaMessage(int id, DateTime dateTime, string message, string user, string handle, Location location, Offence offence = null)
        {
            ID = id;
            DateTime = dateTime;
            Message = message;
            User = user;
            Handle = handle;
            LocationID = location;
            OffenceID = offence;
        }

        public override string ToString()
        {
            return $"\ngebruiker naam: {User}\ntwitter naam: {Handle}\nomschrijving: {Message}\ndatum en tijd: {DateTime.ToShortDateString()} ";
        }
    }
}