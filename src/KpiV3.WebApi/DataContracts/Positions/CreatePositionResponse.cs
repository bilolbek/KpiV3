using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.WebApi.DataContracts.Positions;

public class CreatePositionResponse
{
    public CreatePositionResponse()
    {
    }

    public CreatePositionResponse(Position position)
    {
        Id = position.Id;
        Name = position.Name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
