using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        //controls the offences for this window
        private OffenceController _offenceController { get; set; }
        
        private AddOffenceDialogue OffenceDialogue { get; set; }

        private bool AddModeActivated = false;

        public MainWindow()
        {
            InitializeComponent();
            SetMapBackground(172, 199, 242);
            SetZoomBoundaryCheck();
            _offenceController = new OffenceController();
            FillOffenceList();
            map_Main.MouseLeftButtonDown += AddPin;
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
            //Offence offence = e.AddedItems[0] as Offence;
            //TODO: place code for selected offence here
        }

        //when the addOffence button is clicked:
        private void Btn_addOffence_Click(object sender, RoutedEventArgs e)
        {
            //change the cursor and the Add offence button context.
            if(AddModeActivated == true)
            {
                Btn_addOffence.Content = "delict toevoegen";
                Mouse.OverrideCursor = Cursors.Arrow;
                AddModeActivated = false;
            }

            else
            {
                Btn_addOffence.Content = "Annuleer";
                Mouse.OverrideCursor = Cursors.Cross;
                AddModeActivated = true;
            }
        }

        //open the dialog when clicked on the map and AddMode is activiated
        private void AddPin(object sender, MouseButtonEventArgs e)
        {
            //create nieuw offencedialogue when clicked on map
            OffenceDialogue = new AddOffenceDialogue(_offenceController, this);
            if (AddModeActivated == true)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                // Disables the default mouse double-click action.
                e.Handled = true;

                // Determin the location to place the pushpin at on the map.

                //Get the mouse click coordinates
                Point mousePosition = e.GetPosition(this);
                //Convert the mouse coordinates to a locatoin on the map
                Microsoft.Maps.MapControl.WPF.Location location = map_Main.ViewportPointToLocation(mousePosition);

                //create a WijkAgendModels Location and convert the WPF location to that location.
                WijkagentModels.Location newLocation = new WijkagentModels.Location();
                newLocation.Longitude = location.Longitude;
                newLocation.Latitude = location.Latitude;

                //try to show the dialog, catch if the date enterd is in the future                                                   
                OffenceDialogue.Location = newLocation;
                try
                {
                    OffenceDialogue.ShowDialog();
                }

                catch(ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Date cannot be in the future!");
                    OffenceDialogue.Focus();
                } 
            } 
        }

        //refresh the list (used by the offence controller)
        public void refreshList()
        {
            FillOffenceList();
            Btn_addOffence.Content = "delict toevoegen";
            AddModeActivated = false;
        }
    }
 }


