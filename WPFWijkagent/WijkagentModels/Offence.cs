using System;

namespace WijkagentModels
{
    public class Offence
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public virtual Location LocationID { get; set; }
    }
}
