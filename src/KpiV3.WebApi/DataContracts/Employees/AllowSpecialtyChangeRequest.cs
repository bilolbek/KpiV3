using KpiV3.Domain.SpecialtyChoices.Commands;

namespace KpiV3.WebApi.DataContracts.Employees;

public class AllowSpecialtyChangeRequest
{
    public Guid EmployeeId { get; set; }
    public Guid PeriodId { get; set; }

    public AllowSpecialityChangeCommand ToCommand()
    {
        return new AllowSpecialityChangeCommand
        {
            EmployeeId = EmployeeId,
            PeriodId = PeriodId
        };
    }
}
