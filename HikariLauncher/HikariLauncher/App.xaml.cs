using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using HikariLauncher.Services;
using HikariLauncher.ViewModels;

namespace HikariLauncher
{
    public partial class App : Application
    {
        #region Constants

        private static readonly string CACHE_FOLDER = Environment.ExpandEnvironmentVariables(@"%appdata%\Hikari-Translations\cache");

        #endregion


        #region Propertie & Fields

        private HikariApiService _apiService;
        private ConfigService _configService;
        private PatchService _patchService;

        private MainWindowViewModel _mainWindowViewModel;
        private TopBarViewModel _topBarViewModel;
        private GamesViewModel _gamesViewModel;

        #endregion

        #region Methods

        //TODO DarthAffe 06.01.2016: This is a somehow weird design ...
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += OnDispatcherUnhandledException;

            _apiService = new HikariApiService();
            _configService = new ConfigService();
            _patchService = new PatchService(_apiService, UpdateStatusFunc);

            _topBarViewModel = new TopBarViewModel(_configService);
            _gamesViewModel = new GamesViewModel(_configService, _patchService);
            _mainWindowViewModel = new MainWindowViewModel(_apiService, _topBarViewModel, _gamesViewModel);

            new MainWindow { DataContext = _mainWindowViewModel }.Show();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            File.AppendAllText(Path.Combine(CACHE_FOLDER, "error.log"), dispatcherUnhandledExceptionEventArgs.Exception.Message + "\r\n" + dispatcherUnhandledExceptionEventArgs.Exception.StackTrace + "\r\n" + "\r\n");
        }

        //TODO DarthAffe 09.01.2016: WTH!!!
        private void UpdateStatusFunc(string text, double? value, bool isLoading = true)
        {
            _mainWindowViewModel?.UpdateStatus(text, value, isLoading);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _configService.Persist();

            if (_configService.Config.IsClearCacheOnExit)
                _apiService.ClearCache();
        }

        #endregion
    }
}
