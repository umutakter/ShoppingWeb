using CoreLibrary.Authorization.Interfaces;
using CoreLibrary.Models;
using CoreLibrary.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Authorization
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;

        public TokenService(string secretKey)
        {
            _secretKey = secretKey;
        }

        public string GenerateToken(string licenseKey)
        {
            var repo = new LicenseRepository();
            List<VewLicenseDetails> licenseDetails = repo.GetLicensePermissions(licenseKey);
            if (licenseDetails == null)
                return "";
            var payload = $"{licenseKey}:{string.Join(',', licenseDetails.Select(ld => ld.CombinedName))}";
            var encryptedPayload = EncryptString(payload, _secretKey);
            return Convert.ToBase64String(encryptedPayload);
        }

        private byte[] EncryptString(string plainText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = EnsureKeyLength(Encoding.UTF8.GetBytes(key), aes.KeySize / 8);
            aes.GenerateIV();

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(plainText);
            sw.Close();  // Ensure all data is written and CryptoStream is flushed

            return ms.ToArray();
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
