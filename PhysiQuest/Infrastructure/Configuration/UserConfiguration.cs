using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Points)
                .HasDefaultValue(0);

            builder.Property(u => u.Tokens)
                .HasDefaultValue(100);

            builder.HasMany(u => u.UserQuests)
                .WithOne(uq => uq.User)
                .HasForeignKey(uq => uq.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserBadges)
                .WithOne(ub => ub.User)
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
