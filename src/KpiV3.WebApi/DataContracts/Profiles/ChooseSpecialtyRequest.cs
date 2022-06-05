using KpiV3.Domain.SpecialtyChoices.Commands;

namespace KpiV3.WebApi.DataContracts.Profiles;

public record ChooseSpecialtyRequest
{
    public Guid SpecialtyId { get; set; }
    public Guid PeriodId { get; set; }

    public ChooseSpecialtyCommand ToCommand(Guid employeeId)
    {
        return new ChooseSpecialtyCommand
        {
            EmployeeId = employeeId,
            SpecialtyId = SpecialtyId,
            PeriodId = PeriodId,
        };
    }
}
