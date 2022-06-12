using KpiV3.Domain.Common.DataContracts;
using MediatR;

namespace KpiV3.Domain.Employees.Queries;

public record GetProfileQuery : IRequest<Profile>
{
    public Guid EmployeeId { get; init; }
}

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Profile>
{
    private readonly KpiContext _db;

    public GetProfileQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Profile> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        return await _db.Employees
           .Where(e => e.Id == request.EmployeeId)
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
           .FirstOrDefaultAsync(cancellationToken)
           .EnsureFoundAsync();
    }
}
