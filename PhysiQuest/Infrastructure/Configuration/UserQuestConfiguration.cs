using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class UserQuestConfiguration : IEntityTypeConfiguration<UserQuest>
    {
        public void Configure(EntityTypeBuilder<UserQuest> builder)
        {
            builder.ToTable("UserQuests");

            builder.HasKey(x => new { x.UserId, x.QuestId });

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserQuests)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Quest)
                .WithMany(x => x.UserQuests)
                .HasForeignKey(x => x.QuestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.ProofImage)
                .HasColumnType("nvarchar(max)");
        }
    }

}
