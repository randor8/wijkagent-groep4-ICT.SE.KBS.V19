using Microsoft.Maps.MapControl.WPF;
using System.Collections.Generic;
using System.Windows;
using WijkagentModels;
using Location = WijkagentModels.Location;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for SocialMediaDialogue.xaml
    /// </summary>
    public partial class SocialMediaDialogue : Window
    {
        private SocialMediaDialogueController _controller;
        public SocialMediaDialogue(Pushpin pin, List<Offence> offences)
        {
            InitializeComponent();
            Location l = new Location(pin.Location.Latitude, pin.Location.Longitude);
            _controller = new SocialMediaDialogueController(l, offences);

            string display = _controller.DisplayMessages(_controller.RetrieveOffence());
            SocialMediaLabel.Text = display;
        }
    }
}
