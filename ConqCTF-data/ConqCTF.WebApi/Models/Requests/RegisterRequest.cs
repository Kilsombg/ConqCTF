namespace ConqCTF.WebApi.Models.Requests
{
    public record RegisterRequest
    {
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
