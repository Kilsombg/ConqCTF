namespace ConqCTF.Infrastructure.Identity.JWT
{
    public interface IJwtTokenGenerator
    {
        public Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
