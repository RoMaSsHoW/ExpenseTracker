using ExpenseTracker.Application.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace ExpenseTracker.Infrastructure.Jobs;

public class AutoTransactionJob : IJob
{
    private readonly IAppDbContext _dbContext;
    
    public AutoTransactionJob(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var accounts = _dbContext.Accounts
            .Include(x => x.RecurringRules)
            .ToList();

        foreach (var account in accounts)
        {
            var rules = account.RecurringRules.ToList();
            foreach (var rule in rules)
            {
                if (!rule.IsActive) 
                    continue;

                if (rule.NextRunAt.Date <= DateTime.UtcNow.Date)
                {
                    try
                    {
                        rule.CreateAutoTransaction();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}