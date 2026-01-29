using ConqCTF.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ConqCTF.Infrastructure.Data.Configurations
{
    public class ChallengeHintConfiguration : IEntityTypeConfiguration<ChallengeHint>
    {
        public void Configure(EntityTypeBuilder<ChallengeHint> builder)
        {
            builder.ToTable("ChallengeHints");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Text)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne<Challenge>()
                .WithMany(c => c.Hints)
                .HasForeignKey(h => h.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
