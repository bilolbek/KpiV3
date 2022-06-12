using KpiV3.Domain.Common.DataContracts;
using MediatR;

namespace KpiV3.Domain.Employees.Queries;

public record GetEmployeesQuery : IRequest<Page<Profile>>
{
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? MiddleName { get; init; }
    public Guid? PositionId { get; init; }
    public string? PositionName { get; init; }

    public Pagination Pagination { get; init; }
}

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Page<Profile>>
{
    private readonly KpiContext _db;

    public GetEmployeesQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Page<Profile>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Employees.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            query = query.Where(e => e.Email.Contains(request.Email));
        }

        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            query = query.Where(e => e.Name.FirstName.Contains(request.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            query = query.Where(e => e.Name.LastName.Contains(request.LastName));
        }

        if (!string.IsNullOrWhiteSpace(request.MiddleName))
        {
            query = query.Where(e => e.Name.MiddleName!.Contains(request.MiddleName));
        }

        if (!string.IsNullOrWhiteSpace(request.PositionName))
        {
            query = query.Where(e => e.Position.Name.Contains(request.PositionName));
        }

        if (request.PositionId.HasValue)
        {
            query = query.Where(e => e.PositionId == request.PositionId);
        }

        return await query
            .Select(e => new Profile
            {
                Id = e.Id,
                Email = e.Email,
                Name = new()
                {
                    FirstName = e.Name.FirstName,
                    LastName = e.Name.LastName,
                    MiddleName = e.Name.MiddleName,
                },
                AvatarId = e.AvatarId,
            })
            .ToPageAsync(request.Pagination, cancellationToken);
    }
}
