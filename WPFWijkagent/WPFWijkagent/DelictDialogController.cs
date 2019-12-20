﻿using System.Collections.Generic;
using System.Linq;
using WijkagentModels;
using WijkagentWPF.database;

namespace WijkagentWPF
{
    public class DelictDialogController
    {

        /// <summary>
        /// Window for display of socialMediaMessages in the radius of the given offence
        /// </summary>
        /// <param name="offenceListItems"></param>
        public DelictDialogController()
        {
            
        }

        /// <summary>
        /// This method creates a single string from all elements within the list of found SocialMediaItems 
        /// </summary>
        /// <param name="offence"></param>
        /// <returns>string</returns>
        public void DisplayMessages(Offence offence, System.Windows.Controls.ListView wpfLVMessages, int type = 0)
        {
            SocialMediaMessageController socialMediaMessageController = new SocialMediaMessageController();
            wpfLVMessages.ItemsSource = socialMediaMessageController.GetOffenceSocialMediaMessages(offence.ID, type);
        }
    }
}

