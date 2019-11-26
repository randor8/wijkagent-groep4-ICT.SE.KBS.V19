using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WijkagentModels;
using WPFWijkagent;

namespace WijkagentWPF
{
    /// <summary>
    /// This window displays the various social Media Messages in the region of a Offence
    /// </summary>
    public partial class SocialMediaDialogue : Window
    {
        private Scraper scraper;
        private List<OffenceListItem> listItems;
        public Pushpin Pin { get; set; }

        /// <summary>
        /// The constructor sets both patameters 
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="offenceListItems"></param>
        public SocialMediaDialogue(Pushpin pin, List<OffenceListItem> offenceListItems)
        {
            InitializeComponent();

            listItems = offenceListItems;
            Pin = pin;

            DisplayMessages(RetrieveOffence());
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
        public void DisplayMessages(Offence offence)
        {
            scraper = new Scraper(offence);
            List<SocialMediaMessage> feed = scraper.GetSocialMediaMessages();
            string display = "";
            foreach (SocialMediaMessage media in feed)
            {
                display += $"\n{media.ToString()}\n ";
            }
            SocialMediaLabel.Content = display;
        }
    }
}
