using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .HasColumnName("category_id")
            .ValueGeneratedNever()
            .IsRequired();
        builder.Property(c => c.Name)
            .HasColumnName("name")
            .IsRequired();
        builder.Property(t => t.Type)
            .HasConversion(
                t => t.Name,
                name => Enumeration.FromName<TransactionType>(name))
            .HasColumnName("type")
            .IsRequired();
        builder.Property(c => c.UserId)
            .HasColumnName("user_id");
        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        builder.Property(c => c.IsDefault)
            .HasColumnName("is_default")
            .IsRequired();

        builder.HasMany(c => c.Transactions)
            .WithOne(t => t.Category)
            .HasForeignKey(t => t.CategoryId);
    }
}