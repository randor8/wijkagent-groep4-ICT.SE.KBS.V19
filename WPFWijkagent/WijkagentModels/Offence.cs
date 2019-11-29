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

        /// <summary>
        /// creates a string representation of the object
        /// </summary>
        /// <returns> the string representation of the object</returns>
        public override string ToString() => $"{Description}, {DateTime}";
    }
}
