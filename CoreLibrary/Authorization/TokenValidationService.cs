using CoreLibrary.Authorization.SecretKeySettings;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CoreLibrary.Authorization
{
    public class TokenValidationService
    {
        private string secretKey;

        public TokenValidationService()
        {
            this.secretKey = BaseAuthSettings.SecretKey;
        }

        public bool ValidateToken(string token, out string licenseKey, out string[] permissions, out string expireDate)
        {
            licenseKey = null;
            permissions = null;
            expireDate = null;

            try
            {
                var stoken = Convert.FromBase64String(token);
                var decryptedToken = DecryptString(stoken, secretKey);
                var tokenParts = decryptedToken.Split("::");
                if (tokenParts.Length != 3)
                {
                    return false;
                }

                licenseKey = tokenParts[0];
                permissions = tokenParts[2].Split(',');
                expireDate = tokenParts[1];

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string DecryptString(byte[] cipherText, string key)
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

        private byte[] EnsureKeyLength(byte[] key, int length)
        {
            if (key.Length == length) return key;
            var newKey = new byte[length];
            Array.Copy(key, newKey, Math.Min(key.Length, newKey.Length));
            return newKey;
        }
    }
}
