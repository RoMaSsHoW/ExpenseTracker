using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .IsRequired();
        builder.Property(t => t.Name)
            .HasColumnName("name")
            .IsRequired();
        builder.Property(t => t.Description)
            .HasColumnName("description");
        builder.Property(t => t.Amount)
            .HasColumnName("amount")
            .IsRequired();
        builder.Property(t => t.Currency)
            .HasConversion(
                c => c.Name,
                name => Enumeration.FromName<Currency>(name))
            .HasColumnName("currency")
            .IsRequired();
        builder.Property(t => t.CategoryId)
            .HasColumnName("category_id");
        builder.Property(t => t.Type)
            .HasConversion(
                t => t.Name,
                name => Enumeration.FromName<TransactionType>(name))
            .HasColumnName("type")
            .IsRequired();
        builder.Property(t => t.Source)
            .HasConversion(
                s => s.Name,
                name => Enumeration.FromName<TransactionSource>(name))
            .HasColumnName("source")
            .IsRequired();
        builder.Property(t => t.AccountId)
            .HasColumnName("account_id")
            .IsRequired();
        builder.Property(t => t.Date)
            .HasColumnName("date")
            .IsRequired();
        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId);
        builder.HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}