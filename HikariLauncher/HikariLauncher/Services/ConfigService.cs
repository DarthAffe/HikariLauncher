using System;
using System.IO;
using HikariLauncher.Helper;
using HikariLauncher.Model;

namespace HikariLauncher.Services
{
    public class ConfigService
    {
        #region Constants

        private static readonly string CONFIG_FILE = Environment.ExpandEnvironmentVariables(@"%appdata%\Hikari-Translations\launcher.config");

        #endregion

        #region Properties & Fields

        public Config Config { get; private set; }

        #endregion

        #region Constructors

        public ConfigService()
        {
            LoadOrCreateConfig();
        }

        #endregion

        #region Methods

        private void LoadOrCreateConfig()
        {
            if (File.Exists(CONFIG_FILE))
            {
                try
                {
                    Config = SerializationHelper.Deserialize<Config>(File.ReadAllText(CONFIG_FILE));
                }
                catch (Exception ex)
                {
                    CreateNewConfig();
                }
            }
            else
                CreateNewConfig();
        }

        private void CreateNewConfig()
        {
            Config = new Config();
            Persist();
        }

        public void Persist()
        {
            File.WriteAllText(CONFIG_FILE, SerializationHelper.Serialize(Config));
        }

        #endregion
    }
}
