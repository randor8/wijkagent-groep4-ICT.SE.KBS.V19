using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WijkagentModels;
using WijkagentWPF;

namespace WPFWijkagent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OffenceController _offenceController { get; set; }
        
        private AddOffenceDialogue OffenceDialogue { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            map_Main.Background = new SolidColorBrush(Color.FromRgb(172, 199, 242));
            SetZoomBoundaryCheck();
            _offenceController = new OffenceController();
            FillOffenceList();
            OffenceDialogue = new AddOffenceDialogue(_offenceController);
        }

        public void SetZoomBoundaryCheck()
        {
            map_Main.ViewChangeOnFrame += CheckZoomBoundaries;
        }

        public void CheckZoomBoundaries(object sender, MapEventArgs e)
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
            List<OffenceListItem> offenceListItems = new List<OffenceListItem>();
            List<Offence> offences = _offenceController.GetOffences();
            offences.ForEach(of => offenceListItems.Add(new OffenceListItem(of.ID, of.DateTime, of.Description)));
            wpf_lb_delicten.ItemsSource = offenceListItems;
            wpf_lb_delicten.Items.Refresh();
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

        private void Btn_addOffence_Click(object sender, RoutedEventArgs e)
        {
            OffenceDialogue.ShowDialog();
            FillOffenceList();
        }
    }


}


