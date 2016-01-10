using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using HikariLauncher.Helper;
using HikariLauncher.Model;

namespace HikariLauncher.Services
{
    public class HikariApiService
    {
        #region Constants

        private const string API_URL = "http://hikari-translations.de/api?version={0}";
        private static readonly string CACHE_FOLDER = Environment.ExpandEnvironmentVariables(@"%appdata%\Hikari-Translations\cache");

        #endregion

        #region Constructors

        public HikariApiService()
        {
            if (!Directory.Exists(CACHE_FOLDER))
                Directory.CreateDirectory(CACHE_FOLDER);
        }

        #endregion

        #region Methods

        public ApiMetadata LoadApiData()
        {
            try
            {
                using (WebClient webclient = new WebClient())
                {
                    webclient.Encoding = Encoding.UTF8;
                    string result = webclient.DownloadString(string.Format(API_URL, AssemblyHelper.AssemblyVersion));
                    return SerializationHelper.Deserialize<ApiMetadata>(result);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<Game> ExtractGameData(IList<GameMetadata> gameMetadata)
        {
            try
            {
                List<Game> games = new List<Game>();
                foreach (GameMetadata gm in gameMetadata)
                {
                    games.Add(new Game(gm.Id)
                    {
                        Name = gm.Name,
                        Info = gm.Info,
                        Icon = GetCacheFile(gm.Id, gm.IconHash),
                        Background = GetCacheFile(gm.Id, gm.ImageHash),
                        GameType = (GameType)Enum.Parse(typeof(GameType), gm.GameType),
                        Patch = gm.PatchUrl,
                        StartCommand = string.IsNullOrWhiteSpace(gm.StartCommand) ? null : gm.StartCommand,
                        DefaultPaths = string.IsNullOrWhiteSpace(gm.StartCommand) ? null : gm.DefaultPaths.Split('|').ToList(),
                        PathChecks = gm.PathChecks,
                        FileChecks = gm.FileChecks
                    });
                }

                return games;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void SyncGameCache(IList<GameMetadata> gameMetadata, Action<string, double?, bool> updateStatusFunc)
        {
            try
            {
                CreateCacheDirs(gameMetadata);
                IEnumerable<Tuple<string, string, int>> fileList = CreateFileList(gameMetadata);
                IList<Tuple<string, string, int>> missingFiles = fileList.Where(x => !File.Exists(x.Item1) || GetFileLength(x.Item2) != new FileInfo(x.Item1).Length).ToList();
                if (missingFiles.Any())
                    UpdateCache(missingFiles, updateStatusFunc);

                InitImageCache(fileList);
            }
            catch (Exception ex)
            {
            }
        }

        private IEnumerable<Tuple<string, string, int>> CreateFileList(IEnumerable<GameMetadata> gameMetadata)
        {
            List<Tuple<string, string, int>> fileList = new List<Tuple<string, string, int>>();
            foreach (GameMetadata gm in gameMetadata)
            {
                fileList.Add(new Tuple<string, string, int>(GetCacheFile(gm.Id, gm.IconHash), gm.IconUrl, gm.IconSize));
                fileList.Add(new Tuple<string, string, int>(GetCacheFile(gm.Id, gm.ImageHash), gm.ImageUrl, gm.ImageSize));
            }
            return fileList;
        }

        private void CreateCacheDirs(IEnumerable<GameMetadata> gameMetadata)
        {
            foreach (GameMetadata gm in gameMetadata)
            {
                string dir = Path.Combine(CACHE_FOLDER, gm.Id);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }

        private void UpdateCache(IList<Tuple<string, string, int>> files, Action<string, double?, bool> updateStatusFunc)
        {
            int downloadSize = files.Sum(x => x.Item3);
            int downloaded = 0;

            using (WebClient webClient = new WebClient())
            {
                // ReSharper disable once AccessToModifiedClosure
                webClient.DownloadProgressChanged += (sender, args) => updateStatusFunc("Aktualisiere Cache ...", ((double)(downloaded + args.BytesReceived) / downloadSize) * 100.0, true);
                foreach (Tuple<string, string, int> file in files)
                {
                    webClient.DownloadFileTaskAsync(file.Item2, file.Item1).Wait();
                    downloaded += file.Item3;
                }
            }
        }

        private void InitImageCache(IEnumerable<Tuple<string, string, int>> fileList)
        {
            foreach (string file in fileList.Select(x => x.Item1))
                Application.Current?.Dispatcher?.BeginInvoke((Action)(() => ImageCache.LoadIntoCache(file)));
        }

        private string GetCacheFile(string gameId, string hash)
        {
            return Path.Combine(CACHE_FOLDER, gameId, hash);
        }

        public void ClearCache()
        {
            try
            {
                Directory.Delete(CACHE_FOLDER, true);
            }
            catch (Exception ex)
            {
            }
        }

        public double GetCacheSize()
        {
            return DirectoryHelper.ConvertToMiB(DirectoryHelper.CalculateDirectorySize(CACHE_FOLDER));
        }


        public long GetFileLength(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            using (WebResponse response = request.GetResponse())
                return response.ContentLength;
        }

        #endregion
    }
}
