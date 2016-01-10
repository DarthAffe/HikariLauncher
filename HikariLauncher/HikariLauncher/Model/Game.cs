using System;
using System.Collections.Generic;
using System.IO;
using HikariLauncher.Helper;

namespace HikariLauncher.Model
{
    public class Game : AbstractModel
    {
        #region Properties & Fields

        public string Id { get; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _info;
        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                OnPropertyChanged();
            }
        }

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                OnPropertyChanged();
            }
        }

        private string _background;
        public string Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged();
            }
        }

        private GameType _gameType;
        public GameType GameType
        {
            get { return _gameType; }
            set
            {
                _gameType = value;
                OnPropertyChanged();
            }
        }

        private string _patch;
        public string Patch
        {
            get { return _patch; }
            set
            {
                _patch = value;
                OnPropertyChanged();
            }
        }

        private string _startCommand;
        public string StartCommand
        {
            get { return _startCommand; }
            set
            {
                _startCommand = value;
                OnPropertyChanged();
            }
        }

        private List<string> _defaultPaths;
        public List<string> DefaultPaths
        {
            get { return _defaultPaths; }
            set
            {
                _defaultPaths = value;
                OnPropertyChanged();
            }
        }

        private List<FileCheck> _pathChecks;
        public List<FileCheck> PathChecks
        {
            get { return _pathChecks; }
            set
            {
                _pathChecks = value;
                OnPropertyChanged();
            }
        }

        private List<FileCheck> _fileChecks;
        public List<FileCheck> FileChecks
        {
            get { return _fileChecks; }
            set
            {
                _fileChecks = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public Game(string id)
        {
            this.Id = id;
        }

        #endregion

        #region Methods

        public string ValidateGamePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return "Es muss ein Installationsverzeichnis angegeben werden.";
            if (!Directory.Exists(path)) return $"Das Verzeichnis '{path}' konnte nicht gefunden werden";

            if (PathChecks != null)
                foreach (FileCheck check in PathChecks)
                {
                    string file = Path.Combine(path, check.File);

                    if (!File.Exists(file)) return $"Die Datei '{file}' existiert nicht im angegebenen Pfad.";
                    if (check.IntSize.HasValue && check.IntSize != new FileInfo(file).Length)
                        return $"Die Datei '{file}' ist nicht so wie erwartet.";
                    if (!string.IsNullOrWhiteSpace(check.Hash) && string.Equals(check.Hash, HashHelper.CalcMd5(file), StringComparison.OrdinalIgnoreCase))
                        return $"Die Datei '{file}' ist nicht so wie erwartet.";
                }

            return null;
        }

        #endregion
    }
}
