using System.Security.Cryptography;
using System.Text;

namespace Nexora.Core.Common.Helpers
{
    public static class HashGeneratorHelper
    {
        public static string GenerateSHA512Hash(string secretKey, params string[] hashParams)
        {
            var string2Hash = secretKey + String.Join("", hashParams);

            var SHA512Hasher = SHA512.Create();

            var bytes = Encoding.UTF8.GetBytes(string2Hash);

            var hashbytes = SHA512Hasher.ComputeHash(bytes);

            var hash = new StringBuilder();

            for (var i = 0; i < hashbytes.Length; i++)
            {
                hash.Append(hashbytes[i].ToString("X2"));
            }

            return hash.ToString();
        }

        public static string GenerateHMACSHA512Hash(string secretKey, params string[] hashParams)
        {
            var hash = new StringBuilder();

            var secretkeyBytes = Encoding.UTF8.GetBytes(secretKey);

            var inputBytes = Encoding.UTF8.GetBytes(String.Join("", hashParams));

            using (var hmac = new HMACSHA512(secretkeyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);

                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x02"));
                }
            }

            return hash.ToString();
        }

        /// <summary>
        /// It creates a hash with the rsa algorithm using the secret key and hash parameters.
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="hashParams"></param>
        /// <returns></returns>
        public static string GenerateSHA256RSAHash(string secretKey, params string[] hashParams)
        {
            var rsa = CreateRsaProviderFromSecretKey(secretKey);

            var msgBytes = Encoding.UTF8.GetBytes(String.Join(":", hashParams));

            var signatureBytes = rsa.SignData(msgBytes, "SHA256");

            return BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Ready-made code block that creates RSACryptoServiceProvider using secret key.
        /// </summary>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider CreateRsaProviderFromSecretKey(string secretKey)
        {
            var RSAparams = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(Convert.FromBase64String(secretKey))))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();

                if (twobytes == 0x8130)
                {
                    binr.ReadByte();
                }
                else if (twobytes == 0x8230)
                {
                    binr.ReadInt16();
                }
                else
                {
                    throw new Exception("Unexpected value readbinr.ReadUInt16()");
                }

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0102)
                {
                    throw new Exception("Unexpected version");
                }

                bt = binr.ReadByte();

                if (bt != 0x00)
                {
                    throw new Exception("Unexpected value readbinr.ReadByte()");
                }

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            var RSA = new RSACryptoServiceProvider();

            RSA.ImportParameters(RSAparams);

            return RSA;
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            var bt = binr.ReadByte();

            if (bt != 0x02)
            {
                return 0;
            }

            bt = binr.ReadByte();

            int count;

            if (bt == 0x81)
            {
                count = binr.ReadByte();
            }
            else
            {
                if (bt == 0x82)
                {
                    var highbyte = binr.ReadByte();
                    var lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    count = bt;
                }
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }

            binr.BaseStream.Seek(-1, SeekOrigin.Current);

            return count;
        }

        public static string GenerateRandomAccessKey()
        {
            var hmac = new HMACSHA256();

            return Convert.ToBase64String(hmac.Key)[..32];
        }

        public static string GenerateRandomSecretKey()
        {
            var hmac = new HMACSHA256();

            return Convert.ToBase64String(hmac.Key);
        }

        public static string GenerateKey(int length)
        {
            var keyBytes = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }

            return Convert.ToBase64String(keyBytes).Replace("/", "-").Replace("+", "_").TrimEnd('=');
        }
    }
}