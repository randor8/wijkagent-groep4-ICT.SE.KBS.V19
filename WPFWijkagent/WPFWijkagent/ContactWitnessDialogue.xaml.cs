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
        public ContactWitnessDialogueController dialogueController = new ContactWitnessDialogueController(); 

        public ContactWitnessDialogue()
        {
            InitializeComponent();
        }

        public void PrintMessage(string input)
        {
            throw new NotImplementedException();
        }
    }
}
