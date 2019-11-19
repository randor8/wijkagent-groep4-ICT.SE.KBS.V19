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
    /// Interaction logic for AddOffenceWindow.xaml
    /// </summary>

    public partial class AddOffenceDialogue : Window
    {
        public OffenceCategories categories = new OffenceCategories();
        private List<OffenceListItem> OffenceList { get; set; }

        //change this to the offences from the offence controller!
        /*
        public enum Offence
        {
            Moord,
            Diefstal,
            Verkrachting,
            Geweldpleging
        }
        */
        public AddOffenceDialogue(List<OffenceListItem> offences)
        {
            //init windows
            InitializeComponent();

            OffenceList = offences;

            //add all enum categories to ComboBox
            InitializeCategories();

            //Init the DateTimePicker
            InitializeDatePicker();
        }

        private void InitializeCategories()
        {
            foreach (OffenceCategories categories in (OffenceCategories[])Enum.GetValues(typeof(OffenceCategories)))
            {
                CB_categorie.Items.Add(categories);
            }
        }

        private void InitializeDatePicker()
        {
            //Install-Package Extended.Wpf.Toolkit -Version 3.6.0
            DateTimePicker.Maximum = DateTime.Now;
        }

        private void Btn_toevoegen_Click(object sender, RoutedEventArgs e)
        {
            int index = OffenceList.Count + 1;
            OffenceList.Add(new OffenceListItem(index, DateTime.Now, TxtB_omschrijving.Text));
            this.Hide();

            foreach(OffenceListItem item in OffenceList)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
