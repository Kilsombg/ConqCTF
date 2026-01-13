using ConqCTF.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ConqCTF.Infrastructure.Security
{
    public class FlagHasher : IFlagHasher
    {
        private readonly PasswordHasher<string> _hasher = new();

        public string Hash(string flag)
        {
            return _hasher.HashPassword(null!, flag);
        }

        public bool Verify(string flag, string hash)
        {
            var result = _hasher.VerifyHashedPassword(null!, hash, flag);
            return result == PasswordVerificationResult.Success;
        }
    }
}
