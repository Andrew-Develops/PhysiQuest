using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class QuestConfiguration : IEntityTypeConfiguration<Quest>
    {
        public void Configure(EntityTypeBuilder<Quest> builder)
        {
            builder.ToTable("Quests");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
                .ValueGeneratedOnAdd();

            builder.Property(q => q.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(q => q.Description)
                .HasMaxLength(500);

            builder.Property(q => q.RewardPoints)
                .IsRequired();

            builder.Property(q => q.RewardTokens)
                .IsRequired();

            builder.HasMany(q => q.UserQuests)
                .WithOne(uq => uq.Quest)
                .HasForeignKey(uq => uq.QuestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
