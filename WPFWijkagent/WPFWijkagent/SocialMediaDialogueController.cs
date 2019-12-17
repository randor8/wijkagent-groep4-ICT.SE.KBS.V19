using System.Collections.Generic;
using System.Linq;
using WijkagentModels;
using WijkagentWPF.database;
using Location = WijkagentModels.Location;

namespace WijkagentWPF
{
    public class SocialMediaDialogueController
    {
        private List<Offence> _offenceList;
        public Location Location { get; set; }

        /// <summary>
        /// Window for display of socialMediaMessages in the radius of the given offence
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="offenceListItems"></param>
        public SocialMediaDialogueController(Location location, List<Offence> offences)
        {
            _offenceList = offences;
            Location = location;
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
            where OffenceItem.Location.Latitude == Location.Latitude
            && OffenceItem.Location.Longitude == Location.Longitude
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
            SocialMediaMessageController socialMediaMessageController = new SocialMediaMessageController();
            var feed = socialMediaMessageController.GetOffenceSocialMediaMessages(offence.ID);
            string display = "";
            foreach (SocialMediaMessage media in feed)
            {
                display += $"\n{media.ToString()}\n ";
            }
            return display;
        }
    }
}

