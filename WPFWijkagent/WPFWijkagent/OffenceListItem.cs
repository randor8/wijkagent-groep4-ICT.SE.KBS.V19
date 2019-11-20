using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// class for converting offences to the needed list items
    /// </summary>
    public class OffenceListItem
    {
        /// <summary>
        /// inits the offence list item so it can be used to display in a list
        /// </summary>
        /// <param name="iD"> the offence db item id</param>
        /// <param name="dateTime"> the offence db item date and time</param>
        /// <param name="description">the offence db item description</param>
        public OffenceListItem(int iD, DateTime dateTime, string description)
        {
            ID = iD;
            DateTime = dateTime;
            Description = description;
        }

        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// creates a string representation of the object
        /// </summary>
        /// <returns> the string representation of the object</returns>
        public override string ToString()
        {
            return $"{Description}, {DateTime}";
        }
    }
}
