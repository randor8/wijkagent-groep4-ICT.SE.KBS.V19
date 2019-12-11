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
        private bool _addModeActivated = false;
        SocialMediaDialogue social;

        public MainWindow()
        {
            InitializeComponent();
            SetMapBackground(172, 199, 242);
            SetZoomBoundaryCheck();
            FillCategoryFiltermenu();
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
            }
            else if (wpfMapMain.ZoomLevel > minZoom)
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
            List<Offence> offences = MainWindowController.FilterOffences();

            offences.ForEach(of =>
            {
                of.GetPushpin().MouseDown += Pushpin_MouseDown;
                wpfMapMain.Children.Add(of.GetPushpin());
            });

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
            social = new SocialMediaDialogue((Pushpin)sender, MainWindowController.GetOffences());
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

        private void FillCategoryFiltermenu()
        {
            CheckBox checkBoxAlles = new CheckBox()
            {
                Name = "checkBox_Category_allesTonen",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            }; 
            Label labelAlles = new Label()
            {
                Padding = new Thickness(0, 0, 0, 0),
                Content = "Alles tonen",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            };
            FilterGrid.Children.Add(checkBoxAlles);
            FilterGrid.Children.Add(labelAlles);
            Grid.SetColumn(checkBoxAlles, 0);
            Grid.SetRow(checkBoxAlles, 0);
            Grid.SetColumn(labelAlles, 1);
            Grid.SetRow(labelAlles, 0);

            OffenceCategories[] offenceCategories = (OffenceCategories[])Enum.GetValues(typeof(OffenceCategories));
            for(int i = 0; i < offenceCategories.Length; i++)
            {
                FilterGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
                CheckBox checkBox = new CheckBox()
                {
                    Name = offenceCategories[i].ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    VerticalAlignment = VerticalAlignment.Center 
                };
                checkBox.Checked += CategoryCheckboxChecked;
                checkBox.Unchecked += CategoryCheckboxUnchecked;
                FilterGrid.Children.Add(checkBox);
                Grid.SetColumn(checkBox, 0);
                Grid.SetRow(checkBox, i + 1);

                Label label = new Label() 
                { 
                    Padding = new Thickness(0, 0, 0, 0), 
                    Content = offenceCategories[i], 
                    HorizontalAlignment = HorizontalAlignment.Left, 
                    VerticalAlignment = VerticalAlignment.Center 
                };
                FilterGrid.Children.Add(label);
                Grid.SetColumn(label, 1);
                Grid.SetRow(label, i + 1);
            }
        }
        private void CategoryCheckboxChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                FilterList.AddFilter(new CategoryFilter((OffenceCategories)Enum.Parse(typeof(OffenceCategories), checkBox.Name)));
                FillOffenceList();
            }
        }

        private void CategoryCheckboxUnchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                FilterList.RemoveFilter(new CategoryFilter((OffenceCategories)Enum.Parse(typeof(OffenceCategories), checkBox.Name)));
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
            AddOffenceDialogue OffenceDialogue = new AddOffenceDialogue();
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

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ClickCheck(object sender, RoutedEventArgs e)
        {
            if (FilterStack.Visibility == Visibility.Visible)
            {
                FilterStack.Visibility = Visibility.Collapsed;
                wpfLBSelection.Margin = new Thickness(0, 30, 0, 0);
            }
            else
            {
                FilterStack.Visibility = Visibility.Visible;
                wpfLBSelection.Margin = new Thickness(0, FilterStack.MaxHeight, 0, 0);
            }
            
        }
    }
}