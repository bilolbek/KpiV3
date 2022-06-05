using KpiV3.Domain.Common;
using KpiV3.Domain.Grades.DataContracts;
using KpiV3.Domain.Grades.Ports;
using KpiV3.Domain.Requirements.Ports;
using MediatR;

namespace KpiV3.Domain.Grades.Commands;

public record UpdateGradeCommand : IRequest<Result<Grade, IError>>
{
    public Guid EmployeeId { get; set; }
    public Guid RequirementId { get; set; }
    public double Value { get; set; }
}

public class UpdateGradeCommandHandler : IRequestHandler<UpdateGradeCommand, Result<Grade, IError>>
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IRequirementRepository _requirementRepository;
    private readonly ITransactionProvider _transactionProvider;

    public UpdateGradeCommandHandler(
        IGradeRepository gradeRepository,
        IRequirementRepository requirementRepository,
        ITransactionProvider transactionProvider)
    {
        _gradeRepository = gradeRepository;
        _requirementRepository = requirementRepository;
        _transactionProvider = transactionProvider;
    }

    public async Task<Result<Grade, IError>> Handle(UpdateGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = new Grade
        {
            EmployeeId = request.EmployeeId,
            RequirementId = request.RequirementId,
            Value = request.Value,
        };

        return await _transactionProvider
            .RunAsync(() => EnsureThanValueDoesNotExceedWeight(request)
                .BindAsync(() => _gradeRepository.UpdateAsync(grade)))
            .InsertSuccessAsync(() => grade);
    }

    private async Task<Result<IError>> EnsureThanValueDoesNotExceedWeight(UpdateGradeCommand request)
    {
        return await _requirementRepository
            .FindByIdAsync(request.RequirementId)
            .BindAsync(requirement => request.Value > requirement.Weight ?
                Result<IError>.Fail(new BusinessRuleViolation("Value cannot exceed weight")) :
                Result<IError>.Ok());
    }
}
