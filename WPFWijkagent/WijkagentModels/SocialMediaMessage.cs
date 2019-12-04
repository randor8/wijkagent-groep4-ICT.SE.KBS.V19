using System;

namespace WijkagentModels
{
    public class SocialMediaMessage
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public virtual Location LocationID { get; set; }
        public string Name { get; set; }
        public string Handle { get; set; }


        public SocialMediaMessage(int id, string user, string handle, DateTime time, string message, Location location)
        {
            ID = id;
            Name = user;
            Handle = $"@{handle}";
            DateTime = time;
            Message = message;
            LocationID = location;
        }

        public override string ToString()
        {
            return $"User: {Name}\nHandle: {Handle}\nContent: {Message}\nDate: {DateTime}";
        }
    }
}
