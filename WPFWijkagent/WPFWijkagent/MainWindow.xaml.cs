using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WijkagentModels;
using WijkagentWPF.database;
using System.Windows.Threading;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // controls the offences for this window
        private readonly depOffenceController _offenceController = new depOffenceController();
        private bool _addModeActivated = false;
        SocialMediaDialogue social;
        private List<Offence> _offenceList = new List<Offence>();

        public MainWindow()
        {
            InitializeComponent();
            SetMapBackground(172, 199, 242);
            SetZoomBoundaryCheck();
            FillCategoriesCombobox();
            FillOffenceList();
            wpfMapMain.MouseLeftButtonDown += AddPin;
        }

        /// <summary>
        /// Sets the zoom boundary check on the map in the main window.
        /// </summary>
        public void SetZoomBoundaryCheck()
        {
            wpfMapMain.ViewChangeOnFrame += CheckZoomBoundaries;
        }

        /// <summary>
        /// Checks whether the map in the main window is on a zoom level within specified boundaries.
        /// </summary>
        /// <param name="sender">Caller of the event.</param>
        /// <param name="e">Parameters associated with the event.</param>
        private void CheckZoomBoundaries(object sender, MapEventArgs e)
        {
            double maxZoom = 3;
            double minZoom = 20;
            if (wpfMapMain.ZoomLevel < maxZoom)
            {
                wpfMapMain.ZoomLevel = maxZoom;
            } else if (wpfMapMain.ZoomLevel > minZoom)
            {
                wpfMapMain.ZoomLevel = minZoom;
            }
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
        /// fills the listbox with all of the offences 
        /// </summary>
        private void FillOffenceList()
        {
            // convert to offenceListItems (so we can ad our own tostring and retrieve the id in events.)
            RemoveMouseDownEvents();
            wpfMapMain.Children.Clear();
            List<Offence> offences = _offenceController.GetOffenceDataByCategory(wpfCBCategoriesFilter.SelectedItem.ToString(), _offenceController.GetOffences());
            List<OffenceListItem> offenceListItems = new List<OffenceListItem>();
            _offenceList = offences;
            offences.ForEach(of =>
            {
                of.GetPushpin().MouseDown += Pushpin_MouseDown;
                offenceListItems.Add(of.GetListItem());
                wpfMapMain.Children.Add(of.GetPushpin());
            });
            wpfLBSelection.ItemsSource = offenceListItems;
            wpfLBSelection.Items.Refresh();
        }

        /// <summary>
        /// This method is subscribed to the mousedown event for the pushpin, and opens the socialMediaDialogue, with the information of the pushpin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Pushpin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            social = new SocialMediaDialogue((Pushpin)sender, _offenceController.GetOffences());
            social.Show();
        }

        /// <summary>
        /// Removes Mouse events from thr Pushpin to make certain the amount of events stays equal to one 
        /// </summary>
        public void RemoveMouseDownEvents()
        {
            if(_offenceList.Count != 0)
            {
                foreach (var item in _offenceList)
                {
                    item.GetPushpin().MouseDown -= Pushpin_MouseDown;
                }
            }
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
        /// Gets called when the categories combobox selection is changed
        /// Fills the OffenceListItems with the correct Offences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wpfCBCategoriesFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            WijkagentModels.Location newLocation = new WijkagentModels.Location(0, location.Latitude, location.Longitude);

            // try to show the dialog, catch if the date enterd is in the future                                                   
            OffenceDialogue.Location = newLocation;

            OffenceDialogue.ShowDialog();
            FillOffenceList();
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
            wpfMapMain.Center = item.Offence.GetPushpin().Location;
            wpfMapMain.ZoomLevel = 16;
            item.Offence.GetPushpin().Background = OffenceExtensions.ColorSelected;

            for (int i = 0; i < e.RemovedItems.Count; i++)
            {
                OffenceListItem removed = e.RemovedItems[i] as OffenceListItem;
                removed.Offence.GetPushpin().Background = OffenceExtensions.ColorDefault;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}