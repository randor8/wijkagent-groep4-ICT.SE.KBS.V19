using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WijkagentModels;
using WijkagentWPF.Filters;
using WijkagentWPF.Session;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _addModeActivated = false;
        DelictDialog social;

        public MainWindow()
        {
            FilterList.AddFilter(CategoryFilterCollection.Instance);
            InitializeComponent();

            App.RegisterSession(new SessionMapLocation(wpfMapMain));
            App.RegisterSession(new SessionMapZoom(wpfMapMain));
            App.RegisterSession(new SessionFilterCategories());
            App.LoadSession();

            wpfMapMain.Background = new SolidColorBrush(Color.FromRgb(172, 199, 242));
            wpfMapMain.ViewChangeOnFrame += CheckZoomBoundaries;
            wpfMapMain.MouseLeftButtonDown += AddPin;

            FillCategoryFiltermenu();
            FillOffenceList();
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
            }
            else if (wpfMapMain.ZoomLevel > minZoom)
            {
                wpfMapMain.ZoomLevel = minZoom;
            }
        }

        /// <summary>
        /// fills the listbox with all of the offences 
        /// </summary>
        private void FillOffenceList()
        {
            // convert to offenceListItems (so we can ad our own tostring and retrieve the id in events.)
            RemoveMouseDownEvents();
            wpfMapMain.Children.Clear();
            List<Offence> offences = MainWindowController.FilterOffences();

            offences.ForEach(of =>
            {
                of.GetPushpin().MouseDown += Pushpin_MouseDown;
                wpfMapMain.Children.Add(of.GetPushpin());
            });
            offences = offences.OrderByDescending(x => x.DateTime).ToList();
            wpfLBSelection.ItemsSource = offences;
            wpfLBSelection.Items.Refresh();
        }

        /// <summary>
        /// This method is subscribed to the mousedown event for the pushpin, and opens the socialMediaDialogue, with the information of the pushpin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Pushpin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Pushpin pin = (Pushpin)sender;
            social = new DelictDialog(pin,
                MainWindowController.RetrieveOffence(
                    pin.Location.Latitude,
                    pin.Location.Longitude));
            social.Show();
        }

        /// <summary>
        /// Removes Mouse events from thr Pushpin to make certain the amount of events stays equal to one 
        /// </summary>
        public void RemoveMouseDownEvents()
        {
            if (MainWindowController.GetOffences().Count != 0)
            {
                foreach (var item in MainWindowController.GetOffences())
                {
                    item.GetPushpin().MouseDown -= Pushpin_MouseDown;
                }
            }
        }

        /// <summary>
        /// Fills the Category Tab in the filtermenu.
        /// </summary>
        private void FillCategoryFiltermenu()
        {
            OffenceCategories[] offenceCategories = (OffenceCategories[])Enum.GetValues(typeof(OffenceCategories));
            for (int i = 0; i < offenceCategories.Length - 1; i++)
            {
                FilterGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
                CheckBox checkBox = new CheckBox()
                {
                    Name = offenceCategories[i].ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsChecked = SessionFilterCategories.IsFilterActive(offenceCategories[i].ToString())
                };
                checkBox.Checked += CategoryCheckboxToggle;
                checkBox.Unchecked += CategoryCheckboxToggle;
                FilterGrid.Children.Add(checkBox);
                Grid.SetColumn(checkBox, 0);
                Grid.SetRow(checkBox, i);

                Label label = new Label()
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Content = offenceCategories[i],
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                FilterGrid.Children.Add(label);
                Grid.SetColumn(label, 1);
                Grid.SetRow(label, i);
            }
        }

        /// <summary>
        /// Toggles the Category on or off when a checkbox is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event when a checkbox is clicked.</param>
        /// <param name="e">Parameters given by the sender of the event.</param>
        private void CategoryCheckboxToggle(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                CategoryFilterCollection.Instance.ToggleCategory((OffenceCategories)Enum.Parse(typeof(OffenceCategories), checkBox.Name));
                FillOffenceList();
            }
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
            }
            else
            {
                wpfBTNAddOffence.Content = "Annuleer";
                Mouse.OverrideCursor = Cursors.Cross;
            }
            _addModeActivated = !_addModeActivated;
        }

        private WijkagentModels.Location GetLocationFromClick(MouseButtonEventArgs e)
        {
            // Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            Microsoft.Maps.MapControl.WPF.Location location = wpfMapMain.ViewportPointToLocation(mousePosition);
            return new WijkagentModels.Location(location.Latitude, location.Longitude);
        }

        /// <summary>
        /// shows the dialog (if we are in add mode) and resets the screen
        /// </summary>
        private void AddPin(object sender, MouseButtonEventArgs e)
        {
            if (!_addModeActivated)
            {
                return;
            }

            //show dialog and reset screen 
            AddOffenceDialogue OffenceDialogue = new AddOffenceDialogue(GetLocationFromClick(e));
            e.Handled = true;
            Mouse.OverrideCursor = Cursors.Arrow;
            wpfBTNAddOffence.Content = "delict toevoegen";
            _addModeActivated = false;

            OffenceDialogue.ShowDialog();
            FillOffenceList();
        }

        /// <summary>
        /// gets called when a offence in the list is clicked/selected.
        /// </summary>
        /// <param name="sender">the publisher</param>
        /// <param name="e">arguments for retrieving the selected item</param>
        private void wpfLBSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;

            Offence item = e.AddedItems[0] as Offence;
            wpfMapMain.Center = item.GetPushpin().Location;
            wpfMapMain.ZoomLevel = 16;
            item.GetPushpin().Background = MainWindowController.ColorSelected;

            for (int i = 0; i < e.RemovedItems.Count; i++)
            {
                Offence removed = e.RemovedItems[i] as Offence;
                removed.GetPushpin().Background = MainWindowController.ColorDefault;
            }
        }

        /// <summary>
        /// Reset all category checkboxes in the filter expander
        /// </summary>
        private void ResetCategoryCheckbox()
        {
            foreach (CheckBox item in FilterGrid.Children.OfType<CheckBox>())
            {
                item.IsChecked = false;
            }
        }
        /// <summary>
        /// On click button reset all filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wpfBTNResetFilters_Click(object sender, RoutedEventArgs e)
        {
            ResetCategoryCheckbox();
            FilterList.ClearFilters();
            FillOffenceList();
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Parameters given by the sender.</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            App.SaveSession();
            Application.Current.Shutdown();
        }
    }
}