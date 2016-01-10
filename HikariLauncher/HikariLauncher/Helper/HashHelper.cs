using System;
using System.IO;
using System.Security.Cryptography;

namespace HikariLauncher.Helper
{
    public static class HashHelper
    {
        public static string CalcMd5(string file)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                using (Stream stream = File.OpenRead(file))
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
