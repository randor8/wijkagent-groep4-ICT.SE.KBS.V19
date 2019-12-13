using Microsoft.Maps.MapControl.WPF;
using System.IO;
using System.Windows;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string SessionFile = "./Session.cfg";
        private static double _centerX = 52.499620, _centerY = 6.079510;

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
                    case "MapCenter":
                        var locationParts = parts[1].Split('|', 2); // Splitting value into longitude and latitude
                        _centerX = double.Parse(locationParts[0]);
                        _centerY = double.Parse(locationParts[1]);
                        continue;
                    case "MapZoom":
                        MapZoom = double.Parse(parts[1]);
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
            writer.WriteLine($"MapCenter:{center.Longitude}|{center.Latitude}");
            writer.WriteLine($"MapZoom:{zoom}");
            writer.Flush();
            writer.Close();
        }
    }
}
