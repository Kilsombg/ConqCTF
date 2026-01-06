namespace ConqCTF.Infrastructure.Identity.JWT
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string? Token { get; set; }

        public string? UserId { get; set; }

        public DateTime Expires { get; set; }

        public bool IsRevoked { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
