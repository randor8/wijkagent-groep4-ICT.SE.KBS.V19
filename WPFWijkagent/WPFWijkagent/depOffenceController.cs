using System;
using System.Collections.Generic;
using WijkagentModels;
using WijkagentWPF.database;

namespace WijkagentWPF
{
    /// <summary>
    /// controls the offence(s) for the wpf application
    /// </summary>
    public class depOffenceController
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
            OffenceController offenceController = new OffenceController();
            int offenceID = offenceController.SetOffence(
                dateTime,
                description,
                location,
                category);
            Scraper scraper = new Scraper(new Offence(
                offenceID, 
                dateTime,
                description,
                location,
                category));
            scraper.GetSocialMediaMessages();
        }

        /// <summary>
        /// Get all offences from a specific category
        /// </summary>
        /// <param name="categoryFilter"></param>
        /// <param name="offences"></param>
        /// <returns></returns>
        public List<Offence> GetOffenceDataByCategory(string categoryFilter, List<Offence> offences)
        {
            List<Offence> offenceItems = new List<Offence>();
            if (categoryFilter == "Alles tonen")
            {
               return offences;
            }
            else
            {
                foreach (Offence offence in offences)
                {
                    if (offence.Category.ToString() == categoryFilter)
                    {
                        offenceItems.Add(offence);
                    }
                }
            }
            return offenceItems;
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
            OffenceController offenceController = new OffenceController();
            return offenceController.GetOffences();
        }
    }
}
