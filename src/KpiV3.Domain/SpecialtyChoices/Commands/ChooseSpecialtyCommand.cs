using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Periods.Ports;
using KpiV3.Domain.Specialties.Ports;
using KpiV3.Domain.SpecialtyChoices.DataContracts;
using KpiV3.Domain.SpecialtyChoices.Ports;
using MediatR;

namespace KpiV3.Domain.SpecialtyChoices.Commands;

public record ChooseSpecialtyCommand : IRequest<Result<IError>>
{
    public Guid EmployeeId { get; set; }
    public Guid SpecialtyId { get; set; }
    public Guid PeriodId { get; set; }
}

public class ChooseSpecialtyCommandHandler : IRequestHandler<ChooseSpecialtyCommand, Result<IError>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly ISpecialtyChoiceRepository _specialtyChoiceRepository;

    public ChooseSpecialtyCommandHandler(
        IEmployeeRepository employeeRepository,
        ISpecialtyRepository specialtyRepository,
        ISpecialtyChoiceRepository specialtyChoiceRepository)
    {
        _employeeRepository = employeeRepository;
        _specialtyRepository = specialtyRepository;
        _specialtyChoiceRepository = specialtyChoiceRepository;
    }

    public async Task<Result<IError>> Handle(ChooseSpecialtyCommand request, CancellationToken cancellationToken)
    {
        return await _employeeRepository
            .FindByIdAsync(request.EmployeeId)
            .BindAsync(employee => EnsureCanChooseSpecialty(employee.Id, request.PeriodId)
                .BindAsync(() => EnsureSpecialtyIsCorrect(employee.PositionId, request.SpecialtyId)))
            .BindAsync(() => ChooseSpecialty(request));
    }

    private async Task<Result<IError>> ChooseSpecialty(ChooseSpecialtyCommand command)
    {
        return await _specialtyChoiceRepository
            .FindByEmployeeIdAndPeriodIdAsync(command.EmployeeId, command.PeriodId)
            .BindAsync(choice => _specialtyChoiceRepository.UpdateAsync(choice with { SpecialtyId = command.SpecialtyId }))
            .BindFailureAsync(async error => error is NoEntity ?
                await _specialtyChoiceRepository.InsertAsync(new SpecialtyChoice
                {
                    PeriodId = command.PeriodId,
                    EmployeeId = command.EmployeeId,
                    SpecialtyId = command.SpecialtyId,
                    CanBeChanged = false,
                }) : Result<IError>.Fail(error));
    }

    private async Task<Result<IError>> EnsureCanChooseSpecialty(Guid employeeId, Guid periodId)
    {
        return await _specialtyChoiceRepository
            .FindByEmployeeIdAndPeriodIdAsync(employeeId, periodId)
            .BindAsync(choice => choice.CanBeChanged ?
                Result<IError>.Ok() :
                Result<IError>.Fail(new BusinessRuleViolation("You are not allowed to change your current specialty")))
            .BindFailureAsync(failure => failure is NoEntity ?
                Result<IError>.Ok() :
                Result<IError>.Fail(failure));
    }

    private async Task<Result<IError>> EnsureSpecialtyIsCorrect(Guid positionId, Guid specialtyId)
    {
        return await _specialtyRepository
            .FindByPositionIdAsync(positionId)
            .BindAsync(allowedSpecialties => allowedSpecialties.Any(s => s.Id == specialtyId) ?
                Result<IError>.Ok() :
                Result<IError>.Fail(new BusinessRuleViolation("This specialty is not allowed on your position")));
    }
}
