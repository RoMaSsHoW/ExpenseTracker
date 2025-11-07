using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .IsRequired();
        builder.Property(a => a.Name)
            .HasColumnName("name")
            .IsRequired();
        builder.Property(a => a.Balance)
            .HasColumnName("balance")
            .IsRequired();
        builder.Property(a => a.Currency)
            .HasConversion(
                c => c.Name,
                name => Enumeration.FromName<Currency>(name))
            .HasColumnName("currency")
            .IsRequired();
        builder.Property(a => a.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        builder.Property(a => a.IsDefault)
            .HasColumnName("is_default")
            .HasDefaultValue(false)
            .IsRequired();
        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.HasMany(a => a.Transactions)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(a => a.RecurringRules)
            .WithOne(r => r.Account)
            .HasForeignKey(r => r.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}