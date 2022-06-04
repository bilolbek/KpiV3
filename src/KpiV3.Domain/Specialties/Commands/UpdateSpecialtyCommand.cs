using KpiV3.Domain.Specialties.DataContracts;
using KpiV3.Domain.Specialties.Ports;
using MediatR;

namespace KpiV3.Domain.Specialties.Commands;

public record UpdateSpecialtyCommand : IRequest<Result<Specialty, IError>>
{
    public Guid SpecialtyId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid PositionId { get; set; }
}

public class UpdateSpecialtyCommandHandler : IRequestHandler<UpdateSpecialtyCommand, Result<Specialty, IError>>
{
    private readonly ISpecialtyRepository _repository;

    public UpdateSpecialtyCommandHandler(ISpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Specialty, IError>> Handle(UpdateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = new Specialty
        {
            Id = request.SpecialtyId,
            Name = request.Name,
            Description = request.Description,
            PositionId = request.PositionId
        };

        return await _repository
            .UpdateAsync(specialty)
            .InsertSuccessAsync(() => specialty);
    }
}
