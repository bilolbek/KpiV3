using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.WebApi.Authentication.Services;

public interface IEmployeeAccessor
{
    Guid EmployeeId { get; }

    Guid PositionId { get; }
    string PositionName { get; }
    PositionType PositionType { get; }
}
