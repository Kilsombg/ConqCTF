using ConqCTF.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ConqCTF.Infrastructure.Data.Configurations
{
    public class ChallengeFileConfiguration : IEntityTypeConfiguration<ChallengeFile>
    {
        public void Configure(EntityTypeBuilder<ChallengeFile> builder)
        {
            builder.ToTable("ChallengeFiles");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(f => f.Path)
                .IsRequired();
        }
    }
}
