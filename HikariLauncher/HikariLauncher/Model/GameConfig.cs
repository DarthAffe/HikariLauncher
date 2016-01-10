namespace HikariLauncher.Model
{
    public class GameConfig : AbstractModel
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

        private string _installDir;
        public string InstallDir
        {
            get { return _installDir; }
            set
            {
                _installDir = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
