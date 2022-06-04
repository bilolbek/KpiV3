using KpiV3.Domain.Specialties.DataContracts;
using KpiV3.Domain.Specialties.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Specialties.Data;
using MediatR;

namespace KpiV3.Infrastructure.Specialties.QueryHandlers;

internal class GetSpecialtiesQueryHandler : IRequestHandler<GetSpecialtiesQuery, Result<List<Specialty>, IError>>
{
    private readonly Database _db;

    public GetSpecialtiesQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<List<Specialty>, IError>> Handle(GetSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT * FROM specialties 
WHERE position_id = @PositionId";

        return await _db
            .QueryAsync<SpecialtyRow>(new(sql, new { request.PositionId }))
            .MapAsync(rows => rows.Select(row => row.ToModel()).ToList());
    }
}
