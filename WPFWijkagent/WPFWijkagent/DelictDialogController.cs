using System.Windows.Controls;
using WijkagentModels;
using WijkagentWPF.database;

namespace WijkagentWPF
{
    public class DelictDialogController
    {
        private readonly SocialMediaMessageController _controller;


        /// <summary>
        /// Window for display of socialMediaMessages in the radius of the given offence
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="offenceListItems"></param>
        public DelictDialogController()
        {
            _controller = new SocialMediaMessageController();
        }

        /// <summary>
        /// This method creates a single string from all elements within the list of found SocialMediaItems 
        /// </summary>
        /// <param name="offence"></param>
        /// <returns>string</returns>
        public void DisplayMessages(Offence offence, ListView wpfLVMessages, int mediatype = 0)
        {
            wpfLVMessages.ItemsSource = _controller.GetOffenceSocialMediaMessages(offence.ID, mediatype);
        }

        public void RetrieveWitnessMessages(Offence offence)
        {
            OffenceController offenceController = new OffenceController();
            Scraper WitnessScraper = new Scraper(offence, true, offenceController.Hashtag(offence));
            WitnessScraper.UpdateSocialMediaMessages(1);
        }

        /// <summary>
        /// Retrieves the rest of the images from the social media message.
        /// </summary>
        /// <param name="image">image to use to find the social media message</param>
        public void ShowImages(SocialMediaImage image)
        {
            new MediaWindow(new SocialMediaImageController().GetSocialMediaImages(image.SocialMediaMessageID)).Show();
        }
    }
}

