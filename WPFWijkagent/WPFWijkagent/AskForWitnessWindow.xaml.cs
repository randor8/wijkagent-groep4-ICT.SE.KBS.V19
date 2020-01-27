using System;
using System.Windows;
using WijkagentModels;
using WijkagentWPF.database;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for AskForWitnessWindow.xaml
    /// </summary>
    public partial class AskForWitnessWindow : Window
    {
        private WitnessController _witnessController { get; set; }
        private SendMessageController _SendMessageController { get; set; }

        public AskForWitnessWindow(Offence offence)
        {
            InitializeComponent();
            _witnessController = new WitnessController(offence, this);
            _SendMessageController = new SendMessageController();
            txtb_omschrijving.Text = offence.Description;
            ShowDialog();
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
                if (!_SendMessageController.MessageExists(_witnessController.CreateWitnessMessage()))
                {
                    lbl_errorbeschrijving.Visibility = Visibility.Hidden;
                    lbl_DubbeleMelding.Visibility = Visibility.Hidden;
                    _witnessController.SendTweet();
                    Close();
                }
                else
                {
                    lbl_DubbeleMelding.Visibility = Visibility.Visible;
                }
            }
            else
            {
                lbl_errorbeschrijving.Visibility = Visibility.Visible;
            }
        }
    }
}
