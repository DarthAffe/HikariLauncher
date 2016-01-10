using System;
using System.Globalization;
using System.Windows.Data;
using HikariLauncher.Helper;

namespace HikariLauncher.Converter
{
    public class ImageCacheConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as string;
            if (string.IsNullOrWhiteSpace(path)) return null;

            return ImageCache.GetImage(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
