using System.Collections.Generic;
using System.Linq;
using WijkagentModels;
using WijkagentWPF.database;

namespace WijkagentWPF
{
    public class DelictDialogController
    {

        public DelictDialogController()
        {
            
        }

        /// <summary>
        /// This method changes the itemsource of the given listview to the found socialmedia items
        /// </summary>
        /// <param name="offence">the offence</param>
        /// <param name="wpfLVMessages">the listview that has to be updated/changed</param>
        /// <param name="Mediatype">the mediatype of the requested messages (default is 0)</param>
        public void DisplayMessages(Offence offence, System.Windows.Controls.ListView wpfLVMessages, int Mediatype = 0)
        {
            SocialMediaMessageController socialMediaMessageController = new SocialMediaMessageController();
            wpfLVMessages.ItemsSource = socialMediaMessageController.GetOffenceSocialMediaMessages(offence, Mediatype);
        }
    }
}

