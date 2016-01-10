namespace HikariLauncher.Model
{
    public class FileCheck : AbstractModel
    {
        #region Properties & Fields

        private string _file;
        public string File
        {
            get { return _file; }
            set
            {
                _file = value;
                OnPropertyChanged();
            }
        }

        private string _hash;
        public string Hash
        {
            get { return _hash; }
            set
            {
                _hash = value;
                OnPropertyChanged();
            }
        }

        private string _size;
        public string Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }
        public int? IntSize
        {
            get
            {
                int tmp;
                if (int.TryParse(Size, out tmp))
                    return tmp;
                return null;
            }
        }

        #endregion
    }
}
