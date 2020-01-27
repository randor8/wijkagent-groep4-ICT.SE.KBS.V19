using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// Interaction logic for MediaWindow.xaml
    /// </summary>
    public partial class MediaWindow : Window
    {
        public MediaWindow(List<SocialMediaImage> images)
        {
            InitializeComponent();
            wpfLVImages.ItemsSource = images;
        }
    }

    public class WindowHeightConverter : IValueConverter
    {
        /// <summary>
        /// Removes a certain amount from the height so object fits inside window.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => double.Parse(value.ToString()) - 46;

        /// <summary>
        /// This function is not actually used...
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 0;
    }

    public class WindowWidthConverter : IValueConverter
    {
        /// <summary>
        /// Removes a certain amount from the width so object fits inside window.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => double.Parse(value.ToString()) - 30;

        /// <summary>
        /// This function is not actually used...
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 0;
    }
}
