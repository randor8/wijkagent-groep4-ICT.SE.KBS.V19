using System.Collections.Generic;
using System.Linq;
using WijkagentModels;
using WijkagentWPF.database;
using System.Windows.Controls;

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
        public void DisplayMessages(Offence offence, ListView wpfLVMessages)
        {
            wpfLVMessages.ItemsSource = _controller.GetOffenceSocialMediaMessages(offence);
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

