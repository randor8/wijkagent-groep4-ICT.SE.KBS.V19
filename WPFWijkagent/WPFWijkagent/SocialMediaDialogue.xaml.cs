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
        public SocialMediaDialogue(Location location, Offence offence)
        {
            InitializeComponent();
            _controller = new SocialMediaDialogueController(location, offence);

            string display = _controller.DisplayMessages();
            SocialMediaLabel.Text = display;
        }
    }
}
