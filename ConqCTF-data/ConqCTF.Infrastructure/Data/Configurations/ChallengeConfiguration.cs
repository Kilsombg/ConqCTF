using ConqCTF.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ConqCTF.Infrastructure.Data.Configurations
{
    public class ChallengeConfiguration : IEntityTypeConfiguration<Challenge>
    {
        public void Configure(EntityTypeBuilder<Challenge> builder)
        {
            builder.ToTable("Challenges");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .IsRequired();

            builder.Property(c => c.Points)
                .IsRequired();

            builder.Property(c => c.FlagHash)
                .IsRequired();

            builder.Property(c => c.Category)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(c => c.Difficulty)
                .HasConversion<int>()
                .IsRequired();

            builder.HasMany(c => c.Files)
                .WithOne()
                .HasForeignKey(f => f.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Hints)
                .WithOne()
                .HasForeignKey(h => h.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
