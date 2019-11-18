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

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for AddOffenceWindow.xaml
    /// </summary>
    public partial class AddOffenceDialogue : Window
    {
        //change this to the offences from the offence controller!
        public enum Offence
        {
            Moord,
            Diefstal,
            Verkrachting,
            Geweldpleging
        }

        public AddOffenceDialogue()
        {
            //init windows
            InitializeComponent();
            
            //add all enum categories to ComboBox
            InitializeCategories();

            //Init the DateTimePicker
            InitializeDatePicker();
        }

        private void InitializeCategories()
        {
            foreach (Offence offence in (Offence[])Enum.GetValues(typeof(Offence)))
            {
                CB_categorie.Items.Add(offence);
            }
        }

        private void InitializeDatePicker()
        {
            //Install-Package Extended.Wpf.Toolkit -Version 3.6.0
            DateTimePicker.Maximum = DateTime.Now;
        }
    }
}
