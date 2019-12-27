using System.Windows;
using System.Windows.Controls;
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

        private void wpfBPrint_Click(object sender, RoutedEventArgs e)
        { }

        /// <summary>
        /// Opens a new window showing all images
        /// </summary>
        /// <param name="sender">item that was clicked on</param>
        /// <param name="e">event arguments</param>
        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SocialMediaMessage msg = (sender as Image).DataContext as SocialMediaMessage;
            new MediaWindow(msg.Media).Show();
        }
    }
}
