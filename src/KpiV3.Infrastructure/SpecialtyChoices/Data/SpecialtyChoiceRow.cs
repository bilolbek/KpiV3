using KpiV3.Domain.SpecialtyChoices.DataContracts;

namespace KpiV3.Infrastructure.SpecialtyChoices.Data;

internal class SpecialtyChoiceRow
{
    public SpecialtyChoiceRow()
    {
    }

    public SpecialtyChoiceRow(SpecialtyChoice choice)
    {
        SpecialtyId = choice.SpecialtyId;
        PeriodId = choice.PeriodId;
        EmployeeId = choice.EmployeeId;
        CanBeChanged = choice.CanBeChanged;
    }

    public Guid SpecialtyId { get; set; }
    public Guid PeriodId { get; set; }
    public Guid EmployeeId { get; set; }
    public bool CanBeChanged { get; set; }

    public SpecialtyChoice ToModel()
    {
        return new SpecialtyChoice
        {
            SpecialtyId = SpecialtyId,
            PeriodId = PeriodId,
            EmployeeId = EmployeeId,
            CanBeChanged = CanBeChanged
        };
    }
}
