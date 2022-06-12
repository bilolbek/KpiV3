namespace KpiV3.Domain.Common.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type entityType) : this(entityType, $"{entityType.Name} not found")
    {
    }

    public EntityNotFoundException(Type entityType, string? message) : base(message)
    {
        EntityType = entityType;
    }

    public Type EntityType { get; }
}
