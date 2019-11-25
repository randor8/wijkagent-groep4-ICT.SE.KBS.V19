using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Media;
using WijkagentModels;
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// class for converting offences to the needed list items
    /// </summary>
    public class OffenceListItem
    {
        public static readonly SolidColorBrush ColorSelected = new SolidColorBrush(Colors.Red);
        public static readonly SolidColorBrush ColorDefault = new SolidColorBrush(Colors.Blue);

        public Offence Offence { get; private set; }
        public Pushpin Pushpin { get; private set; }

        /// <summary>
        /// inits the offence list item so it can be used to display in a list
        /// </summary>
        /// <param name="offence"> the offence db item</param>
        public OffenceListItem(Offence offence)
        {
            Offence = offence;
            Pushpin = new Pushpin
            {
                Name =  offence.ID.ToString(),
                Location = new Microsoft.Maps.MapControl.WPF.Location
                {
                    Latitude = offence.LocationID.Latitude,
                    Longitude = offence.LocationID.Longitude
                },
                Background = ColorDefault
            };
        }

        /// <summary>
        /// creates a string representation of the object
        /// </summary>
        /// <returns> the string representation of the object</returns>
        public override string ToString() => $"{Offence.Description}, {Offence.DateTime}";
    }
}
