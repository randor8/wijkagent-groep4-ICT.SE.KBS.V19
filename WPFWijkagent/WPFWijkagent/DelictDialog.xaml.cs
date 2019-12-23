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
        private MainWindow _MainWindow { get; set; }

        public DelictDialog(Pushpin pin, Offence offence, MainWindow window)
        {
            InitializeComponent();
            _controller = new DelictDialogController();
            _controller.DisplayMessages(offence, wpfLVMessages);
            _controller.DisplayMessages(offence, WitnessMessages, 1);
            _offence = offence;
            _MainWindow = window;
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
            WitnessController witnessController = new WitnessController(_offence, _MainWindow);
        }
    }
}
