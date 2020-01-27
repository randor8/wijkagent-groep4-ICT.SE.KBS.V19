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
        public AddOffenceDialogue(Location location)
        {
            Location = location;
            //Initializes itself (the Window)
            InitializeComponent();

            //add all enum categories to ComboBox so they can be selected
            InitializeCategories();

            ErrorMessagesVisibility();
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
        /// sets visibility for the error messages
        /// hides all messages by default
        /// </summary>
        /// <param name="omschrijving">hide omschijving error field?</param>
        /// <param name="datTime">hide datetime error field?</param>
        /// <param name="general">hide general error field?</param>
        private void ErrorMessagesVisibility(bool description = true, bool datTime = true, bool general = true)
        {
            wpfLErrorMsg.Visibility = general ? Visibility.Hidden : Visibility.Visible;
            wpfLErrorMsgDatumTijd.Visibility = datTime ? Visibility.Hidden : Visibility.Visible;
            wpfLErrorMsgOmschrijving.Visibility = description ? Visibility.Hidden : Visibility.Visible;
        }


        /// <summary>
        /// checks for validity of the submitted form and submits it if valid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wpfBTNToevoegen_Click(object sender, RoutedEventArgs e)
        {
            bool descriptionPresent = wpfTBOmschrijving.Text.Length > 0;
            OffenceCategories offenceCategories = OffenceCategories.Null;

            ErrorMessagesVisibility();

            //is datetime filled in?
            if (wpfDBDatePicker.SelectedDate.HasValue && wpfTPTimePicker.Value.HasValue)
            {
                DateTime date = wpfDBDatePicker.SelectedDate.Value,
                         time = wpfTPTimePicker.Value.Value,
                         dateTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
                bool dateValid = dateTime != null && dateTime < DateTime.Now;

                //is datetime and 'omschrijving' valid?
                if (dateValid && descriptionPresent)
                {
                    //set category if selected
                    if (wpfCBCategorie.SelectedItem != null)
                    {
                        offenceCategories = (OffenceCategories)wpfCBCategorie.SelectedItem;
                    }
                    MainWindowController.AddOffence(wpfTBOmschrijving.Text, offenceCategories, dateTime, Location);
                    this.Close();

                }
                else
                {
                    ErrorMessagesVisibility(descriptionPresent, dateValid, false);
                }
            }
            else
            {
                ErrorMessagesVisibility(descriptionPresent, false, false);
            }
        }
    }
}
