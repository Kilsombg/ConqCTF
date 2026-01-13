namespace ConqCTF.WebApi.Models.Auth.Requests
{
    public record LogoutRequest
    {
        public string? RefreshToken { get; set; }
    }
}
