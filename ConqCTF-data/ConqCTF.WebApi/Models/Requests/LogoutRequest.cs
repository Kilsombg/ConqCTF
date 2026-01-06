namespace ConqCTF.WebApi.Models.Requests
{
    public record LogoutRequest
    {
        public string? RefreshToken { get; set; }
    }
}
