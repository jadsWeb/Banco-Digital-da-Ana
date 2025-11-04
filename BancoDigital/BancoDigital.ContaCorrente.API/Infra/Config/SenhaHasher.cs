using System.Security.Cryptography;
using System.Text;

namespace BancoDigital.ContaCorrente.API.Infra.Config
{
    public static class SenhaHasher
    {
        public static (string Hash, string Salt) HashPassword(string senha)
        {
            // Gera salt aleatório (16 bytes)
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            var salt = Convert.ToBase64String(saltBytes);

            // Combina senha + salt e gera hash
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(senha + salt);
            var hash = Convert.ToBase64String(sha256.ComputeHash(combined));

            return (hash, salt);
        }

        public static bool VerifyPassword(string senhaInformada, string hashArmazenado, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(senhaInformada + salt);
            var hash = Convert.ToBase64String(sha256.ComputeHash(combined));

            return hash == hashArmazenado;
        }
    }
}