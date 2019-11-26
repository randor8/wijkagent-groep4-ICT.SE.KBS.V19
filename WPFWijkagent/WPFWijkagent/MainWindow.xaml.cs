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
        // controls the offences for this window
        private readonly OffenceController _offenceController = new OffenceController();
        private bool _addModeActivated = false;

        public MainWindow()
        {
            InitializeComponent();
            SetMapBackground(172, 199, 242);
            FillCategoriesCombobox();
            FillOffenceList();
            wpfMapMain.MouseLeftButtonDown += AddPin;
        }

        /// <summary>
        /// Sets the background color of the map to the color composed of the given rgb values.
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        public void SetMapBackground(byte r, byte g, byte b)
        {
            wpfMapMain.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        /// <summary>
        /// Makes sure the zoom level will not go beyond the given upper and lower bounds.
        /// </summary>
        /// <param name="sender">Object sending the event.</param>
        /// <param name="e">Parameters given by the sender.</param>
        private void CheckZoomBoundaries(object sender, MapEventArgs e)
        {
            double maxZoom = 3; double minZoom = 20;
            if (sender.Equals(wpfMapMain))
            {
                if (wpfMapMain.ZoomLevel < maxZoom)
                {
                    wpfMapMain.ZoomLevel = maxZoom;
                }
                else if (wpfMapMain.ZoomLevel > minZoom)
                {
                    wpfMapMain.ZoomLevel = minZoom;
                }
            }
        }

        /// <summary>
        /// fills the listbox with all of the offences 
        /// </summary>
        private void FillOffenceList()
        {
            // convert to offenceListItems (so we can ad our own tostring and retrieve the id in events.)
            wpfMapMain.Children.Clear();
            List<OffenceListItem> offences = _offenceController.GetOffenceDataByCategory(wpfCBCategoriesFilter.SelectedItem.ToString(), _offenceController.GetOffences());
            List<OffenceListItem> offenceListItems = new List<OffenceListItem>();

            offences.ForEach(of =>
            {
                offenceListItems.Add(of);
                wpfMapMain.Children.Add(of.Pushpin);
            });

            wpfLBSelection.ItemsSource = offenceListItems;
            wpfLBSelection.Items.Refresh();
        }

        /// <summary>
        /// Fills the categories combobox
        /// </summary>
        private void FillCategoriesCombobox()
        {
            wpfCBCategoriesFilter.Items.Add("Alles tonen");

            foreach (OffenceCategories offenceItem in Enum.GetValues(typeof(OffenceCategories)))
            {
                wpfCBCategoriesFilter.Items.Add(offenceItem);
            }

            wpfCBCategoriesFilter.SelectedIndex = 0;
        }

        /// <summary>
        /// Updates the categories combobox
        /// </summary>
        private void UpdateCategoriesCombobox()
        {
            wpfLBSelection.ItemsSource = _offenceController.GetOffenceDataByCategory(wpfCBCategoriesFilter.SelectedItem.ToString(), _offenceController.GetOffences());
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
                offenceListItems.Add(new OffenceListItem(offenceItem));
            }

            return offenceListItems;
        }

        /// <summary>
        /// Gets called when the categories combobox selection is changed
        /// Fills the OffenceListItems with the correct Offences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wpfCBCategoriesFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCategoriesCombobox();
            FillOffenceList();
        }

        /// <summary>
        /// when the addOffence button is clicked:
        /// </summary>
        private void wpfBTNAddOffence_Click(object sender, RoutedEventArgs e)
        {
            // change the cursor and the Add offence button context.
            if (_addModeActivated == true)
            {
                wpfBTNAddOffence.Content = "delict toevoegen";
                Mouse.OverrideCursor = Cursors.Arrow;
                _addModeActivated = false;
            }
            else
            {
                wpfBTNAddOffence.Content = "Annuleer";
                Mouse.OverrideCursor = Cursors.Cross;
                _addModeActivated = true;
            }
        }

        /// <summary>
        /// open the dialog when clicked on the map and AddMode is activiated
        /// </summary>
        private void AddPin(object sender, MouseButtonEventArgs e)
        {
            //create nieuw offencedialogue when clicked on map
            AddOffenceDialogue OffenceDialogue = new AddOffenceDialogue(_offenceController);
            if (!_addModeActivated)
            {
                return;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            // Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            // Convert the mouse coordinates to a locatoin on the map
            Microsoft.Maps.MapControl.WPF.Location location = wpfMapMain.ViewportPointToLocation(mousePosition);

            // create a WijkAgendModels Location and convert the WPF location to that location.
            WijkagentModels.Location newLocation = new WijkagentModels.Location(location.Longitude, location.Latitude);

            // try to show the dialog, catch if the date enterd is in the future                                                   
            OffenceDialogue.Location = newLocation;

            OffenceDialogue.ShowDialog();
            FillOffenceList();
            UpdateCategoriesCombobox();
            wpfBTNAddOffence.Content = "delict toevoegen";
            _addModeActivated = false;
        }

        /// <summary>
        /// gets called when a offence in the list is clicked/selected.
        /// </summary>
        /// <param name="sender">the publisher</param>
        /// <param name="e">arguments for retrieving the selected item</param>
        private void wpfLBSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;

            OffenceListItem item = e.AddedItems[0] as OffenceListItem;
            wpfMapMain.Center = item.Pushpin.Location;
            wpfMapMain.ZoomLevel = 16;
            item.Pushpin.Background = OffenceListItem.ColorSelected;

            for (int i = 0; i < e.RemovedItems.Count; i++)
            {
                OffenceListItem removed = e.RemovedItems[i] as OffenceListItem;
                removed.Pushpin.Background = OffenceListItem.ColorDefault;
            }
        }
    }
}


