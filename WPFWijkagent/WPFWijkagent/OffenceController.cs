using System;
using System.Collections.Generic;
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// controls the offence(s) for the wpf application
    /// </summary>
    public class OffenceController
    {
        private readonly Dictionary<Offence, OffenceListItem> _offenceItems = new Dictionary<Offence, OffenceListItem>();

        /// <summary>
        /// sets all the offence fields
        /// </summary>
        /// <param name="description">offence description</param>
        /// <param name="category">offence Categorie(enum value)</param>
        /// <param name="dateTime">offence date and time</param>
        /// <param name="location">offence location</param>
        public void SetOffenceData(string description, OffenceCategories category, DateTime dateTime, Location location)
        {
            Offence NewOffence = new Offence
            {
                Category = category,
                Description = description,
                DateTime = dateTime,
                LocationID = location
            };
            //TODO: save the location as a separate object
            Offence.OffenceData.Add(NewOffence);
        }

        /// <summary>
        /// Get all offences from a specific category
        /// </summary>
        /// <param name="categoryFilter"></param>
        /// <param name="offences"></param>
        /// <returns></returns>
        public List<Offence> GetOffenceDataByCategory(string categoryFilter, List<Offence> offences)
        {
            if (categoryFilter == "Alles tonen")
            {
               return offences;
            }
            else
            {
                OffenceCategories offenceCategorie = (OffenceCategories)System.Enum.Parse(typeof(OffenceCategories), categoryFilter);
                FilterList.AddFilter(new CategoryFilter(offenceCategorie));
                return FilterList.ApplyFilters(offences);
            }
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
                offenceListItems.Add(offenceItem.GetListItem());
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
