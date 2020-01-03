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
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for ContactWitnessDialogue.xaml
    /// </summary>
    public partial class ContactWitnessDialogue : Window
    {
        private double topdistance = 20;

        public ContactWitnessDialogue()
        {
            InitializeComponent();
        }

        public TextBlock CreateMessageBlock(DirectMessage message, int side)
        {
            TextBlock block = new TextBlock();
            block.Text = $"{message._content}\n{message._createdAt}";
            block.Width = 60;
            block.Height = 50;
            block.Background = Brushes.Aqua;
            block.Padding = new Thickness(5);
            block.TextWrapping = TextWrapping.Wrap;
            Canvas.SetLeft(block,side);
            Canvas.SetTop(block, topdistance);
            return block;
        }
    }
}
