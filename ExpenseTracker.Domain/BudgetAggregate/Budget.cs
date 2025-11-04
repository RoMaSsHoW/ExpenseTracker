using ExpenseTracker.Domain.SeedWork;

namespace ExpenseTracker.Domain.BudgetAggregate;

public class Budget : Entity
{
    
    public string Name { get; private set; }
    public string Description { get; private set; }
        
}