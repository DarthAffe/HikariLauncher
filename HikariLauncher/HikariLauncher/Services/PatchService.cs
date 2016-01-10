using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using HikariLauncher.Helper;
using HikariLauncher.Model;

namespace HikariLauncher.Services
{
    public class PatchService
    {
        #region Constants

        private static readonly string CACHE_FOLDER = Environment.ExpandEnvironmentVariables(@"%appdata%\Hikari-Translations\cache");

        #endregion

        #region Properties & Fields

        private readonly HikariApiService _hikariApiService;
        private readonly Action<string, double?, bool> _updateStatusFunc;

        #endregion

        #region Constructors

        public PatchService(HikariApiService hikariApiService, Action<string, double?, bool> updateStatusFunc)
        {
            this._hikariApiService = hikariApiService;
            this._updateStatusFunc = updateStatusFunc;
        }

        #endregion

        #region Methods

        public bool CheckIfGameIsPatched(Game game, string installDir)
        {
            if (!Directory.Exists(installDir)) return false;

            if (game.FileChecks != null)
                foreach (FileCheck check in game.FileChecks)
                {
                    if (string.IsNullOrWhiteSpace(check.File)) continue;

                    string file = Path.Combine(installDir, check.File);

                    if (!File.Exists(file)) return false;
                    if (check.IntSize.HasValue && check.IntSize != new FileInfo(file).Length) return false;
                    if (!string.IsNullOrWhiteSpace(check.Hash) && !string.Equals(check.Hash, HashHelper.CalcMd5(file), StringComparison.OrdinalIgnoreCase)) return false;
                }

            return true;
        }

        public void PatchGame(Game game, string installDir)
        {
            if (string.IsNullOrWhiteSpace(game?.Patch)) return;

            string patch = DownloadPatch(game);
            UnpackPatch(patch, installDir);
            AutorunIfNeeded(installDir);
            _updateStatusFunc(null, 100.0, false);
        }

        private string DownloadPatch(Game game)
        {
            _updateStatusFunc("Downloade Dateien ...", 0.0, true);

            string file = Path.Combine(CACHE_FOLDER, game.Id, Path.GetFileName(game.Patch));

            if (!File.Exists(file) || _hikariApiService.GetFileLength(game.Patch) != new FileInfo(file).Length)
                using (WebClient webClient = new WebClient())
                {
                    // ReSharper disable once AccessToModifiedClosure
                    webClient.DownloadProgressChanged += (sender, args) => _updateStatusFunc(null, ((double)args.BytesReceived / (double)args.TotalBytesToReceive) * 100.0, true);
                    webClient.DownloadFileTaskAsync(game.Patch, file).Wait();
                }

            return file;
        }

        private void UnpackPatch(string patch, string installDir)
        {
            _updateStatusFunc("Entpacke Dateien ...", 0.0, true);

            using (FileStream fs = File.OpenRead(patch))
            using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Read, false, Encoding.GetEncoding(850))) //HACK DarthAffe 09.01.2016: Holy shit ... microsoft ...
            {
                long totalSize = archive.Entries.Sum(x => x.Length);
                long extractedSize = 0;
                byte[] buffer = new byte[1 << 18]; // 256 kiB;

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (string.IsNullOrWhiteSpace(entry.Name)) continue;

                    string fileName = entry.FullName.Replace("/", @"\");

                    string newFilePath = Path.Combine(installDir, fileName);
                    if (!Directory.Exists(Path.GetDirectoryName(newFilePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(newFilePath));

                    using (FileStream newFile = File.Create(newFilePath))
                    {
                        using (Stream fileData = entry.Open())
                        {
                            int len = fileData.Read(buffer, 0, buffer.Length);
                            while (len > 0)
                            {
                                extractedSize += len;
                                _updateStatusFunc(null, ((double)extractedSize / (double)totalSize) * 100.0, true);

                                newFile.Write(buffer, 0, len);
                                len = fileData.Read(buffer, 0, buffer.Length);
                            }
                            newFile.Flush();
                        }
                    }
                }
            }
        }

        private void AutorunIfNeeded(string installDir)
        {
            string autorunFile = Path.Combine(installDir, "hikari_patch_autorun.bat");
            if (File.Exists(autorunFile))
                Process.Start(new ProcessStartInfo(autorunFile) { WorkingDirectory = installDir });
        }

        #endregion
    }
}
