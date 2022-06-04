using KpiV3.Domain.Specialties.DataContracts;
using KpiV3.Domain.Specialties.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Specialties.Data;
using MediatR;

namespace KpiV3.Infrastructure.Specialties.QueryHandlers;

internal class GetSpecialtyQueryHandler : IRequestHandler<GetSpecialtyQuery, Result<Specialty, IError>>
{
    private readonly Database _db;

    public GetSpecialtyQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Specialty, IError>> Handle(GetSpecialtyQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT * FROM specialties
WHERE id = @SpecialtyId";

        return await _db
            .QueryFirstAsync<SpecialtyRow>(new(sql, new { request.SpecialtyId }))
            .MapAsync(row => row.ToModel());
    }
}
