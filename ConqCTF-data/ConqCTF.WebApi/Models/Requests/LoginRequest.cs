namespace ConqCTF.WebApi.Models.Requests
{
    public record LoginRequest
    {
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
