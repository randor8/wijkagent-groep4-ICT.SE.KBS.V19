using System;
using System.Windows;
using WijkagentWPF;

namespace WPFWijkagent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Btn_addOffence_Click(object sender, RoutedEventArgs e)
        {
            AddOffenceDialogue offenceDialogue = new AddOffenceDialogue();
            offenceDialogue.Show();
        }
    }
}
