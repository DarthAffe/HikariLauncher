using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HikariLauncher.Converter
{
    public class ProgressToOpacityMaskConverter : IMultiValueConverter
    {
        #region Constants

        private static readonly Color TRANSPARENT = Color.FromArgb(0, 0, 0, 0);
        private static readonly Color OPAQUE = Color.FromArgb(255, 0, 0, 0);

        #endregion

        #region Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double value = double.Parse(values[0].ToString());
            double maxValue = double.Parse(values[1].ToString());

            if (value <= 0) return new SolidColorBrush(TRANSPARENT);
            if (value >= maxValue) return new SolidColorBrush(OPAQUE);

            double percentage = value / maxValue;

            GradientStopCollection gradientStops = new GradientStopCollection();
            gradientStops.Add(new GradientStop(OPAQUE, 0.0));
            gradientStops.Add(new GradientStop(OPAQUE, percentage - 0.01));
            gradientStops.Add(new GradientStop(TRANSPARENT, percentage + 0.01));
            gradientStops.Add(new GradientStop(TRANSPARENT, 1.0));

            return new LinearGradientBrush(gradientStops, new Point(0.5, 1.0), new Point(0.5, 0.0));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
