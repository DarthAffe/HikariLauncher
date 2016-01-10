using System.Collections.Generic;

namespace HikariLauncher.Model
{
    public class GameMetadata : AbstractModel
    {
        #region Properties & Fields

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

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

        private string _iconHash;
        public string IconHash
        {
            get { return _iconHash; }
            set
            {
                _iconHash = value;
                OnPropertyChanged();
            }
        }

        private int _iconSize;
        public int IconSize
        {
            get { return _iconSize; }
            set
            {
                _iconSize = value;
                OnPropertyChanged();
            }
        }

        private string _iconUrl;
        public string IconUrl
        {
            get { return _iconUrl; }
            set
            {
                _iconUrl = value;
                OnPropertyChanged();
            }
        }

        private string _imageHash;
        public string ImageHash
        {
            get { return _imageHash; }
            set
            {
                _imageHash = value;
                OnPropertyChanged();
            }
        }

        private int _imageSize;
        public int ImageSize
        {
            get { return _imageSize; }
            set
            {
                _imageSize = value;
                OnPropertyChanged();
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                OnPropertyChanged();
            }
        }

        private string _gameType;
        public string GameType
        {
            get { return _gameType; }
            set
            {
                _gameType = value;
                OnPropertyChanged();
            }
        }

        private string _patchUrl;
        public string PatchUrl
        {
            get { return _patchUrl; }
            set
            {
                _patchUrl = value;
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

        private string _defaultPaths;
        public string DefaultPaths
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
    }
}
