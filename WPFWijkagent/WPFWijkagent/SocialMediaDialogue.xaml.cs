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

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for SocialMediaDialogue.xaml
    /// </summary>
    public partial class SocialMediaDialogue : Window
    {
        private SocialMediaDialogueController controller;
        public SocialMediaDialogue(Pushpin pin, List<OffenceListItem> offenceListItems)
        {
            InitializeComponent();
            controller = new SocialMediaDialogueController(pin, offenceListItems);
            string display = controller.DisplayMessages(controller.RetrieveOffence());
            SocialMediaLabel.Content = display;
        }
    }
}
