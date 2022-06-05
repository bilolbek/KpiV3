using KpiV3.Domain.Grades.Ports;
using MediatR;

namespace KpiV3.Domain.Grades.Commands;

public record DeleteGradeCommand : IRequest<Result<IError>>
{
    public Guid RequirementId { get; set; }
    public Guid EmployeeId { get; set; }
}

public class DeleteGradeCommandHandler : IRequestHandler<DeleteGradeCommand, Result<IError>>
{
    private readonly IGradeRepository _repository;

    public DeleteGradeCommandHandler(IGradeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IError>> Handle(DeleteGradeCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.EmployeeId, request.RequirementId);
    }
}
