using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Identity.Entities;

namespace Restaurant.Identity.Data.Configurations;

public sealed class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(u => u.RefreshToken)
            .WithOne()
            .HasForeignKey<RefreshToken>(t => t.UserId);
    }
}