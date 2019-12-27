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

        private Offence _offence;

        public DelictDialog(Pushpin pin, Offence offence)
        {
            InitializeComponent();
            _controller = new DelictDialogController();
            _controller.DisplayMessages(offence, wpfLVMessages);
            _controller.DisplayMessages(offence, WitnessMessages, 1);
            _offence = offence;
        }

        /// <summary>
        /// create a witnesscontroller when button is clicked. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_oproep_Click(object sender, RoutedEventArgs e)
        {
            AskForWitnessWindow witnessWindow = new AskForWitnessWindow(_offence);
        }
    }
}
