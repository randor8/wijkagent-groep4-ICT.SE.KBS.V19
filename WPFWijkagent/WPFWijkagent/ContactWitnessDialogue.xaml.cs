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
        /// Adds a Textblock to the Dialogue
        /// </summary>
        /// <param name="message"> the message of the textblock</param>
        /// <param name="side">the side of the text in the textblock 0 = left & 1 = right </param>
        /// <param name="distance"> distance from the left side of the canvas where the textblock is placed </param>
        public void DrawMessageBlock(DirectMessage message, int side, int distance)
        {
            TextBlock block = new TextBlock();
            block.Text = $"{message._content}\n{message._createdAt}";
            block.Width = 100;
            block.Height = 50;
            block.Background = Brushes.Aqua;
            block.Padding = new Thickness(5);
            block.TextWrapping = TextWrapping.Wrap;
            if (side == 0)
            {
                block.TextAlignment = TextAlignment.Left;
            }
            else
            {
                block.TextAlignment = TextAlignment.Right;
            }
            Canvas.SetLeft(block,distance);
            Canvas.SetTop(block, topdistance);
            ChatCanvas.Children.Add(block);
            ChatCanvas.Height += 70;
            topdistance += 70;
        }

        /// <summary>
        /// prints all messages in the list on the dialogue screen
        /// </summary>
        /// <param name="directMessages"></param>
        public void PrintMessages(List<DirectMessage> directMessages)
        {
            ChatCanvas.Children.Clear();
            topdistance = 20;
            directMessages.Reverse();
            foreach (DirectMessage message in directMessages)
            {
                if (message._senderID == UserID)
                {
                    DrawMessageBlock(message, 0, 600);
                }
                else 
                {
                    DrawMessageBlock(message, 1, 20);
                }
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
            string input = InputLabel.Text;
            if (input.Length > 0) 
            { 
                controller.directMessages.Add(new DirectMessage(UserID, 1, input, DateTime.Now));
                scanner.scraper.SentDirectMessage(input, _witnessID);
                PrintMessages(controller.directMessages);
            }
            InputLabel.Text = "";
        }
    }
}
