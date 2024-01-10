using System.Security.Cryptography;
using System.Text;
using ManageWorker_API.Models;

namespace ManageWorker_API.Service
{
    public static class HashWorker
    {
        public static string GenerateSHA512SaltedHash(string password, string salt = "")
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hash = SHA512.HashData(bytes);

            return Convert.ToBase64String(hash);
        }

        public static string GenerateSpecialHashBySHA512(User user, string password)
        {
            return GenerateSHA512SaltedHash
            (
                GenerateSHA512SaltedHash(user.Login + password),
                GenerateSHA512SaltedHash(user.DateCreated.ToString())
            );
        }
    }
}