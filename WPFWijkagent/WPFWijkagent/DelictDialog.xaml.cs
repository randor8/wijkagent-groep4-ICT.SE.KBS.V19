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
    public partial class DelictDialog : Window
    {
        private DelictDialogController _controller;

        private WitnessController _witnessController;

        private Offence _offence;

        public DelictDialog(Pushpin pin, List<Offence> offences)
        {
            InitializeComponent();
            Location l = new Location(pin.Location.Latitude, pin.Location.Longitude);
            _controller = new DelictDialogController(l, offences);
            _offence = _controller.RetrieveOffence();
            _controller.DisplayMessages(_controller.RetrieveOffence(), wpfLVMessages);
        }

        private void wpfBPrint_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// create a witnesscontroller when button is clicked. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_oproep_Click(object sender, RoutedEventArgs e)
        {
            WitnessController witnessController = new WitnessController(_offence);
        }
    }
}
