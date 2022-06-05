using KpiV3.Domain.SpecialtyChoices.Ports;
using MediatR;

namespace KpiV3.Domain.SpecialtyChoices.Commands;

public record AllowSpecialityChangeCommand : IRequest<Result<IError>>
{
    public Guid EmployeeId { get; set; }
    public Guid PeriodId { get; set; }
}

public class AllowSpecialityChangeCommandHandler : IRequestHandler<AllowSpecialityChangeCommand, Result<IError>>
{
    private readonly ISpecialtyChoiceRepository _specialtyChoiceRepository;

    public AllowSpecialityChangeCommandHandler(ISpecialtyChoiceRepository specialtyChoiceRepository)
    {
        _specialtyChoiceRepository = specialtyChoiceRepository;
    }

    public async Task<Result<IError>> Handle(AllowSpecialityChangeCommand request, CancellationToken cancellationToken)
    {
        return await _specialtyChoiceRepository
            .FindByEmployeeIdAndPeriodIdAsync(request.EmployeeId, request.PeriodId)
            .BindAsync(choice => _specialtyChoiceRepository.UpdateAsync(choice with { CanBeChanged = true }));
    }
}
