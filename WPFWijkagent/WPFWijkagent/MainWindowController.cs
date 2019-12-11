using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using WijkagentModels;
using Location = WijkagentModels.Location;
using WijkagentWPF.database;

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
            scraper.GetSocialMediaMessages();

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
        public static List<Offence> GetOffences()
        {
            _offences = new OffenceController().GetOffences();
            return _offences;
        }

        /// <summary>
        /// Clears the list offences in this controller.
        /// </summary>
        public static void ClearOffences() => _offences = new List<Offence>();
    }
}
