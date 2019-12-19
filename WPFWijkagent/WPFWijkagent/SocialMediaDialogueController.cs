using System.Collections.Generic;
using System.Linq;
using WijkagentModels;
using WijkagentWPF.database;
using Location = WijkagentModels.Location;

namespace WijkagentWPF
{
    public class SocialMediaDialogueController
    {
        private Offence _offence { get; set; }
        public Location Location { get; set; }

        /// <summary>
        /// Window for display of socialMediaMessages in the radius of the given offence
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="offenceListItems"></param>
        public SocialMediaDialogueController(Location location, Offence offence)
        {
            _offence = offence;
            Location = location;
        }


        /// <summary>
        /// This method creates a single string from all elements within the list of found SocialMediaItems 
        /// </summary>
        /// <param name="offence"></param>
        /// <returns>string</returns>
        public string DisplayMessages()
        {
            SocialMediaMessageController socialMediaMessageController = new SocialMediaMessageController();
            var feed = socialMediaMessageController.GetOffenceSocialMediaMessages(_offence.ID);
            string display = "";
            if (feed.Count > 0)
            {
                foreach (SocialMediaMessage media in feed)
                {
                    display += $"\n{media.ToString()}\n ";
                }
            }
            return display;
        }
    }
}

