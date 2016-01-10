using System;
using System.Globalization;
using System.Windows.Data;

namespace HikariLauncher.Converter
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class NullToBoolConverter : IValueConverter
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

            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
