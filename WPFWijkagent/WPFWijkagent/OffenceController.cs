using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// controls the offence(s) for the wpf application
    /// </summary>
    public class OffenceController
    {
        public Offence Offence { get; private set; }

        public OffenceController()
        {
            Offence = new Offence();
        }

        /// <summary>
        /// sets the offence location
        /// </summary>
        /// <param name="location">the location object to add</param>
        public void SetOffenceData(Location location)
        {
            SetOffenceData("", "", new DateTime().ToLocalTime(), location);
        }

        /// <summary>
        /// sets the form fields for the offence 
        /// </summary>
        /// <param name="description">offence description</param>
        /// <param name="category">offence Categorie(enum value)</param>
        /// <param name="dateTime">offence date and time</param>
        public void SetOffenceData(string description, string category, DateTime dateTime)
        {
            SetOffenceData(description, category, dateTime, Offence.LocationID);

        }

        /// <summary>
        /// sets all the offence fields
        /// </summary>
        /// <param name="description">offence description</param>
        /// <param name="category">offence Categorie(enum value)</param>
        /// <param name="dateTime">offence date and time</param>
        /// <param name="location">offence location</param>
        public void SetOffenceData(string description, string category, DateTime dateTime, Location location)
        {
            Offence NewOffence = new Offence();
            NewOffence.Category = category;
            NewOffence.Description = description;
            NewOffence.DateTime = dateTime;
            NewOffence.LocationID = location;
            //TODO: save the location as a separate object
            Offence.OffenceData.Add(NewOffence);
        }

        /// <summary>
        /// gets all the offences from the db
        /// </summary>
        /// <returns></returns>
        public List<Offence> GetOffences()
        {
            return Offence.OffenceData;
        }
    }
}
