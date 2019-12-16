﻿using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using WijkagentModels;
using Location = WijkagentModels.Location;
using WijkagentWPF.database;
using System.Linq;

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
        /// Get all offences from a specific category
        /// </summary>
        /// <param name="categoryFilter"></param>
        /// <param name="offences"></param>
        /// <returns></returns>
        public static List<Offence> GetOffencesByCategory(string categoryFilter)
        {
            List<Offence> filteredOffences = new List<Offence>();
            if (categoryFilter == "Alles tonen")
            {
                return _offences;
            }
            else
            {
                foreach (Offence offence in _offences)
                {
                    if (offence.Category.ToString() == categoryFilter)
                    {
                        filteredOffences.Add(offence);
                    }
                }
            }

            return filteredOffences;
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
