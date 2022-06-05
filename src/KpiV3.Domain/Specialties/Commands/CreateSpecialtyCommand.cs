using KpiV3.Domain.Common;
using KpiV3.Domain.Specialties.DataContracts;
using KpiV3.Domain.Specialties.Ports;
using MediatR;

namespace KpiV3.Domain.Specialties.Commands;

public record CreateSpecialtyCommand : IRequest<Result<Specialty, IError>>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid PositionId { get; set; }
}

public class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, Result<Specialty, IError>>
{
    private readonly ISpecialtyRepository _repository;
    private readonly IGuidProvider _guidProvider;

    public CreateSpecialtyCommandHandler(
        ISpecialtyRepository repository, 
        IGuidProvider guidProvider)
    {
        _repository = repository;
        _guidProvider = guidProvider;
    }

    public async Task<Result<Specialty, IError>> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = new Specialty
        {
            Id = _guidProvider.New(),
            Name = request.Name,
            Description = request.Description,
            PositionId = request.PositionId,
        };

        return await _repository
            .InsertAsync(specialty)
            .InsertSuccessAsync(() => specialty);
    }
}
