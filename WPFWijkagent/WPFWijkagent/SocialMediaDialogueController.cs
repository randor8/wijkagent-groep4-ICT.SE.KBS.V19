using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WijkagentModels;
using Location = WijkagentModels.Location;

namespace WijkagentWPF
{
    public class SocialMediaDialogueController
    {
        private Scraper scraper;
        private List<Offence> offenceList;
        //test variable
        public string testDisplay;
        public Location Location { get; set; }

        /// <summary>
        /// Window for display of socialMediaMessages in the radius of the given offence
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="offenceListItems"></param>
        public SocialMediaDialogueController(Location location, List<Offence> offences)
        {
            offenceList = offences;
            Location = location;
        }

        /// <summary>
        /// The method executes a LINQ search on the List items and finds the offencelistItem with the same pin. 
        /// </summary>
        /// <returns>The method returns the offence that has the same pin</returns>
        public Offence RetrieveOffence()
        {
            Offence o = new Offence();
            IEnumerable<Offence> offenceQuerry =
            from OffenceItem in offenceList
            where OffenceItem.LocationID.Latitude == Location.Latitude 
            && OffenceItem.LocationID.Longitude == Location.Longitude
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
        public string DisplayMessages(Offence offence)
        {
            scraper = new Scraper(offence);
            List<SocialMediaMessage> feed = scraper.GetSocialMediaMessages();
            string display = "";
            foreach (SocialMediaMessage media in feed)
            {
                display += $"\n{media.ToString()}\n ";
            }
            return display;
        }
    }
}

