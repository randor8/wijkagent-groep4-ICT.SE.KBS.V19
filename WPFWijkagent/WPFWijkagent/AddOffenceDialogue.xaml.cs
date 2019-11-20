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
using WPFWijkagent;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for AddOffenceWindow.xaml
    /// </summary>

    public partial class AddOffenceDialogue : Window
    {
        public OffenceCategories categories = new OffenceCategories();

        private OffenceController Controller { get; set; }

        public Location Location { get; set; }
        public AddOffenceDialogue(OffenceController controller)
        {
            //init windows
            InitializeComponent();

            Controller = controller;

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
            DateTime dateTime = DateTimePicker.Value.Value;
            Controller.SetOffenceData(TxtB_omschrijving.Text, CB_categorie.Text, dateTime, Location);
            this.Hide();
        }
    }
}
