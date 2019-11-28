using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    public class SocialMediaDialogueController
    {
        private Scraper scraper;
        private List<OffenceListItem> listItems;
        //test variable
        public string testDisplay;

        public Pushpin Pin { get; set; }

        /// <summary>
        /// Window for display of socialMediaMessages in the radius of the given offence
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="offenceListItems"></param>
        public SocialMediaDialogueController(Pushpin pin, List<OffenceListItem> offenceListItems)
        {
            listItems = offenceListItems;
            Pin = pin;
        }

        /// <summary>
        /// The method executes a LINQ search on the List items and finds the offencelistItem with the same pin. 
        /// </summary>
        /// <returns>The method returns the offence that has the same pin</returns>
        public Offence RetrieveOffence()
        {
            Offence o = new Offence();
            IEnumerable<Offence> offenceQuerry =
            from OffenceItem in listItems
            where OffenceItem.Pushpin.Location == Pin.Location
            select OffenceItem.Offence;
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

