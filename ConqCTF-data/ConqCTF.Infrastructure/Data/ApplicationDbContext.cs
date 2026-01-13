using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Domain.Entities;
using ConqCTF.Infrastructure.Identity;
using ConqCTF.Infrastructure.Identity.JWT;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ConqCTF.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public DbSet<Challenge> Challenges => Set<Challenge>();

        public DbSet<ChallengeFile> ChallengeFiles => Set<ChallengeFile>();

        public DbSet<ChallengeSolve> ChallengeSolves => Set<ChallengeSolve>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
