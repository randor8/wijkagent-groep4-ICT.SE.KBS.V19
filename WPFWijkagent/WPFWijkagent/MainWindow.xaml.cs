using System;
using System.Collections.Generic;
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
        //controlls the offences for this window
        private OffenceController _offenceController { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            _offenceController = new OffenceController();
            FillOffenceList();
        }
        /// <summary>
        /// fills the listbox with all of the offences 
        /// </summary>
        private void FillOffenceList()
        {
            //convert to offenceListItems (so we can ad our own tostring and retrieve the id in events.)
            List<Offence> offences = _offenceController.GetOffences();
            List<OffenceListItem> offenceListItems = new List<OffenceListItem>(); 
            offences.ForEach(of => offenceListItems.Add(new OffenceListItem(of.ID, of.DateTime, of.Description)));

            wpf_lb_delicten.ItemsSource = offenceListItems;
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
    }


}


