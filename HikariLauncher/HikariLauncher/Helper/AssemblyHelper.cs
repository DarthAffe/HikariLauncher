using System.Reflection;

namespace HikariLauncher.Helper
{
    public static class AssemblyHelper
    {
        #region Properties & Fields

        public static string AssemblyVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #endregion
    }
}
