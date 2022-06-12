using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Employees.Services;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public class CreateEmployeeCommand : IRequest
{
    public string Email { get; init; } = default!;

    public Name Name { get; init; } = default!;

    public Guid PositionId { get; set; }
}

public class CreateEmployeeCommandHandler : AsyncRequestHandler<CreateEmployeeCommand>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;
    private readonly IDateProvider _dateProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly EmployeeWelcomeService _employeeWelcomeService;

    public CreateEmployeeCommandHandler(
        KpiContext db,
        IGuidProvider guidProvider,
        IDateProvider dateProvider,
        IPasswordHasher passwordHasher,
        IPasswordGenerator passwordGenerator,
        EmployeeWelcomeService employeeWelcomeService)
    {
        _db = db;
        _guidProvider = guidProvider;
        _dateProvider = dateProvider;
        _passwordHasher = passwordHasher;
        _passwordGenerator = passwordGenerator;
        _employeeWelcomeService = employeeWelcomeService;
    }

    protected override async Task Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var password = _passwordGenerator.Generate();

        var employee = await CreateEmployeeAsync(request, password, cancellationToken);

        await _employeeWelcomeService.SendWelcomeMessageAsync(employee, password, cancellationToken);
    }

    private async Task<Employee> CreateEmployeeAsync(
        CreateEmployeeCommand request,
        string password,
        CancellationToken cancellationToken)
    {
        var employee = new Employee
        {
            Id = _guidProvider.New(),
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(password),
            Name = request.Name,
            PositionId = request.PositionId,
            IsBlocked = false,
            RegisteredDate = _dateProvider.Now(),
        };

        _db.Employees.Add(employee);

        await _db.SaveChangesAsync(cancellationToken);

        return employee;
    }
}
