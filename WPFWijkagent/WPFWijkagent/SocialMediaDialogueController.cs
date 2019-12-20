using WijkagentModels;
using WijkagentWPF.database;

namespace WijkagentWPF
{
    public class SocialMediaDialogueController
    {
        private Offence _offence { get; set; }

        /// <summary>
        /// Window for display of socialMediaMessages in the radius of the given offence
        /// </summary>
        /// <param name="offence"></param>
        public SocialMediaDialogueController(Offence offence)
        {
            _offence = offence;
        }

        /// <summary>
        /// This method creates a single string from all elements within the list of found SocialMediaItems 
        /// </summary>
        /// <param name="offence"></param>
        /// <returns>string</returns>
        public string DisplayMessages()
        {
            SocialMediaMessageController socialMediaMessageController = new SocialMediaMessageController();
            Scraper scraper = new Scraper(_offence);
            string display = "";

            scraper.UpdateSocialMediaMessages();
            var feed = socialMediaMessageController.GetOffenceSocialMediaMessages(_offence.ID);
            foreach (SocialMediaMessage media in feed)
            {
                display += $"\n{media.ToString()}\n ";
            }
            return display;
        }
    }
}

