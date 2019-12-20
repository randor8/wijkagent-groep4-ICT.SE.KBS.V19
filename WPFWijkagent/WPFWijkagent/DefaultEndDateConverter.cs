using System;
using System.Globalization;
using System.Windows.Data;

namespace WijkagentWPF
{
    public class DefaultEndDateConverter : IValueConverter
    {
        /// <summary>
        /// Limits the given date to a date in the past or the current date.
        /// </summary>
        /// <param name="value">DateTime object.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime;
            }
            return DateTime.Today;
        }

        /// <summary>
        /// This method should not be used.
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
