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
        public int Type { get; set; }

        public SocialMediaMessage(DateTime dateTime, string message, string user, string handle, Location location, Offence offence = null, int type = 0, int id = 0)
        {
            ID = id;
            DateTime = dateTime;
            Message = message;
            User = user;
            Handle = handle;
            Location = location;
            Type = type;
            Offence = offence;
        }

        public override string ToString()
        {
            return $"\nGebruiker: {User}\nTwitter Naam: {Handle}\nomschrijving: {Message}\ndatum en tijd: {DateTime.ToShortDateString()} ";
        }
    }
}
