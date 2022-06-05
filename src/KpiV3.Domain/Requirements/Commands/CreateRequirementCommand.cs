using KpiV3.Domain.Common;
using KpiV3.Domain.Requirements.DataContracts;
using KpiV3.Domain.Requirements.Ports;
using MediatR;

namespace KpiV3.Domain.Requirements.Commands;

public record CreateRequirementCommand : IRequest<Result<Requirement, IError>>
{
    public Guid PeriodPartId { get; set; }
    public Guid SpecialtyId { get; set; }
    public Guid IndicatorId { get; set; }
    public string? Note { get; set; }
    public double Weight { get; set; }
}

public class CreateRequirementCommandHandler : IRequestHandler<CreateRequirementCommand, Result<Requirement, IError>>
{
    private readonly IRequirementRepository _requirementRepository;
    private readonly IGuidProvider _guidProvider;
    private readonly ITransactionProvider _transactionProvider;

    public CreateRequirementCommandHandler(
        IRequirementRepository requirementRepository,
        IGuidProvider guidProvider,
        ITransactionProvider transactionProvider)
    {
        _requirementRepository = requirementRepository;
        _guidProvider = guidProvider;
        _transactionProvider = transactionProvider;
    }

    public async Task<Result<Requirement, IError>> Handle(CreateRequirementCommand request, CancellationToken cancellationToken)
    {
        var requirement = new Requirement
        {
            Id = _guidProvider.New(),
            IndicatorId = request.IndicatorId,
            PeriodPartId = request.PeriodPartId,
            SpecialtyId = request.SpecialtyId,
            Weight = request.Weight,
            Note = request.Note,
        };

        return await _transactionProvider
            .RunAsync(() => EnsureTotalWeightIsNotGreaterThan100(request)
                .BindAsync(() => _requirementRepository.InsertAsync(requirement)))
            .InsertSuccessAsync(() => requirement);
    }

    private async Task<Result<IError>> EnsureTotalWeightIsNotGreaterThan100(CreateRequirementCommand request)
    {
        return await CalculcateTotalWeight(request)
            .BindAsync(total => total + request.Weight > 100
                ? Result<IError>.Fail(new BusinessRuleViolation("Total weight is greater than 100"))
                : Result<IError>.Ok());
    }

    private async Task<Result<double, IError>> CalculcateTotalWeight(CreateRequirementCommand request)
    {
        return await _requirementRepository
            .FindBySpecialtyIdAndPeriodPartIdAsync(request.SpecialtyId, request.PeriodPartId)
            .MapAsync(requirements => requirements.Sum(r => r.Weight));
    }
}
