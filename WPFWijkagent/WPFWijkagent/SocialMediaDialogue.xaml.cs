using Microsoft.Maps.MapControl.WPF;
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
using Location = WijkagentModels.Location;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for SocialMediaDialogue.xaml
    /// </summary>
    public partial class SocialMediaDialogue : Window
    {
        private SocialMediaDialogueController controller;
        public SocialMediaDialogue(Pushpin pin, List<Offence> offenceListItems)
        {
            InitializeComponent();
            Location l = new Location(pin.Location.Latitude, pin.Location.Longitude);
            controller = new SocialMediaDialogueController(l, offenceListItems);
            string display = controller.DisplayMessages(controller.RetrieveOffence());
            SocialMediaLabel.Content = display;
        }
    }
}
