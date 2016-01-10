using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HikariLauncher.Converter
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullToCollapsedVisibilityConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = value != null;
            bool invert;
            if (!bool.TryParse(parameter?.ToString(), out invert))
                invert = false;

            if (invert)
                val = !val;

            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
