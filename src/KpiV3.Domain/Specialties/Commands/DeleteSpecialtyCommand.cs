using MediatR;

namespace KpiV3.Domain.Specialties.Commands;

public record DeleteSpecialtyCommand : IRequest
{
    public Guid SpecialtyId { get; set; }
}

public class DeleteSpecialtyCommandHandler : AsyncRequestHandler<DeleteSpecialtyCommand>
{
    private readonly KpiContext _db;

    public DeleteSpecialtyCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = await _db.Specialties
            .FindAsync(new object?[] { request.SpecialtyId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        _db.Specialties.Remove(specialty);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
