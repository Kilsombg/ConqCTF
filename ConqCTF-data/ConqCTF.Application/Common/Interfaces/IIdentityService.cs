using ConqCTF.Application.Common.Models;

namespace ConqCTF.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<(Result, string AccessToken, string RefreshToken)> LoginAsync(string email,string password);

        Task<(Result, string AccessToken)> RefreshTokenAsync(string refreshToken);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);

        Task<Result> LogoutAsync(string refreshToken);
    }

}
