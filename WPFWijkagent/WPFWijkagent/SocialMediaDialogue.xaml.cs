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

        public SocialMediaDialogue(Offence offence)
        {
            InitializeComponent();
            _controller = new SocialMediaDialogueController(offence);
            string display = _controller.DisplayMessages();
            SocialMediaLabel.Text = display;
        }


        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
