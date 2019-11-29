using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using WijkagentModels;
using Location = WijkagentModels.Location;

namespace WijkagentWPF
{
    /// <summary>
    /// controls the offence(s) for the wpf application
    /// </summary>
    public static class OffenceController
    {
        public static readonly SolidColorBrush ColorSelected = new SolidColorBrush(Colors.Red);
        public static readonly SolidColorBrush ColorDefault = new SolidColorBrush(Colors.Blue);

        private static readonly Dictionary<Offence, Pushpin> _pushpins = new Dictionary<Offence, Pushpin>();

        /// <summary>
        /// sets all the offence fields
        /// </summary>
        /// <param name="description">offence description</param>
        /// <param name="category">offence Categorie(enum value)</param>
        /// <param name="dateTime">offence date and time</param>
        /// <param name="location">offence location</param>
        public static void SetOffenceData(string description, OffenceCategories category, DateTime dateTime, Location location)
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
        public static List<Offence> GetOffenceDataByCategory(string categoryFilter, List<Offence> offences)
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

        public static Pushpin GetPushpin(this Offence value)
        {
            if (!_pushpins.ContainsKey(value)) _pushpins.Add(value, CreatePushpin(value));
            return _pushpins[value];
        }

        private static Pushpin CreatePushpin(Offence offence) => new Pushpin
        {
            Location = new Microsoft.Maps.MapControl.WPF.Location
            {
                Latitude = offence.LocationID.Latitude,
                Longitude = offence.LocationID.Longitude
            },
            Background = ColorDefault
        };

        /// <summary>
        /// gets all the offences from the db
        /// </summary>
        /// <returns></returns>
        public static List<Offence> GetOffences() => Offence.OffenceData;
    }
}
