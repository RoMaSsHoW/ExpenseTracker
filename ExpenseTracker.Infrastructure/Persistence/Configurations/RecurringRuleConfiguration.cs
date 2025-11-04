using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations;

public class RecurringRuleConfiguration : IEntityTypeConfiguration<RecurringRule>
{
    public void Configure(EntityTypeBuilder<RecurringRule> builder)
    {
        builder.ToTable("recurring_rules");
        builder.HasKey(rr => rr.Id);
        
        builder.Property(rr => rr.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .IsRequired();
        builder.Property(rr => rr.Name)
            .HasColumnName("name")
            .IsRequired();
        builder.Property(rr => rr.Amount)
            .HasColumnName("amount")
            .IsRequired();
        builder.Property(rr => rr.Currency)
            .HasConversion(
                c => c.Name,
                name => Enumeration.FromName<Currency>(name))
            .HasColumnName("currency")
            .IsRequired();
        builder.Property(rr => rr.CategoryId)
            .HasColumnName("category_id");
        builder.Property(rr => rr.Type)
            .HasConversion(
                t => t.Name,
                name => Enumeration.FromName<TransactionType>(name))
            .HasColumnName("type")
            .IsRequired();
        builder.Property(rr => rr.Frequency)
            .HasConversion(
                f => f.Name,
                name => Enumeration.FromName<RecurringFrequency>(name))
            .HasColumnName("frequency")
            .IsRequired();
        builder.Property(rr => rr.AccountId)
            .HasColumnName("account_id")
            .IsRequired();
        builder.Property(rr => rr.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();
        builder.Property(rr => rr.NextRunAt)
            .HasColumnName("next_run_at")
            .IsRequired();
        builder.Property(rr => rr.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasOne(rr => rr.Account)
            .WithMany(a => a.RecurringRules)
            .HasForeignKey(r => r.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}