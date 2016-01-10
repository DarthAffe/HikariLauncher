using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HikariLauncher.Misc;
using HikariLauncher.Model;
using HikariLauncher.Services;
using Application = System.Windows.Application;

namespace HikariLauncher.ViewModels
{
    public class GamesViewModel : AbstractViewModel, IDataErrorInfo
    {
        #region Constants

        private const string URL_PROJECT = "http://hikari-translations.de/projekte/{0}/";

        #endregion

        #region Properties & Fields

        private readonly Config _config;
        private readonly PatchService _patchService;

        private CancellationTokenSource _tokenSource;

        private IList<Game> _games;
        public IList<Game> Games
        {
            get { return _games; }
            set
            {
                _games = value;
                OnPropertyChanged();
            }
        }

        private Game _selectedGame;
        public Game SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                LoadGameData();
                OnPropertyChanged();
            }
        }

        private string _installDir;
        public string InstallDir
        {
            get { return _installDir; }
            set
            {
                _installDir = value;
                _config[SelectedGame.Id].InstallDir = value;

                CheckIfGameIsPatched();

                OnPropertyChanged();
            }
        }

        private bool? _isGamePatched;
        public bool? IsGamePatched
        {
            get { return _isGamePatched; }
            set
            {
                _isGamePatched = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        private ActionCommand _browseInstallDirCommand;
        public ActionCommand BrowseInstallDirCommand => _browseInstallDirCommand ?? (_browseInstallDirCommand = new ActionCommand(BrowseInstallDir));

        private ActionCommand _browseProjectSiteCommand;
        public ActionCommand OpenProjectSiteCommand => _browseProjectSiteCommand ?? (_browseProjectSiteCommand = new ActionCommand(OpenProjectSite));

        private ActionCommand _installPatchCommand;
        public ActionCommand InstallPatchCommand => _installPatchCommand ?? (_installPatchCommand = new ActionCommand(InstallPatch));

        private ActionCommand _startGameCommand;
        public ActionCommand StartGameCommand => _startGameCommand ?? (_startGameCommand = new ActionCommand(StartGame));

        #endregion

        #region Constructors

        public GamesViewModel(ConfigService configService, PatchService patchService)
        {
            this._patchService = patchService;

            _config = configService.Config;
        }

        #endregion

        #region Methods

        private void LoadGameData()
        {
            if (SelectedGame == null) return;

            InstallDir = _config[SelectedGame.Id].InstallDir;
            if (string.IsNullOrWhiteSpace(InstallDir) && SelectedGame.DefaultPaths != null)
            {
                foreach (string defaultPath in SelectedGame.DefaultPaths)
                    if (SelectedGame.ValidateGamePath(defaultPath) == null)
                    {
                        InstallDir = defaultPath;
                        break;
                    }
            }

            CheckIfGameIsPatched();
        }

        private void CheckIfGameIsPatched()
        {
            _tokenSource?.Cancel();

            IsGamePatched = null;

            _tokenSource = new CancellationTokenSource();
            CancellationToken token = _tokenSource.Token;
            Task.Run(() =>
            {
                bool result = _patchService.CheckIfGameIsPatched(SelectedGame, InstallDir);
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
                Dispatch(() => IsGamePatched = result);
            }, token);
        }

        private void OpenProjectSite()
        {
            Process.Start(string.Format(URL_PROJECT, SelectedGame.Id));
        }

        private void BrowseInstallDir()
        {
            using (FolderBrowserDialog fwd = new FolderBrowserDialog())
            {
                fwd.ShowNewFolderButton = SelectedGame.GameType == GameType.Full;
                fwd.Description = $"\"{SelectedGame.Name}\"-Installationsverzeichnis";

                if (fwd.ShowDialog() == DialogResult.OK)
                    InstallDir = fwd.SelectedPath;
            }
        }

        private async void InstallPatch()
        {
            if (SelectedGame == null || this[nameof(InstallDir)] != null) return;
            await Task.Run(() =>
            {
                _patchService.PatchGame(SelectedGame, InstallDir);
                CheckIfGameIsPatched();
            });
        }

        private void StartGame()
        {
            string[] data = SelectedGame.StartCommand?.Split('|');

            if (data?.Length > 0)
            {
                ProcessStartInfo psi = new ProcessStartInfo(data[0]) { WorkingDirectory = InstallDir };
                if (data.Length > 1)
                    psi.Arguments = data[1];

                Process.Start(psi);
            }
        }

        private void Dispatch(Action action)
        {
            Application.Current?.Dispatcher?.BeginInvoke(action);
        }

        #endregion

        #region IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(InstallDir))
                    return SelectedGame?.ValidateGamePath(InstallDir);

                return null;
            }
        }
        public string Error { get; } = null;

        #endregion
    }
}
