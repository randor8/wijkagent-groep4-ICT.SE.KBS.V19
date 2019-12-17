using System;
using System.Windows;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for AskForWitnessWindow.xaml
    /// </summary>
    public partial class AskForWitnessWindow : Window
    {
        private WitnessController _witnessController { get; set; }
        public AskForWitnessWindow(WitnessController witnessController)
        {
            InitializeComponent();
            _witnessController = witnessController;
            lbl_errorbeschrijving.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Set the error to visable if the texbox is empty,
        /// if the textbox is filled in, send the tweet using the controller and close the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_verstuurOproep_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtb_omschrijving.Text))
            {
                lbl_errorbeschrijving.Visibility = Visibility.Hidden;
                _witnessController.SendTweet();
                Close();
            }

            else
            {
                lbl_errorbeschrijving.Visibility = Visibility.Visible;
            }
        }
    }
}
