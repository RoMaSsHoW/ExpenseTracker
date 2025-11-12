namespace ExpenseTracker.Domain.SeedWork;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    public Guid Id { get; protected init; } = Guid.NewGuid();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;
    
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (Entity)obj;

        if (Id == Guid.Empty || other.Id == Guid.Empty)
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);
    
    public void ClearDomainEvents()
        => _domainEvents.Clear();
    
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));
        _domainEvents.Add(domainEvent);
    }
}