using System;
using System.Globalization;
using System.Windows.Data;
using HikariLauncher.Model;

namespace HikariLauncher.Converter
{
    [ValueConversion(typeof(GameType), typeof(string))]
    public class GameTypeToButtonTextConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GameType? gameType = value as GameType?;
            if (!gameType.HasValue) return null;

            switch (gameType.Value)
            {
                case GameType.Patch:
                    return "Patchen";
                case GameType.Full:
                    return "Installieren";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;
            if (string.IsNullOrWhiteSpace(text)) return null;

            switch (text)
            {
                case "Patchen":
                    return GameType.Patch;
                case "Installieren":
                    return GameType.Full;
                default:
                    return null;
            }
        }

        #endregion
    }
}
