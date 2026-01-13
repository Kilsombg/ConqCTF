namespace ConqCTF.WebApi.Models.Auth.Requests
{
    public record RefreshRequest
    {
        public string? RefreshToken { get; set; }
    }
}
