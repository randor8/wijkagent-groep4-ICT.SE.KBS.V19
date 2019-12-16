using System.Windows;
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for SocialMediaDialogue.xaml
    /// </summary>
    public partial class DelictDialog : Window
    {
        private readonly DelictDialogController _controller;

        public DelictDialog(Offence offence)
        {
            InitializeComponent();
            _controller = new DelictDialogController();

            wpfDelict.DataContext = offence;
            _controller.DisplayMessages(offence, wpfLVMessages);
        }

        private void wpfBPrint_Click(object sender, RoutedEventArgs e)
        { }
    }
}
