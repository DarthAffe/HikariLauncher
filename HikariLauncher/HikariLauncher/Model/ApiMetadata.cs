using System.Collections.Generic;

namespace HikariLauncher.Model
{
    public class ApiMetadata : AbstractModel
    {
        #region Properties & Fields

        private string _version;
        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChanged();
            }
        }

        private string _download;
        public string Download
        {
            get { return _download; }
            set
            {
                _download = value;
                OnPropertyChanged();
            }
        }

        private List<GameMetadata> _games;
        public List<GameMetadata> Games
        {
            get { return _games; }
            set
            {
                _games = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        #endregion
    }
}
