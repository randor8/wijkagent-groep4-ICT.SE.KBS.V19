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
            SetOffenceData("", OffenceCategories.categorie1, new DateTime().ToLocalTime(), location);
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
            Offence.Category = category;
            Offence.Description = description;
            Offence.DateTime = dateTime;
            //TODO: save the location as a separate object
            Offence.LocationID = location;
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

                    if (OffenceListItem.Category.ToString() == categoryFilter)
                    {
                        offenceListItems.Add(new OffenceListItem(OffenceListItem.ID, OffenceListItem.DateTime, OffenceListItem.Description, OffenceListItem.Category));
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
            List<OffenceListItem> convertedOffences = ConvertListOffenceToOffenceListItem(offence);
            foreach (OffenceListItem offenceItem in convertedOffences)
            {
                offenceListItems.Add(new OffenceListItem(offenceItem.ID, offenceItem.DateTime, offenceItem.Description, offenceItem.Category));
            }

            return offenceListItems;
        }

        /// <summary>
        /// gets all the offences from the db
        /// </summary>
        /// <returns></returns>
        public List<Offence> GetOffences()
        {
            return Offence.OffenceData();
        }
    }
}
