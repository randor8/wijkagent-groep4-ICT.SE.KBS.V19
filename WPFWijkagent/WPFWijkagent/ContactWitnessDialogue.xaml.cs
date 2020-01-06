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
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for ContactWitnessDialogue.xaml
    /// </summary>
    public partial class ContactWitnessDialogue : Window, IObserver
    {
        MessageScanner scanner;
        ContactWitnessDialogueController controller;
        private double topdistance = 20;
        long UserID = 1194224344144269312;
        public long _witnessID { get; set; }

        public ContactWitnessDialogue(long id)
        {
            InitializeComponent();
            _witnessID = id;
            ResizeMode = ResizeMode.NoResize;
            scanner = new MessageScanner(_witnessID);
            controller = new ContactWitnessDialogueController(_witnessID);
            PrintMessages(scanner.GetConversation());
            scanner.Attach(controller);
            scanner.Attach(this);
            scanner.StartScanning(60000);
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
                if (item.SenderID == UserID)
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
        /// <param name="observable"></param>
        public void Update(IObservable observable)
        {
            this.Dispatcher.Invoke(() =>
            {
                PrintMessages(controller.directMessages);
            });
        }


        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = wpfTBinput.Text;
            if (input.Length > 0) 
            { 
                controller.directMessages.Add(new DirectMessage(UserID, 1, input, DateTime.Now));
                scanner.scraper.SentDirectMessage(input, _witnessID);
                PrintMessages(controller.directMessages);
            }
            wpfTBinput.Text = "";
        }
    }
}
