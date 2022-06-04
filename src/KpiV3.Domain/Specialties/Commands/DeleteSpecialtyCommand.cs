using KpiV3.Domain.Specialties.Ports;
using MediatR;

namespace KpiV3.Domain.Specialties.Commands;

public record DeleteSpecialtyCommand : IRequest<Result<IError>>
{
    public Guid SpecialtyId { get; set; }
}

public record DeleteSpecialtyCommandHandler : IRequestHandler<DeleteSpecialtyCommand, Result<IError>>
{
    private readonly ISpecialtyRepository _repository;

    public DeleteSpecialtyCommandHandler(ISpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IError>> Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.SpecialtyId);
    }
}
