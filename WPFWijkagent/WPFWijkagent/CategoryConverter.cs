using System;
using System.Globalization;
using System.Windows.Data;
using WijkagentModels;

namespace WijkagentWPF
{
    public class CategoryConverter : IValueConverter
    {
        /// <summary>
        /// Converts category Null to Onbekend.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OffenceCategories categories && categories.Equals(OffenceCategories.Null))
            {
                return "Onbekend";
            }
            return value;
        }

        /// <summary>
        /// Should not be used at any point.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
