using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HikariLauncher.Misc;
using HikariLauncher.Model;
using HikariLauncher.Services;

namespace HikariLauncher.ViewModels
{
    public class MainWindowViewModel : AbstractViewModel
    {
        #region Properties & Fields

        private readonly HikariApiService _apiService;

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (!_isLoading && value)
                    LoadingValue = 0;

                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private double _loadingValue;
        public double LoadingValue
        {
            get { return _loadingValue; }
            set
            {
                _loadingValue = value;
                OnPropertyChanged();
            }
        }

        private string _loadingText;
        public string LoadingText
        {
            get { return _loadingText; }
            set
            {
                _loadingText = value;
                OnPropertyChanged();
            }
        }

        #region ViewModels

        private TopBarViewModel _topBarViewModel;
        public TopBarViewModel TopBarViewModel
        {
            get { return _topBarViewModel; }
            set
            {
                _topBarViewModel = value;
                OnPropertyChanged();
            }
        }

        private GamesViewModel _gamesViewModel;
        public GamesViewModel GamesViewModel
        {
            get { return _gamesViewModel; }
            set
            {
                _gamesViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        private ActionCommand _initializeCommand;
        public ActionCommand InitializeCommand => _initializeCommand ?? (_initializeCommand = new ActionCommand(Initialize));

        #endregion

        #region Constructors

        public MainWindowViewModel(HikariApiService apiService, TopBarViewModel topBarViewModel, GamesViewModel gamesViewModel)
        {
            this._apiService = apiService;
            this.TopBarViewModel = topBarViewModel;
            this.GamesViewModel = gamesViewModel;
        }

        #endregion

        #region Methods

        private async void Initialize()
        {
            await Task.Run(() =>
            {
                UpdateStatus("Initialisiere ...", 0.0);

                ApiMetadata metadata = _apiService.LoadApiData();
                TopBarViewModel.Metadata = metadata;

                _apiService.SyncGameCache(metadata.Games, UpdateStatus);
                TopBarViewModel.CacheSize = _apiService.GetCacheSize();

                UpdateStatus(null, 100.0, false);

                GamesViewModel.Games = _apiService.ExtractGameData(metadata.Games);
                GamesViewModel.SelectedGame = GamesViewModel.Games.FirstOrDefault();
            });
        }

        public void UpdateStatus(string text, double? value, bool isLoading = true)
        {
            Dispatch(() =>
            {
                IsLoading = isLoading;

                if (isLoading)
                {
                    if (text != null)
                        LoadingText = text;

                    if (value.HasValue)
                        LoadingValue = value.Value;
                }
            });
        }

        private void Dispatch(Action action)
        {
            Application.Current?.Dispatcher?.BeginInvoke(action);
        }

        #endregion
    }
}
