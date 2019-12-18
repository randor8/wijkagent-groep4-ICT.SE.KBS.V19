using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WijkagentModels;
using WijkagentWPF.database;
using Location = WijkagentModels.Location;

namespace WijkagentWPF
{
    /// <summary>
    /// controls the offence(s) for the wpf application
    /// </summary>
    public static class MainWindowController
    {
        public static readonly SolidColorBrush ColorSelected = new SolidColorBrush(Colors.Red);
        public static readonly SolidColorBrush ColorDefault = new SolidColorBrush(Colors.Blue);

        private static List<Offence> _offences = new List<Offence>();

        private static readonly Dictionary<Offence, Pushpin> _pushpins = new Dictionary<Offence, Pushpin>();

        /// <summary>
        /// Adds an offence to the list of offences for this window.
        /// </summary>
        /// <param name="description">offence description</param>
        /// <param name="category">offence Categorie(enum value)</param>
        /// <param name="dateTime">offence date and time</param>
        /// <param name="location">offence location</param>
        public static void AddOffence(string description, OffenceCategories category, DateTime dateTime, Location location)
        {
            OffenceController offenceController = new OffenceController();
            Offence offence = new Offence(0, dateTime, description, location, category);
            offence.ID = offenceController.SetOffence(
                dateTime,
                description,
                location,
                category);

            Scraper scraper = new Scraper(offence);
            scraper.SetSocialMediaMessages();

            _offences.Add(offence);
        }

        /// <summary>
        /// Applies all filters contained in the FilterList to the offences.
        /// </summary>
        /// <returns></returns>
        public static List<Offence> FilterOffences()
        {
            return FilterList.ApplyFilters(_offences);
        }

        /// <summary>
        /// Gets the pushpin of the offence
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Pushpin GetPushpin(this Offence value)
        {
            if (!_pushpins.ContainsKey(value)) _pushpins.Add(value, CreatePushpin(value));
            return _pushpins[value];
        }

        /// <summary>
        /// Creates a pushpin
        /// </summary>
        /// <param name="offence"></param>
        /// <returns></returns>
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
        /// This function returns the Offence associated with the given pushpin.
        /// </summary>
        /// <param name="pushpin">the pushpin to search</param>
        /// <returns>the offence</returns>
        public static Offence GetOffence(this Pushpin pushpin)
        {
            foreach (Offence offence in _pushpins.Keys)
            {
                if (_pushpins[offence].Equals(pushpin))
                {
                    return offence;
                }
            }
            return null; // This should not be able to happen.
        }

        /// <summary>
        /// gets all the offences from the db
        /// </summary>
        /// <returns>List of offences ordered by date descending</returns>
        public static List<Offence> GetOffences()
        {
            _offences = new OffenceController().GetOffences();

            return _offences;
        }

        /// <summary>
        /// The method executes a LINQ search on the List items and finds the offencelistItem with the same pin. 
        /// </summary>
        /// <returns>The method returns the offence that has the same pin</returns>
        public static Offence RetrieveOffence(double latitude, double longitude)
        {
            Offence offence = null;
            IEnumerable<Offence> offenceQuerry =
                from OffenceItem in _offences
                where OffenceItem.LocationID.Latitude == latitude
                && OffenceItem.LocationID.Longitude == longitude
                select OffenceItem;
            offence = offenceQuerry.First(); 
            return offence;
        }

        /// <summary>
        /// Clears the list offences in this controller.
        /// </summary>
        public static void ClearOffences() => _offences = new List<Offence>();
    }
}
