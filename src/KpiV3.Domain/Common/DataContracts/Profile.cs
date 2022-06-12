namespace KpiV3.Domain.Common.DataContracts;

public record Profile
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public Name Name { get; set; } = default!;
    public Guid? AvatarId { get; set; }
}
