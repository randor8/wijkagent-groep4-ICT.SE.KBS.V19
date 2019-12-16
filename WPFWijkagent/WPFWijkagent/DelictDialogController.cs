using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WijkagentModels;
using Location = WijkagentModels.Location;
using WijkagentWPF.database;

namespace WijkagentWPF
{
    public class DelictDialogController
    {
        private readonly List<Offence> _offenceList;
        private readonly Location _location;

        /// <summary>
        /// Window for display of socialMediaMessages in the radius of the given offence
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="offenceListItems"></param>
        public DelictDialogController(Location location, List<Offence> offences)
        {
            _offenceList = offences;
            _location = location;
        }

        /// <summary>
        /// The method executes a LINQ search on the List items and finds the offencelistItem with the same pin. 
        /// </summary>
        /// <returns>The method returns the offence that has the same pin</returns>
        public Offence RetrieveOffence()
        {
            Offence o = null;
            IEnumerable<Offence> offenceQuerry =
            from OffenceItem in _offenceList
            where OffenceItem.LocationID.Latitude == _location.Latitude 
            && OffenceItem.LocationID.Longitude == _location.Longitude
            select OffenceItem;
            foreach (var item in offenceQuerry)
            {
                o = item;
            }
            return o;
        }

        /// <summary>
        /// This method creates a single string from all elements within the list of found SocialMediaItems 
        /// </summary>
        /// <param name="offence"></param>
        /// <returns>string</returns>
        public void DisplayMessages(Offence offence, System.Windows.Controls.ListView wpfLVMessages)
        {
            SocialMediaMessageController socialMediaMessageController = new SocialMediaMessageController();
            wpfLVMessages.ItemsSource = socialMediaMessageController.GetOffenceSocialMediaMessages(offence.ID);
        }
    }
}

