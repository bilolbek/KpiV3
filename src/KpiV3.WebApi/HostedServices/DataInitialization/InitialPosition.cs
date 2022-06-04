using KpiV3.Domain.Positions.Commands;
using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.WebApi.HostedServices.DataInitialization;

public class InitialPosition
{
    public string Name { get; set; } = default!;
    public PositionType Type { get; set; }

    public CreatePositionCommand ToCreateCommand()
    {
        return new CreatePositionCommand { Name = Name, Type = Type };
    }
}
