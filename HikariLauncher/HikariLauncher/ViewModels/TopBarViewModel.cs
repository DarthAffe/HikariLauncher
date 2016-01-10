using System.Diagnostics;
using System.Windows;
using HikariLauncher.Helper;
using HikariLauncher.Misc;
using HikariLauncher.Model;
using HikariLauncher.Services;

namespace HikariLauncher.ViewModels
{
    public class TopBarViewModel : AbstractViewModel
    {
        #region Properties & Fields
        // ReSharper disable MemberCanBePrivate.Global

        private ApiMetadata _metadata;
        public ApiMetadata Metadata
        {
            get { return _metadata; }
            set
            {
                _metadata = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AvailableVersion));
                OnPropertyChanged(nameof(IsUpdateAvailable));
            }
        }

        public Config Config { get; }

        public string InstalledVersion => AssemblyHelper.AssemblyVersion;

        public string AvailableVersion => Metadata?.Version ?? "-";

        public bool IsUpdateAvailable => InstalledVersion != AvailableVersion && !string.IsNullOrWhiteSpace(Metadata?.Download);

        private double? _cacheSize = null;
        public double? CacheSize
        {
            get { return _cacheSize; }
            set
            {
                _cacheSize = value;
                OnPropertyChanged();
            }
        }

        // ReSharper enable MemberCanBePrivate.Global
        #endregion

        #region Commands

        private ActionCommand _minimizeCommand;
        public ActionCommand MinimizeCommand => _minimizeCommand ?? (_minimizeCommand = new ActionCommand(Minimize));

        private ActionCommand _closeCommand;
        public ActionCommand CloseCommand => _closeCommand ?? (_closeCommand = new ActionCommand(Close));

        private ActionCommand _openHomepageCommand;
        public ActionCommand OpenHomepageCommand => _openHomepageCommand ?? (_openHomepageCommand = new ActionCommand(OpenHomepage));

        private ActionCommand _downloadUpdateCommand;
        public ActionCommand DownloadUpdateCommand => _downloadUpdateCommand ?? (_downloadUpdateCommand = new ActionCommand(DownloadUpdate));

        #endregion

        #region Constructors

        public TopBarViewModel(ConfigService configService)
        {
            Config = configService.Config;
        }

        #endregion

        #region Methods

        private void OpenHomepage()
        {
            Process.Start("http://hikari-translations.de");
        }

        private void DownloadUpdate()
        {
            if (!string.IsNullOrWhiteSpace(Metadata?.Download))
                Process.Start(Metadata.Download);
        }

        private void Minimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Close()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
