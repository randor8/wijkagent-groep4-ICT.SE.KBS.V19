using System;

namespace WijkagentModels
{
    public class Offence
    {
        public int ID { get; set; }

        public DateTime DateTime { get; set; }

        public string Description { get; set; }

        public virtual Location LocationID { get; set; }

        public OffenceCategories Category { get; set; }

        public Offence(DateTime dateTime, Location Location)
        {
            DateTime = dateTime;
            LocationID = Location;
        }

        /// <summary>
        /// creates a string representation of the object
        /// </summary>
        /// <returns> the string representation of the object</returns>
        public override string ToString() => $"{Description}, {DateTime}";

        /// <summary>
        /// Compares given object with this object.
        /// </summary>
        /// <param name="obj">Object to compare this object with.</param>
        /// <returns>True if given object and this object are the same, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Offence)
            {
                Offence that = (Offence)obj;
                if (this.ID == that.ID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
