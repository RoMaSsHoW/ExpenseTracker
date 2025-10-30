using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Data;

public class Seeder
{
    private readonly AppDbContext  _dbContext;
    
    public Seeder(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task RunAsync()
    {
        await SeedUsers();
    }

    private async Task SeedUsers()
    {
        if (await _dbContext.Users.AnyAsync()) 
            return;

        var admin = User.Registration("admin", "admin", "admin@gmail.com", "admin123", Role.Admin.Id, "123456789", DateTime.UtcNow.AddHours(1));
        
        await _dbContext.Users.AddAsync(admin);
        await _dbContext.SaveChangesAsync();
    }
}