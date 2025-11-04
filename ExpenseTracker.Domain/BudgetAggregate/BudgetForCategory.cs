using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.BudgetAggregate;

public class BudgetForCategory : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid CategoryId { get; private set; }
    
}