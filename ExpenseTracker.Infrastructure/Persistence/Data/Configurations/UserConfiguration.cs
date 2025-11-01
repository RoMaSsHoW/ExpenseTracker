using ExpenseTracker.Domain.ProfileAggregate;
using ExpenseTracker.Domain.ProfileAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .IsRequired();
        
        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .IsRequired();
        
        builder.Property(u => u.LastName)
            .HasColumnName("last_name")
            .IsRequired();

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Address)
                .HasColumnName("email_address")
                .IsRequired();

            email.Property(e => e.IsConfirmed)
                .HasColumnName("email_is_confirmed")
                .HasDefaultValue(false);
            
            email.HasIndex(e => e.Address).IsUnique();

            email.WithOwner();
        });

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(p => p.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();
            
            password.WithOwner();
        });
        
        builder.Property(u => u.Role)
            .HasConversion(
                r => r.Name,
                name => Enumeration.FromName<Role>(name))
            .HasColumnName("role")
            .IsRequired();

        builder.OwnsOne(u => u.RefreshToken, refreshToken =>
        {
            refreshToken.Property(rt => rt.Token)
                .HasColumnName("refresh_token")
                .IsRequired();

            refreshToken.Property(rt => rt.ExpireDate)
                .HasColumnName("refresh_token_expiry_date")
                .IsRequired();

            refreshToken.WithOwner();
        });

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.HasMany(u => u.Accounts)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}