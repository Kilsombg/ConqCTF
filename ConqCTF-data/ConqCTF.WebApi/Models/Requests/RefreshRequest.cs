namespace ConqCTF.WebApi.Models.Requests
{
    public record RefreshRequest
    {
        public string? RefreshToken { get; set; }   
    }
}
