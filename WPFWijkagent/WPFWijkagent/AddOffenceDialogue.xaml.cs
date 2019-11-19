using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WijkagentModels;
using WPFWijkagent;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for AddOffenceWindow.xaml
    /// </summary>

    public partial class AddOffenceDialogue : Window
    {
        public OffenceCategories categories = new OffenceCategories();

        private List<OffenceListItem> OffenceList { get; set; }
        private MainWindow Mainwindow { get; set; }

        public AddOffenceDialogue(List<OffenceListItem> offences, MainWindow mainWindow)
        {
            //init windows
            InitializeComponent();

            OffenceList = offences;
            Mainwindow = mainWindow;

            //add all enum categories to ComboBox
            InitializeCategories();

            //Init the DateTimePicker
            InitializeDatePicker();
        }

        private void InitializeCategories()
        {
            foreach (OffenceCategories categories in (OffenceCategories[])Enum.GetValues(typeof(OffenceCategories)))
            {
                CB_categorie.Items.Add(categories);
            }
        }

        private void InitializeDatePicker()
        {
            //Install-Package Extended.Wpf.Toolkit -Version 3.6.0
            DateTimePicker.Maximum = DateTime.Now;
        }

        private void Btn_toevoegen_Click(object sender, RoutedEventArgs e)
        {
            int index = OffenceList.Count + 1;
            DateTime dateTime = DateTimePicker.Value.Value;
            OffenceList.Add(new OffenceListItem(index, dateTime, TxtB_omschrijving.Text));
            Mainwindow.wpf_lb_delicten.ItemsSource = OffenceList;
            Mainwindow.wpf_lb_delicten.Items.Refresh();
            this.Hide();
        }
    }
}
