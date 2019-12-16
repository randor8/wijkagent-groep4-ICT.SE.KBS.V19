using System;
using System.Windows;
using WijkagentModels;

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

            //hide error messages
            wpfLErrorMsg.Visibility = Visibility.Hidden;
            wpfLErrorMsg.Visibility = Visibility.Hidden;
            wpfLErrorMsg.Visibility = Visibility.Hidden;
        }


        // Add al the categories (currently from the enum, later on from the database) to the combobox of the dialog
        private void InitializeCategories()
        {
            // add all categories from the OffenceCategories enum to the combobox
            foreach (OffenceCategories categories in (OffenceCategories[])Enum.GetValues(typeof(OffenceCategories)))
            {
                if (categories != OffenceCategories.Null)
                {
                    wpfCBCategorie.Items.Add(categories);
                }
            }
        }


        /// <summary>
        /// checks for validity of the submitted form and submits it if valid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wpfBTNToevoegen_Click(object sender, RoutedEventArgs e)
        {
            //reset all fields
            wpfLErrorMsg.Visibility = Visibility.Hidden;
            wpfLErrorMsg.Visibility = Visibility.Hidden;
            wpfLErrorMsg.Visibility = Visibility.Hidden;

            if (wpfDBDatePicker.SelectedDate.HasValue && wpfTPTimePicker.Value.HasValue)
            {
                DateTime date = new DateTime(
                    wpfDBDatePicker.SelectedDate.Value.Year,
                    wpfDBDatePicker.SelectedDate.Value.Month,
                    wpfDBDatePicker.SelectedDate.Value.Day,
                    wpfTPTimePicker.Value.Value.Hour,
                    wpfTPTimePicker.Value.Value.Minute,
                    wpfTPTimePicker.Value.Value.Second,
                    wpfTPTimePicker.Value.Value.Millisecond);
                OffenceCategories offenceCategories = OffenceCategories.Null;

                //is everything valid?
                if (date != null && date < DateTime.Now && Location != null && wpfTBOmschrijving.Text.Length <= 0)
                {
                    //set category if selected
                    if (wpfCBCategorie.SelectedItem != null)
                    {
                        offenceCategories = (OffenceCategories)wpfCBCategorie.SelectedItem;
                    }
                    MainWindowController.AddOffence(wpfTBOmschrijving.Text, offenceCategories, date, Location);
                    this.Close();

                } else {
                    //display needed errors
                    wpfLErrorMsg.Visibility = Visibility.Visible;
                    
                    if (date == null || date > DateTime.Now && Location != null)
                    {
                        wpfLErrorMsgDatumTijd.Visibility = Visibility.Visible;
                    }
                    if (wpfTBOmschrijving.Text.Length <= 0)
                    {
                        wpfLErrorMsgOmschrijving.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}
