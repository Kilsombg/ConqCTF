using ConqCTF.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ConqCTF.Infrastructure.Data.Configurations
{
    public class ChallengeSolveConfiguration : IEntityTypeConfiguration<ChallengeSolve>
    {
        public void Configure(EntityTypeBuilder<ChallengeSolve> builder)
        {
            builder.ToTable("ChallengeSolves");

            builder.HasKey(cs => cs.Id);

            builder.Property(cs => cs.UserId)
                .IsRequired();

            builder.HasIndex(cs => new { cs.ChallengeId, cs.UserId })
                .IsUnique();

            builder.HasOne<Challenge>()
                .WithMany()
                .HasForeignKey(cs => cs.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
