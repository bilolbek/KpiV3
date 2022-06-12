using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Employees.Services;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Specialties.DataContracts;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record ImportEmployeesCommand : IRequest
{
    public List<ImportedEmployee> Employees { get; init; } = default!;
}

public class ImportedEmployeesCommandHandler : AsyncRequestHandler<ImportEmployeesCommand>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;
    private readonly IDateProvider _dateProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly EmployeeWelcomeService _employeeWelcomeService;

    public ImportedEmployeesCommandHandler(
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

    protected override async Task Handle(ImportEmployeesCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        var positions = await GetPositionIdsDictionaryAsync(request, cancellationToken);

        var passwords = new Dictionary<Guid, string>();
        var employees = request.Employees
            .Select(e => (id: _guidProvider.New(), employee: e))
            .Select(x => new Employee
            {
                Id = x.id,
                Email = x.employee.Email,
                Name = new()
                {
                    FirstName = x.employee.Name.FirstName,
                    LastName = x.employee.Name.LastName,
                    MiddleName = x.employee.Name.MiddleName,
                },
                PasswordHash = _passwordHasher.Hash(passwords[x.id] = _passwordGenerator.Generate()),
                PositionId = positions[x.employee.Position],
                RegisteredDate = _dateProvider.Now(),
            })
            .ToList();

        _db.Employees.AddRange(employees);

        await _db.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        await WelcomeImportedEmployeesAsync(passwords, employees, cancellationToken);
    }

    private async Task WelcomeImportedEmployeesAsync(Dictionary<Guid, string> passwords, List<Employee> employees, CancellationToken cancellationToken)
    {
        foreach (var employee in employees)
        {
            await _employeeWelcomeService.SendWelcomeMessageAsync(employee, passwords[employee.Id], cancellationToken);
        }
    }

    private async Task<Dictionary<string, Guid>> GetPositionIdsDictionaryAsync(
        ImportEmployeesCommand request,
        CancellationToken cancellationToken)
    {
        var requiredPositions = request.Employees
            .Select(e => e.Position)
            .ToHashSet();

        var existingPositions = await _db.Positions
            .Where(p => requiredPositions.Contains(p.Name))
            .ToListAsync(cancellationToken);

        var positions = existingPositions
            .ToDictionary(p => p.Name, p => p.Id);

        if (requiredPositions.Count != existingPositions.Count)
        {
            var positionsToCreate = requiredPositions
                .Where(p => !existingPositions.Any(e => e.Name == p))
                .Select(p => new Position
                {
                    Id = _guidProvider.New(),
                    Name = p,
                    Type = PositionType.Employee,
                    Specialties = new List<Specialty>(),
                })
                .ToList();

            _db.Positions.AddRange(positionsToCreate);

            positionsToCreate.ForEach(p => positions[p.Name] = p.Id);

            await _db.SaveChangesAsync(cancellationToken);
        }

        return positions;
    }
}
