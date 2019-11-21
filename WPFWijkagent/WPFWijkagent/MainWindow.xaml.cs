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
            List<Offence> offences = _offenceController.GetOffences();
            List<OffenceListItem> offenceListItems = new List<OffenceListItem>();
            /*offences.ForEach(of => offenceListItems.Add(new OffenceListItem(of.ID, of.DateTime, of.Description, of.Category)));*/
            offenceListItems = ConvertListOffenceToOffenceListItem(offences);

            wpf_lb_delicten.ItemsSource = offenceListItems;
        }

        private void FillCategoriesCombobox()
        {

            wpf_cb_categoriesFilter.Items.Add("Alles tonen");

            foreach (OffenceCategories loop in Enum.GetValues(typeof(OffenceCategories)))
            {
                wpf_cb_categoriesFilter.Items.Add(loop);
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
            foreach (Offence i in offence)
            {
               offenceListItems.Add(new OffenceListItem(i.ID, i.DateTime, i.Description, i.Category));
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
            List<OffenceListItem> offenceListItems = new List<OffenceListItem>();

            List<OffenceListItem> offences = ConvertListOffenceToOffenceListItem(_offenceController.GetOffences());
            if (wpf_cb_categoriesFilter.SelectedItem.ToString() == "Alles tonen")
            {
                offenceListItems = offences;
            } 
            else
            {
                foreach (OffenceListItem i in offences)
                {

                    if (i.Category.ToString() == wpf_cb_categoriesFilter.SelectedItem.ToString())
                    {
                        offenceListItems.Add(new OffenceListItem(i.ID, i.DateTime, i.Description, i.Category));
                    }

                }
            }

            wpf_lb_delicten.ItemsSource = offenceListItems;
        }
    }


}


