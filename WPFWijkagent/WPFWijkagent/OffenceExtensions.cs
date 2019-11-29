using Microsoft.Maps.MapControl.WPF;
using System.Collections.Generic;
using System.Windows.Media;
using WijkagentModels;

namespace WijkagentWPF
{
    public static class OffenceExtensions
    {
        public static readonly SolidColorBrush ColorSelected = new SolidColorBrush(Colors.Red);
        public static readonly SolidColorBrush ColorDefault = new SolidColorBrush(Colors.Blue);

        private static readonly Dictionary<Offence, Pushpin> _pushpins = new Dictionary<Offence, Pushpin>();

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
    }
}
