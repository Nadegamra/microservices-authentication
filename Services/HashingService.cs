using System.Security.Cryptography;
using System.Text;

namespace Authentication.Services
{
    public class HashingService
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
    }
}
