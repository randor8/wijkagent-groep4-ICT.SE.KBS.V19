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

        private OffenceController Controller { get; }

        public Location Location { get; set; }

        public MainWindow MainWindow { get; }

        public AddOffenceDialogue(OffenceController controller, MainWindow mainWindow)
        {
            //init windows
            InitializeComponent();

            //init controller and window
            Controller = controller;
            MainWindow = mainWindow;

            //add all enum categories to ComboBox
            InitializeCategories();

            //Init the DateTimePicker
            InitializeDatePicker();
        }

        private void InitializeCategories()
        {
            //add all categories from the OffenceCategories enum to the combobox
            foreach (OffenceCategories categories in (OffenceCategories[])Enum.GetValues(typeof(OffenceCategories)))
            {
                CB_categorie.Items.Add(categories);
            }
        }

        private void InitializeDatePicker()
        {
            //Install-Package Extended.Wpf.Toolkit -Version 3.6.0

            //set the maximum date for the datetimepicker
            DateTimePicker.Maximum = DateTime.Now;
        }

        //when 'toevoegen' is clicked. Add Offence to the Controllers offence data and refresh the list of main window.
        private void Btn_toevoegen_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTimePicker.Value.Value;
            if(dateTime < DateTime.Now)
            {
                Controller.SetOffenceData(TxtB_omschrijving.Text,(OffenceCategories) CB_categorie.SelectedItem, dateTime, Location);
                this.Close();
                MainWindow.refreshList();
            }
        }
    }
}
