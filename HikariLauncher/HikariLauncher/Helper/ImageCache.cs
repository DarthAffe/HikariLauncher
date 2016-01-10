using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace HikariLauncher.Helper
{
    public static class ImageCache
    {
        #region Properties & Fields

        private static Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();

        #endregion

        #region Methods

        public static BitmapImage GetImage(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            BitmapImage image;
            if (!_images.TryGetValue(path, out image))
                image = LoadImage(path);

            return image;
        }

        public static void LoadIntoCache(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;

            if (!_images.ContainsKey(path))
                LoadImage(path);
        }

        private static BitmapImage LoadImage(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return null;

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(path);
            image.EndInit();

            _images.Add(path, image);

            return image;
        }

        #endregion
    }
}
