using KpiV3.Domain.Requirements.DataContracts;
using KpiV3.Domain.Requirements.Queries;
using KpiV3.Domain.Specialties.Queries;
using MediatR;

namespace KpiV3.Infrastructure.Requirements.QueryHandlers;

internal class GetRequirementsOfEmployeeQueryHandler : IRequestHandler<GetRequirementsOfEmployeeQuery, Result<List<Requirement>, IError>>
{
    private readonly IMediator _mediator;

    public GetRequirementsOfEmployeeQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<List<Requirement>, IError>> Handle(GetRequirementsOfEmployeeQuery request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetChoosenSpecialtyQuery
        {
            EmployeeId = request.EmployeeId,
            PeriodId = request.PeriodId,
        }).BindAsync(specialty => _mediator.Send(new GetRequirementsQuery
        {
            SpecialtyId = specialty.Id,
            PeriodId = request.PeriodId,
        }));
    }
}
