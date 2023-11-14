using System.Security.Cryptography;
using System.Text;

namespace Authentication.Services
{
    public class CryptoService
    {
        public string GetHash(string password)
        {
            const string salt = "Ac15!.=";
            return Convert.ToHexString(Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                Encoding.UTF8.GetBytes(salt),
                350000,
                HashAlgorithmName.SHA512,
                128));
        }
        public string GenerateRandomUrlSafeBase64String(int length)
        {
            // Generate a random byte array
            byte[] randomBytes = new byte[length];
            Random random = new();
            random.NextBytes(randomBytes);

            // Convert the byte array to a URL-safe base64 string
            string base64String = Convert.ToBase64String(randomBytes);
            StringBuilder urlSafeStringBuilder = new(base64String);
            urlSafeStringBuilder.Replace("+", "-").Replace("/", "_").Replace("=", "");

            return urlSafeStringBuilder.ToString();
        }
    }
}
