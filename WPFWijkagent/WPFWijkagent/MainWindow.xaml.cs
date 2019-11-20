using Microsoft.Maps.MapControl.WPF;
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
        //controls the offences for this window
        private OffenceController _offenceController { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            SetMapBackground(172, 199, 242);
            SetZoomBoundaryCheck();

            _offenceController = new OffenceController();
            FillOffenceList();
        }

        /// <summary>
        /// Sets the background color of the map to the color composed of the given rgb values.
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        public void SetMapBackground(byte r, byte g, byte b)
        {
            map_Main.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        /// <summary>
        /// Adds check on zooming.
        /// </summary>
        public void SetZoomBoundaryCheck()
        {
            map_Main.ViewChangeOnFrame += CheckZoomBoundaries;
        }

        /// <summary>
        /// Makes sure the zoom level will not go beyond the given upper and lower bounds.
        /// </summary>
        /// <param name="sender">Object sending the event.</param>
        /// <param name="e">Parameters given by the sender.</param>
        private void CheckZoomBoundaries(object sender, MapEventArgs e)
        {
            double maxZoom = 3; double minZoom = 20;
            if (sender.Equals(map_Main))
            {
                if (map_Main.ZoomLevel < maxZoom)
                {
                    map_Main.ZoomLevel = maxZoom;
                } else if (map_Main.ZoomLevel > minZoom)
                {
                    map_Main.ZoomLevel = minZoom;
                }
            }
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


