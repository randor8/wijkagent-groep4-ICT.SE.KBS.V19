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
            SetOffenceData("", OffenceCategories.Cybercrime, new DateTime().ToLocalTime(), location);
        }

        /// <summary>
        /// sets the form fields for the offence 
        /// </summary>
        /// <param name="description">offence description</param>
        /// <param name="category">offence Categorie(enum value)</param>
        /// <param name="dateTime">offence date and time</param>
        public void SetOffenceData(string description, OffenceCategories category, DateTime dateTime)
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
        public void SetOffenceData(string description, OffenceCategories category, DateTime dateTime, Location location)
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
        /// Get all offences from a specific category
        /// </summary>
        /// <param name="categoryFilter"></param>
        /// <param name="offences"></param>
        /// <returns></returns>
        public List<OffenceListItem> GetOffenceDataByCategory(string categoryFilter, List<Offence> offences)
        {
            List<OffenceListItem> offenceListItems = new List<OffenceListItem>();
            List<OffenceListItem> convertedOffences = ConvertListOffenceToOffenceListItem(offences);
            if (categoryFilter == "Alles tonen")
            {
                offenceListItems = convertedOffences;
            }
            else
            {
                foreach (OffenceListItem OffenceListItem in convertedOffences)
                {
                    if (OffenceListItem.Offence.Category.ToString() == categoryFilter)
                    {
                        offenceListItems.Add(new OffenceListItem(OffenceListItem.Offence));
                    }
                }
            }

            return offenceListItems;
        }

        /// <summary>
        /// Converts an Offence list into an OffenceListItem list
        /// </summary>
        /// <param name="offence"></param>
        /// <returns></returns>
        public List<OffenceListItem> ConvertListOffenceToOffenceListItem(List<Offence> offence)
        {
            List<OffenceListItem> offenceListItems = new List<OffenceListItem>();
            foreach (Offence offenceItem in offence)
            {
                offenceListItems.Add(new OffenceListItem(offenceItem));
            }

            return offenceListItems;
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
