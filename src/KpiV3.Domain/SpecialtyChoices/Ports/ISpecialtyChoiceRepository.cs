using KpiV3.Domain.SpecialtyChoices.DataContracts;

namespace KpiV3.Domain.SpecialtyChoices.Ports;

public interface ISpecialtyChoiceRepository
{
    Task<Result<SpecialtyChoice, IError>> FindByEmployeeIdAndPeriodIdAsync(
        Guid employeeId,
        Guid periodId);

    Task<Result<IError>> InsertAsync(SpecialtyChoice choice);
    
    Task<Result<IError>> UpdateAsync(SpecialtyChoice choice);
}
