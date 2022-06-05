using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.Ports;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record UpdateProfileCommand : IRequest<Result<IError>>
{
    public Guid EmployeeId { get; set; }
    public Name Name { get; set; }
    public Guid? AvatarId { get; set; }
}

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<IError>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateProfileCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<IError>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        return await _employeeRepository
            .FindByIdAsync(request.EmployeeId)
            .MapAsync(employee => employee with { Name = request.Name, AvatarId = request.AvatarId })
            .BindAsync(employee => _employeeRepository.UpdateAsync(employee));
    }
}
