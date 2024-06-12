using CoreLibrary.Authorization.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CoreLibrary.Authorization
{
    public static class TokenValidationService
    {
        private static string _secretKey = "NIJsKJI2LuVeAdpy4bxknCCkbV3j74kj"; //TODO: KEY ATANMALI

        public static bool ValidateToken(string token, out string licenseKey, out string[] permissions)
        {
            licenseKey = null;
            permissions = null;

            try
            {
                var stoken = Convert.FromBase64String(token);
                var decryptedToken = DecryptString(stoken, _secretKey);
                var tokenParts = decryptedToken.Split(':');
                if (tokenParts.Length != 2)
                {
                    return false;
                }

                licenseKey = tokenParts[0];
                permissions = tokenParts[1].Split(',');

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string DecryptString(byte[] cipherText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = EnsureKeyLength(Encoding.UTF8.GetBytes(key), aes.KeySize / 8);
            aes.IV = cipherText.Take(aes.BlockSize / 8).ToArray();
            var actualCipherText = cipherText.Skip(aes.BlockSize / 8).ToArray();

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(actualCipherText);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            var decrypted = sr.ReadToEnd();
            return decrypted;
        }

        private static byte[] EnsureKeyLength(byte[] key, int length)
        {
            if (key.Length == length) return key;
            var newKey = new byte[length];
            Array.Copy(key, newKey, Math.Min(key.Length, newKey.Length));
            return newKey;
        }
    }
}
