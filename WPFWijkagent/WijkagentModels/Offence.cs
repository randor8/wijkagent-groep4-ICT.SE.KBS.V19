using System;
using System.Collections.Generic;

namespace WijkagentModels
{
    public class Offence
    {
        public static List<Offence> OffenceData { get; set; } = new List<Offence>();

        public int ID { get; set; }

        public DateTime DateTime { get; set; }

        public string Description { get; set; }

        public virtual Location LocationID { get; set; }

        public OffenceCategories Category { get; set; }

        public Offence(int id, DateTime dateTime, string description, Location locationID, OffenceCategories offenceCategories)
        {
            ID = id;
            DateTime = dateTime;
            Description = description;
            LocationID = locationID;
            Category = offenceCategories;
        }
    }
}
