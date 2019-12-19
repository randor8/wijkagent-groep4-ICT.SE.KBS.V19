
using Microsoft.Maps.MapControl.WPF;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WijkagentModels;
using WijkagentWPF.database;
using Location = WijkagentModels.Location;


namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for SocialMediaDialogue.xaml
    /// </summary>
    public partial class DelictDialog : Window
    {
        private readonly DelictDialogController _controller;

        private Offence _offence;

        // path where the pdf file is saved
        string path = Path.GetTempPath() + "PrintDelict.pdf";

        public DelictDialog(Offence offence)
        {
            InitializeComponent();
            _controller = new DelictDialogController();
            _offence = offence;
            wpfDelict.DataContext = offence;
            _controller.DisplayMessages(offence, wpfLVMessages);
        }

        /// <summary>
        /// Create string with all data from social media
        /// </summary>
        /// <returns>social media messages</returns>
        public string PrintOffenceFile()
        {
            SocialMediaMessageController socialMediaMessageController = new SocialMediaMessageController();
            string text = "";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Datum en tijd : {0}", _offence.DateTime);
            sb.AppendLine();
            sb.AppendFormat("Delict nummer : {0}", _offence.ID);
            sb.AppendLine();
            sb.AppendFormat("Categorie : {0}", _offence.Category);
            sb.AppendLine();
            sb.AppendFormat("Omschrijving : {0}", _offence.Description);
            text += sb + "\n";

            socialMediaMessageController.GetOffenceSocialMediaMessages(_offence.ID).ForEach(x => text += socialMediaMessageController.GetSocialMediaMessage(x.ID) + "\n");

            return text;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wpfBPrint_Click(object sender, RoutedEventArgs e)
        {
            // Create the print dialog object and set options
            // Display the dialog. This returns true if the user presses the Print button.
            PrintDialog _pDialog = new PrintDialog();
            TextBlock tb = new TextBlock()
            {
                Text = PrintOffenceFile(),
                Width = 600,
                TextWrapping = TextWrapping.Wrap,
            };

            if (_pDialog.ShowDialog() == true)
            {
                _pDialog.PrintVisual(tb, "media berichten");
            }

        }

        }
}

