// ReSharper disable MemberCanBePrivate.Global

using System.IO;
using System.Linq;

namespace HikariLauncher.Helper
{
    public static class DirectoryHelper
    {
        public static long CalculateDirectorySize(string directory)
        {
            if (!Directory.Exists(directory)) return 0;
            return CalculateDirectorySize(new DirectoryInfo(directory));
        }

        public static long CalculateDirectorySize(DirectoryInfo d)
        {
            return d.GetFiles().Sum(fi => fi.Length) + d.GetDirectories().Sum(di => CalculateDirectorySize(di));
        }

        public static double ConvertToMiB(long bytes)
        {
            return ((double)bytes / 1024.0) / 1024.0;
        }
    }
}
