using System;
using System.Windows;
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

        public Location Location { get; set; }


        //Create the AddOffenceDialogue. this method initializes all the components used by the AddOffenceDialogue
        public AddOffenceDialogue()
        {
            //Initializes itself (the Window)
            InitializeComponent();

            //add all enum categories to ComboBox so they can be selected
            InitializeCategories();
        }


        // Add al the categories (currently from the enum, later on from the database) to the combobox of the dialog
        private void InitializeCategories()
        {
            // add all categories from the OffenceCategories enum to the combobox
            foreach (OffenceCategories categories in (OffenceCategories[])Enum.GetValues(typeof(OffenceCategories)))
            {
                wpfCBCategorie.Items.Add(categories);
            }
        }


        // when 'toevoegen' is clicked. Add Offence to the Controllers offence data and refresh the list of main window.
        private void wpfBTNToevoegen_Click(object sender, RoutedEventArgs e)
        {
            if (wpfDBDatePicker.SelectedDate.HasValue && wpfTPTimePicker.Value.HasValue)
            {
                DateTime date = new DateTime(
                wpfDBDatePicker.SelectedDate.Value.Year,
                wpfDBDatePicker.SelectedDate.Value.Month,
                wpfDBDatePicker.SelectedDate.Value.Day,
                wpfTPTimePicker.Value.Value.Hour,
                wpfTPTimePicker.Value.Value.Minute,
                wpfTPTimePicker.Value.Value.Second,
                wpfTPTimePicker.Value.Value.Millisecond
                );

                if (date != null && date < DateTime.Now && wpfCBCategorie.SelectedItem != null && Location != null)
                {
                    OffenceController.SetOffenceData(wpfTBOmschrijving.Text, (OffenceCategories)wpfCBCategorie.SelectedItem, date, Location);
                    this.Close();
                }
            }
        }
    }
}
