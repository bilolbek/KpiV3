using KpiV3.Domain.Specialties.DataContracts;
using MediatR;

namespace KpiV3.Domain.Specialties.Commands;

public record UpdateSpecialtyCommand : IRequest<Specialty>
{
    public Guid SpecialtyId { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; set; }
}

public class UpdateSpecialtyCommandHandler : IRequestHandler<UpdateSpecialtyCommand, Specialty>
{
    private readonly KpiContext _db;

    public UpdateSpecialtyCommandHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Specialty> Handle(UpdateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = await _db.Specialties
            .FindAsync(new object?[] { request.SpecialtyId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        specialty.Name = request.Name;
        specialty.Description = request.Description;

        await _db.SaveChangesAsync(cancellationToken);

        return specialty;
    }
}
