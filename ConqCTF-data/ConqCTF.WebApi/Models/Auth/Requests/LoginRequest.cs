namespace ConqCTF.WebApi.Models.Auth.Requests
{
    public record LoginRequest
    {
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
