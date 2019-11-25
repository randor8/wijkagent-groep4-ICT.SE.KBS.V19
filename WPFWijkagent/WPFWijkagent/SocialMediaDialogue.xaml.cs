using System;
using System.Collections.Generic;
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

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for SocialMediaDialogue.xaml
    /// </summary>
    public partial class SocialMediaDialogue : Window
    {
        Scraper scraper;
        public SocialMediaDialogue()
        {
            InitializeComponent();
            //scraper = new Scraper(offence);
            SocialMediaLabel.Content = "Connection";
        }
        //TODO: Create method to change the label to a display of all the social media messages 
        public void DisplayMessages()
        {
            throw new NotImplementedException();
        }

        //TODO: String builder to construct a single string for all the messages within the SocialMediaMessageData 
        public void CreateOverview()
        {
            throw new NotImplementedException();
        }
    }

   
}
