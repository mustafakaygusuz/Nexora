using Nexora.Core.Common.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace Nexora.Core.Common.Helpers
{
    public static class EncryptionHelper
    {
        public static string EncryptWithAes(string text, string key, string iv, CipherMode mode = CipherMode.CBC)
        {
            byte[] array;

            using (var aes = Aes.Create())
            {
                aes.Mode = mode;

                var encryptor = aes.CreateEncryptor(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(text);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array).Replace("/", "-").Replace("+", "_");
        }

        public static string DecryptWithAes(string cipherText, string key, string iv, CipherMode mode = CipherMode.CBC)
        {
            var buffer = Convert.FromBase64String(cipherText.Replace("_", "+").Replace("-", "/"));

            using var aes = Aes.Create();
            aes.Mode = mode;

            var decryptor = aes.CreateDecryptor(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        public static string EncryptNumeric(long numericData, string key, string iv, CipherMode mode = CipherMode.CBC) => EncryptWithAes(numericData.ToString(), key, iv, mode);

        public static long? DecryptNumeric(string cipherText, string key, string iv, CipherMode mode = CipherMode.CBC)
        {
            try
            {
                return DecryptWithAes(cipherText, key, iv, mode).ToLong();
            }
            catch (Exception)
            {
                // Ignore exception
            }

            return default;
        }
    }
}