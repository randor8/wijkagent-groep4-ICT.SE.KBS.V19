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


        //Create the AddOffenceDialogue. this method initializes all the components used by the AddOffenceDialogue
        public AddOffenceDialogue(OffenceController controller)
        {
            //Initializes itself (the Window)
            InitializeComponent();

            //init controller and window so these properties can be used later on
            Controller = controller;

            //add all enum categories to ComboBox so they can be selected
            InitializeCategories();
        }


        //Add al the categories (currently from the enum, later on from the database) to the combobox of the dialog
        private void InitializeCategories()
        {
            //add all categories from the OffenceCategories enum to the combobox
            foreach (OffenceCategories categories in (OffenceCategories[])Enum.GetValues(typeof(OffenceCategories)))
            {
                CB_categorie.Items.Add(categories);
            }
        }


        //when 'toevoegen' is clicked. Add Offence to the Controllers offence data and refresh the list of main window.
        private void Btn_toevoegen_Click(object sender, RoutedEventArgs e)
        {
            if(DatePicker.SelectedDate.HasValue && TimePicker.Value.HasValue)
            {
                DateTime date = new DateTime(
                DatePicker.SelectedDate.Value.Year,
                DatePicker.SelectedDate.Value.Month,
                DatePicker.SelectedDate.Value.Day,
                TimePicker.Value.Value.Hour,
                TimePicker.Value.Value.Minute,
                TimePicker.Value.Value.Second,
                TimePicker.Value.Value.Millisecond
                );

                if (date != null && date < DateTime.Now && CB_categorie.SelectedItem != null && Location != null)
                {
                    Controller.SetOffenceData(TxtB_omschrijving.Text, (OffenceCategories)CB_categorie.SelectedItem, date, Location);
                    this.Close();
                }
            }

        }
    }
}
