using System;
using System.IO;
using System.Windows;
using WijkagentModels;
using WijkagentWPF.Filters;
using Location = Microsoft.Maps.MapControl.WPF.Location;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string SessionFile = "./Session.cfg";
        private static double _centerX = 52.499620, _centerY = 6.079510;

        public const char Separator = '|';
        public const string SessionMapPos = "MapCenter";
        public const string SessionMapZoom = "MapZoom";
        public const string SessionFilterCategories = "FilterCategory";

        public static Location MapLocation
        {
            get => new Location
            {
                Longitude = _centerX,
                Latitude = _centerY
            };
        }

        public static double MapZoom { get; private set; }

        /// <summary>
        /// Checks if the given category exists in the CategoryFilterCOllection and if it has been toggled.
        /// If toggled it returns true.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool IsFilterActive(string category)
        {
            var filters = CategoryFilterCollection.Categories;

            foreach (CategoryFilter filter in filters.Keys)
                if (filter.Category.ToString().Equals(category) && filters[filter])
                    return true;
            return false;
        }

        /// <summary>
        /// This function loads the configuration file and reads it contents.
        /// It loops over all lines and checks if the keys are recognised, the values of these keys are stored in attributes.
        /// </summary>
        public static void LoadSession()
        {
            if (!File.Exists(SessionFile)) return;

            using var reader = File.OpenText(SessionFile);
            string line;

            while ((line = reader.ReadLine()) != null) // checking if we hit the eof, if not; use
            {
                var parts = line.Split(':', 2); // Splits the line, max 2 parts
                switch (parts[0]) // Check what config key has been read
                {
                    case SessionMapPos:
                        var locationParts = parts[1].Split(Separator, 2); // Splitting value into longitude and latitude
                        _centerX = double.Parse(locationParts[0]);
                        _centerY = double.Parse(locationParts[1]);
                        continue;
                    case SessionMapZoom:
                        MapZoom = double.Parse(parts[1]);
                        continue;
                    case SessionFilterCategories:
                        if (parts[1].Length == 0) continue;
                        var categories = parts[1].Split(Separator);
                        foreach (string category in categories)
                            CategoryFilterCollection.Instance.ToggleCategory((OffenceCategories)Enum.Parse(typeof(OffenceCategories), category));
                        continue;
                }
            }
        }

        /// <summary>
        /// This function stores some attributes of the application to a config file.
        /// These values can later be used when the application is loaded.
        /// </summary>
        /// <param name="center">The location to be stored to the config</param>
        /// <param name="zoom">The zoom level to be stored to the config</param>
        public static void SaveSession(Location center, double zoom)
        {
            using var writer = File.CreateText(SessionFile);

            #region SaveMap
            WriteSetting(writer, SessionMapPos, $"{center.Longitude}{Separator}{center.Latitude}");
            WriteSetting(writer, SessionMapZoom, $"{zoom}");
            #endregion

            #region SaveCategories
            var categoryString = "";
            var categoryFilters = CategoryFilterCollection.Categories;
            foreach (var cat in categoryFilters.Keys)
            {
                if (categoryFilters[cat] == false) continue;

                if (categoryString.Length > 0) categoryString += Separator;
                categoryString += cat.Category;
            }
            WriteSetting(writer, SessionFilterCategories, categoryString);
            #endregion

            writer.Close();
        }

        /// <summary>
        /// Write a setting to the file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="setting"></param>
        /// <param name="value"></param>
        private static void WriteSetting(StreamWriter writer, string setting, string value) => writer.WriteLine($"{setting}:{value}");
    }
}
