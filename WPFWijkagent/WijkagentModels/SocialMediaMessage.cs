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
        public virtual Location Location { get; set; }
        public Offence Offence { get; set; }
        public int MediaType { get; set; }
        public long TwitterID { get; set; }

        public SocialMediaMessage(DateTime dateTime, string message, string user, string handle, Location location, long twitterID, Offence offence = null, int Mediatype = 0, int id = 0)
        {
            ID = id;
            DateTime = dateTime;
            Message = message;
            User = user;
            Handle = handle;
            Location = location;
            MediaType = Mediatype;
            Offence = offence;
            TwitterID = twitterID;
        }

        public override string ToString()
        {
            return $"\ngebruiker naam: {User}\ntwitter naam: {Handle}\nomschrijving: {Message}\ndatum en tijd: {DateTime.ToString("dd/MM/yyyy H:mm")} ";
        }
    }
}