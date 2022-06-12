using KpiV3.Domain.SpecialtyChoices.Commands;

namespace KpiV3.WebApi.DataContracts.Employees;

public class AllowSpecialtyChangeRequest
{
    public Guid EmployeeId { get; set; }
    public Guid PeriodId { get; set; }

    public AllowSpecialtyChangeCommand ToCommand()
    {
        return new AllowSpecialtyChangeCommand
        {
            EmployeeId = EmployeeId,
            PeriodId = PeriodId
        };
    }
}
