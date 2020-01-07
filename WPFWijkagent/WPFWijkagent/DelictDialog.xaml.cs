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
        private readonly DelictDialogController _controller;

        /// <summary>
        /// instantiates the window
        /// </summary>
        /// <param name="offence">data to be shown</param>
        public DelictDialog(Offence offence)
        {
            InitializeComponent();
            _controller = new DelictDialogController();

            wpfDelict.DataContext = offence;
            _controller.DisplayMessages(offence, wpfLVMessages);
        }

        /// <summary>
        /// Opens a new window showing all images
        /// </summary>
        /// <param name="sender">item that was clicked on</param>
        /// <param name="e">event arguments</param>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _controller.ShowImages((sender as Image).DataContext as SocialMediaImage);
        }

        private void wpfBTchatbutton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            SocialMediaMessage message = (SocialMediaMessage)button.DataContext;

            IUser user =User.GetUserFromScreenName(message.Handle);
            ContactWitnessDialogue witnessDialogue = new ContactWitnessDialogue(user.Id);
            witnessDialogue.Show();
        }
    }
}
