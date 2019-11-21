using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WijkagentModels;
using WijkagentWPF;

namespace WPFWijkagent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //controls the offences for this window
        private OffenceController _offenceController { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            _offenceController = new OffenceController();
            FillOffenceList();
            FillCategoriesCombobox();
        }
        /// <summary>
        /// fills the listbox with all of the offences 
        /// </summary>
        private void FillOffenceList()
        {
            //convert to offenceListItems (so we can ad our own tostring and retrieve the id in events.)
            List<OffenceListItem> offenceListItems = new List<OffenceListItem>();
            offenceListItems = ConvertListOffenceToOffenceListItem(_offenceController.GetOffences());

            wpf_lb_delicten.ItemsSource = offenceListItems;
        }

        /// <summary>
        /// Fills the categories combobox
        /// </summary>
        private void FillCategoriesCombobox()
        {

            wpf_cb_categoriesFilter.Items.Add("Alles tonen");

            foreach (OffenceCategories offenceItem in Enum.GetValues(typeof(OffenceCategories)))
            {
                wpf_cb_categoriesFilter.Items.Add(offenceItem);
            }

            wpf_cb_categoriesFilter.SelectedIndex = 0;

        }

        /// <summary>
        /// Converts an Offence list into an OffenceListItem list
        /// </summary>
        /// <param name="offence"></param>
        /// <returns></returns>
        private List<OffenceListItem> ConvertListOffenceToOffenceListItem(List<Offence> offence)
        {
            List<OffenceListItem> offenceListItems = new List<OffenceListItem>();
            foreach (Offence offenceItem in offence)
            {
               offenceListItems.Add(new OffenceListItem(offenceItem.ID, offenceItem.DateTime, offenceItem.Description, offenceItem.Category));
            }

            return offenceListItems;
        }

        /// <summary>
        /// gets called when a offence in the list is clicked/selected.
        /// </summary>
        /// <param name="sender">the publisher</param>
        /// <param name="e">arguments for retrieving the selected item</param>
        private void wpf_lb_delicten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Offence offence = e.AddedItems[0] as Offence;
            //TODO: place code for selected offence here
        }

        /// <summary>
        /// Gets called when the categories combobox selection is changed
        /// Fills the OffenceListItems with the correct Offences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wpf_cb_categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            wpf_lb_delicten.ItemsSource = _offenceController.GetOffenceDataByCategory(wpf_cb_categoriesFilter.SelectedItem.ToString(), _offenceController.GetOffences()); 

        }
    }


}


