using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tweetinvi;
using Tweetinvi.Models;
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for SocialMediaDialogue.xaml
    /// </summary>
    public partial class DelictDialog : Window
    {
<<<<<<<<< Temporary merge branch 1
        private DelictDialogController _controller;

        private Offence _offence;

        public DelictDialog(Pushpin pin, List<Offence> offences)
        {
            InitializeComponent();
            Location l = new Location(0, pin.Location.Latitude, pin.Location.Longitude);
            _controller = new DelictDialogController(l, offences);
            _offence = _controller.RetrieveOffence();
            _controller.DisplayMessages(_controller.RetrieveOffence(), wpfLVMessages);
        }

        private void wpfBPrint_Click(object sender, RoutedEventArgs e)
=========
        private SocialMediaDialogueController _controller;

        public SocialMediaDialogue(Offence offence)
        {
            InitializeComponent();
            _controller = new SocialMediaDialogueController(offence);
            string display = _controller.DisplayMessages();
            SocialMediaLabel.Text = display;
        }


        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
>>>>>>>>> Temporary merge branch 2
        {

        }
    }
}
