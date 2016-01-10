using System.Collections.Generic;
using System.Linq;

namespace HikariLauncher.Model
{
    public class Config : AbstractModel
    {
        #region Properties & Fields

        private bool _isClearCacheOnExit = false;
        public bool IsClearCacheOnExit
        {
            get { return _isClearCacheOnExit; }
            set
            {
                _isClearCacheOnExit = value;
                OnPropertyChanged();
            }
        }

        public List<GameConfig> GameConfigs { get; private set; } = new List<GameConfig>();

        public GameConfig this[string id]
        {
            get
            {
                GameConfig gc = GameConfigs.FirstOrDefault(x => x.Id == id);
                if (gc == null)
                    GameConfigs.Add((gc = new GameConfig { Id = id }));

                return gc;
            }
        }

        #endregion
    }
}
