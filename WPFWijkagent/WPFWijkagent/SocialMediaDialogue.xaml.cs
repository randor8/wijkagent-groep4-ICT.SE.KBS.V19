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
        private SocialMediaDialogueController _controller;
        public SocialMediaDialogue(Pushpin pin, List<Offence> offences)
        {
            InitializeComponent();
            Location l = new Location(0, pin.Location.Latitude, pin.Location.Longitude);
            _controller = new SocialMediaDialogueController(l, offences);

            string display = _controller.DisplayMessages(_controller.RetrieveOffence());
            SocialMediaLabel.Text = display;
        }
    }
}
