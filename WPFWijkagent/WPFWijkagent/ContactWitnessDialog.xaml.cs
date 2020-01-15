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
using System.Windows.Threading;
using System.ComponentModel;
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for ContactWitnessDialogue.xaml
    /// </summary>
    public partial class ContactWitnessDialog : Window, IObserver
    {
        private MessageScanner _scanner;
        private ContactWitnessDialogController _controller;
        private long _userID = 1194224344144269312;
        public long WitnessID { get; set; }

        public ContactWitnessDialog(long id)
        {
            InitializeComponent();
            WitnessID = id;
            ResizeMode = ResizeMode.NoResize;
            _scanner = new MessageScanner(WitnessID);
            _controller = new ContactWitnessDialogController();
            PrintMessages(_scanner.GetConversation());
            _scanner.Attach(_controller);
            _scanner.Attach(this);
            _scanner.StartScanning(30000);
        }
               

        /// <summary>
        /// prints all messages in the list on the dialogue screen
        /// </summary>
        /// <param name="directMessages"></param>
        public void PrintMessages(List<DirectMessage> directMessages)
        {
            foreach (DirectMessage item in directMessages)
            {
                ListViewItem viewItem = new ListViewItem()
                {
                    ContentTemplate = (DataTemplate)Resources["wpfDTlistbox"],
                    Content = item,
                    Padding = new Thickness(5),
                    Margin = new Thickness(5),
                    BorderBrush = Brushes.Black,
                };
                if (item.SenderID == _userID)
                {
                    viewItem.HorizontalAlignment = HorizontalAlignment.Right;
                    viewItem.HorizontalContentAlignment = HorizontalAlignment.Right;
                }
                else
                {
                    viewItem.HorizontalAlignment = HorizontalAlignment.Left;
                    viewItem.HorizontalContentAlignment = HorizontalAlignment.Left;
                }
                WPFLBMessageBox.Items.Add(viewItem);
            }
        }

        /// <summary>
        /// Updates the dialogue when a new message is received
        /// </summary>
        /// <param name="observable"> the subject that is being observed for updates</param>
        public void Update(IObservable observable)
        {
            this.Dispatcher.Invoke(() =>
            {
                WPFLBMessageBox.Items.Clear();
                PrintMessages(_controller.DirectMessages);
            });
        }

        /// <summary>
        /// Activates when button on dialogue is pressed, adds a message to the dialogue and sends the message to Twitter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = wpfTBinput.Text;
            if (input.Length > 0) 
            {                _controller.DirectMessages.Add(new DirectMessage(_userID, 1, input, DateTime.Now));
                PrintMessages(_controller.DirectMessages);
                _scanner.Scraper.SentDirectMessage(input, WitnessID);
            }
            wpfTBinput.Text = "";
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _scanner.StopScanning();
        }
    }
}
