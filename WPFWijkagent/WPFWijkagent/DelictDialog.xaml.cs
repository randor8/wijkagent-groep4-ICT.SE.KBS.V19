using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tweetinvi;
using Tweetinvi.Models;
using WijkagentModels;
using WijkagentWPF.database;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for SocialMediaDialogue.xaml
    /// </summary>
    public partial class DelictDialog : Window
    {
        private readonly DelictDialogController _controller;
        private readonly SocialMediaMessageController _messageController;
        private readonly Dictionary<string, Image> _images = new Dictionary<string, Image>();

        /// <summary>
        /// instantiates the window
        /// </summary>
        /// <param name="offence">data to be shown</param>
        public DelictDialog(Offence offence)
        {
            InitializeComponent();
            _controller = new DelictDialogController();
            _messageController = new SocialMediaMessageController();

            wpfDelict.DataContext = offence;
            _controller.DisplayMessages(offence, wpfLVMessages);

            foreach (SocialMediaMessage message in wpfLVMessages.ItemsSource)
            {
                foreach (SocialMediaImage image in message.Media)
                {
                    _images.Add(image.URL, new Image { Source = new BitmapImage(new Uri(image.URL)) });
                }
            }
        }

        /// <summary>
        /// Opens a new window showing all images
        /// </summary>
        /// <param name="sender">item that was clicked on</param>
        /// <param name="e">event arguments</param>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _controller.ShowImages((sender as Image).DataContext as SocialMediaImage);
        }

        private void wpfBTchatbutton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            SocialMediaMessage message = (SocialMediaMessage)button.DataContext;

            IUser user = User.GetUserFromScreenName(message.Handle);
            ContactWitnessDialog witnessDialogue = new ContactWitnessDialog(user.Id);
            witnessDialogue.Show();
        }

        /// <summary>
        /// Click on print button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wpfBPrint_Click(object sender, RoutedEventArgs e)
        {
            var printer = new PrintDialog();
            if (printer.ShowDialog() != true) return;

            var doc = new FixedDocument();
            CalculateSizes(printer, out int margin, out double maxWidth, out double maxHeight);
            var messages = CreateMessagePanels(maxWidth);

            doc.DocumentPaginator.PageSize = new Size(printer.PrintableAreaWidth, printer.PrintableAreaHeight);
            var page = NewPage(doc, margin, out Grid grid, maxWidth);
            AddItem(grid, CreateOffencePanel(maxWidth), 0);
            AddItem(grid, new GridSplitter { Height = 2, HorizontalAlignment = HorizontalAlignment.Stretch, Background = new SolidColorBrush(Colors.LightGray) }, 1);

            int index = 1; // Counting from zero
            foreach (var message in messages)
            {
                grid.Measure(new Size(maxWidth, double.PositiveInfinity));
                if (grid.DesiredSize.Height + message.DesiredSize.Height < maxHeight) AddItem(grid, message, ++index);
                else
                {
                    FinishPage(doc, page, grid);
                    page = NewPage(doc, margin, out grid, maxWidth, true);
                    AddItem(grid, message, index = 0);
                }
            }
            FinishPage(doc, page, grid);
            printer.PrintDocument(doc.DocumentPaginator, "Delict Informatie");
        }

        /// <summary>
        /// calculates sizes used by the pages for the specific printer
        /// </summary>
        /// <param name="printer">used to select printing settings</param>
        /// <param name="margin">size on the page edges</param>
        /// <param name="maxWidth">width of the page</param>
        /// <param name="maxHeight">height of the page</param>
        private void CalculateSizes(PrintDialog printer, out int margin, out double maxWidth, out double maxHeight)
        {
            margin = 30;
            maxWidth = printer.PrintableAreaWidth - (2 * margin);
            maxHeight = printer.PrintableAreaHeight - (2 * margin);
        }

        /// <summary>
        /// Create a new page object and sets up variables for use.
        /// </summary>
        /// <param name="document">used for setting page sizes</param>
        /// <param name="margin">uses for space around the edges</param>
        /// <param name="grid">wpf grid object to add information to</param>
        /// <param name="maxWidth">maximum width of objects</param>
        /// <param name="preMargin">used to add negative margin to account for message top spacing</param>
        /// <returns></returns>
        private FixedPage NewPage(FixedDocument document, int margin, out Grid grid, double maxWidth, bool preMargin = false)
        {
            grid = new Grid { Width = maxWidth };
            if (preMargin) grid.Margin = new Thickness(0, -8, 0, 0); // Account for messages' top spacing

            return new FixedPage()
            {
                Width = document.DocumentPaginator.PageSize.Width,
                Height = document.DocumentPaginator.PageSize.Height,
                Margin = new Thickness(margin)
            };
        }

        /// <summary>
        /// Adds a UI Element to a grid.
        /// </summary>
        /// <param name="grid">grid to add item to</param>
        /// <param name="item">item to add to grid</param>
        /// <param name="row">row to put the item into</param>
        private void AddItem(Grid grid, UIElement item, int row)
        {
            grid.Children.Add(item);
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetRow(item, row);
        }

        /// <summary>
        /// Adds all attributes to current page and adds it to the document.
        /// </summary>
        /// <param name="document">used to store all pages</param>
        /// <param name="page">current page</param>
        /// <param name="grid">current grid</param>
        private void FinishPage(FixedDocument document, FixedPage page, Grid grid)
        {
            page.Children.Add(grid);
            PageContent content = new PageContent();
            ((IAddChild)content).AddChild(page);
            document.Pages.Add(content);
        }

        /// <summary>
        /// Adds information to a grid.
        /// </summary>
        /// <param name="grid">grid to add to</param>
        /// <param name="row">row to add items in</param>
        /// <param name="prefix">information label</param>
        /// <param name="value">information text</param>
        private void AddTextRow(Grid grid, int row, string prefix, string value)
        {
            var txtPrefix = new TextBlock { Text = prefix };
            var txtDivider = new TextBlock { Text = " : " };
            var txtValue = new TextBlock { Text = value };

            grid.Children.Add(txtPrefix);
            grid.Children.Add(txtDivider);
            grid.Children.Add(txtValue);

            Grid.SetColumn(txtPrefix, 0);
            Grid.SetColumn(txtDivider, 1);
            Grid.SetColumn(txtValue, 2);

            Grid.SetRow(txtPrefix, row);
            Grid.SetRow(txtDivider, row);
            Grid.SetRow(txtValue, row);
        }

        /// <summary>
        /// Creates a grid containing the information of an offence.
        /// </summary>
        /// <param name="maxWidth">maximum item width</param>
        /// <returns></returns>
        private Grid CreateOffencePanel(double maxWidth)
        {
            Grid grid = new Grid { Width = maxWidth, Margin = new Thickness(0, 0, 0, 8) };
            Offence offence = wpfDelict.DataContext as Offence;

            for (int i = 0; i < 4; i++) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            AddTextRow(grid, 0, "Datum en tijd", offence.DateTime.ToString());
            AddTextRow(grid, 1, "Delict nummer", $"{offence.ID}");
            AddTextRow(grid, 2, "Categorie", $"{offence.Category}");
            AddTextRow(grid, 3, "Omschrijving", offence.Description);

            grid.Measure(new Size(maxWidth, double.PositiveInfinity)); // Required to calculate if items fit on the page
            return grid;
        }

        /// <summary>
        /// Generates a list containing Grids with message info.
        /// </summary>
        /// <param name="maxWidth">passed to CreateMessagePanel to calculate height with</param>
        /// <returns>a list with all the messages in a grid</returns>
        private List<Grid> CreateMessagePanels(double maxWidth)
        {
            var list = new List<Grid>();
            // Adding all items to the list by creating grid for them.
            _messageController.GetOffenceSocialMediaMessages((wpfDelict.DataContext as Offence).ID).ForEach(m => list.AddRange(CreateMessagePanel(m, maxWidth)));
            return list;
        }

        /// <summary>
        /// Creates a grid showing message information.
        /// </summary>
        /// <param name="message">will be used to show info</param>
        /// <param name="maxWidth">maximum width used to calculate the height</param>
        /// <returns>a list of grids containing the information grid and the images</returns>
        private List<Grid> CreateMessagePanel(SocialMediaMessage message, double maxWidth)
        {
            List<Grid> grids = new List<Grid>();
            var grid = new Grid { Width = maxWidth, Margin = new Thickness(0, 8, 0, 0) }; // Margin between messages on the page
            for (int i = 0; i < 4; i++) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Adding four automatic rows
            for (int i = 0; i < 2; i++) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Adding two automatic collumns
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            AddTextRow(grid, 0, "Gebruiker", message.User);
            AddTextRow(grid, 1, "Twitter naam", message.Handle);
            AddTextRow(grid, 2, "Omschrijving", message.Message);
            AddTextRow(grid, 3, "Datum en tijd", $"{message.DateTime}");

            grid.DataContext = message; // Used to access the message later on for the images
            grid.Measure(new Size(maxWidth, double.PositiveInfinity)); // Required to calculate where to put it on the page
            grids.Add(grid);

            if (message.Media.Count > 0)
            {
                for (int i = 0; i < message.Media.Count; i++)
                {
                    Grid imageGrid = new Grid { Width = maxWidth };
                    imageGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    imageGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    var img1 = _images[message.Media[i].URL];
                    imageGrid.Children.Add(img1);
                    Grid.SetColumn(img1, 0);

                    if (++i < message.Media.Count)
                    {
                        imageGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                        var img2 = _images[message.Media[i].URL];
                        imageGrid.Children.Add(img2);
                        Grid.SetColumn(img2, 1);

                        // Calculating aspect ratios of images
                        var ar1 = img1.Source.Width / img1.Source.Height;
                        var ar2 = img2.Source.Width / img2.Source.Height;
                        var arm = ar1 + ar2; // Max aspect

                        // Percentage of total aspect times maxwidth gives the width for the given aspect ratio making the images equal height
                        img1.Width = ar1 / arm * maxWidth;
                        img2.Width = ar2 / arm * maxWidth;
                    }
                    else img1.Width = maxWidth;

                    imageGrid.Measure(new Size(maxWidth, double.PositiveInfinity));
                    grids.Add(imageGrid);
                }
            }
            return grids;
        }
    }
}
