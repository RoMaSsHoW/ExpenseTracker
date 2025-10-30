using ExpenseTracker.Application.Common.Persistence;
using ExpenseTracker.Domain.AccountAggregate;
using ExpenseTracker.Domain.AccountAggregate.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IAppDbContext _dbContext;

    public UserRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email.Address == email);

        return user;
    }

    public async Task<User?> FindByRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentNullException(nameof(refreshToken), "Refresh token cannot be null or empty");

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.RefreshToken.Token == refreshToken);

        return user;
    }

    public async Task<User?> FindById(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentNullException(nameof(userId), "UserId cannot be null or empty");

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user;
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }
}