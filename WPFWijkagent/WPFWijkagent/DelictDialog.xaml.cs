﻿using Microsoft.Maps.MapControl.WPF;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
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
        private DelictDialogController _controller;

        private Offence _offence;

        string path = Path.GetTempPath() + "PrintDelict.pdf";

        private Font printFont;
        private StreamReader streamToPrint;
        static string filePath;

        public DelictDialog(Pushpin pin, List<Offence> offences)
        {
            InitializeComponent();
            Location l = new Location(0, pin.Location.Latitude, pin.Location.Longitude);
            _controller = new DelictDialogController(l, offences);
            _offence = _controller.RetrieveOffence();
            _controller.DisplayMessages(_controller.RetrieveOffence(), wpfLVMessages);
        }

        public void PrintOffenceFile()
        {
            string text = "";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            SocialMediaMessageController socialMediaMessageController = new SocialMediaMessageController();
            socialMediaMessageController.GetOffenceSocialMediaMessages(_controller.RetrieveOffence().ID).ForEach(x => text += socialMediaMessageController.GetSocialMediaMessage(x.ID) + "\n");

            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XTextFormatter tf = new XTextFormatter(gfx);
            // Create a font
            XFont font = new XFont("Verdana", 10, XFontStyle.BoldItalic);

            // Draw the text
            tf.DrawString(text, font, XBrushes.Black,
              new XRect(0, 0, page.Width, page.Height),
              XStringFormats.TopLeft);

            // Save the document...
            const string filename = "HelloWorld.pdf";
            document.Save(path);
            // ...and start a viewer.

            Printing();

        }

        // The PrintPage event is raised for each page to be printed.
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            String line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Iterate over the file, printing each line.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        // Print the file.
        public void Printing()
        {
            try
            {

                streamToPrint = new StreamReader(path);
                try
                {
                    printFont = new Font("Arial", 10);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    /*pd.PrinterSettings.PrinterName = printer;*/
                    // Print the document.
                    pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void wpfBPrint_Click(object sender, RoutedEventArgs e)
        {
            // Create the print dialog object and set options
            PrintDialog pDialog = new PrintDialog();
            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
            pDialog.UserPageRangeEnabled = true;

            // Display the dialog. This returns true if the user presses the Print button.
            Nullable<Boolean> print = pDialog.ShowDialog();
            /*var settings = pDialog.Printer*/
            if (print == true)
            {
                
                PrintOffenceFile();
            }
            
        }

        }
}

