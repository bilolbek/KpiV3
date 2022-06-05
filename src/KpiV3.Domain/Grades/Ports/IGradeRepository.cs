using KpiV3.Domain.Grades.DataContracts;

namespace KpiV3.Domain.Grades.Ports;

public interface IGradeRepository
{
    Task<Result<IError>> InsertAsync(Grade grade);
    Task<Result<IError>> UpdateAsync(Grade grade);
    Task<Result<IError>> DeleteAsync(Guid employeeId, Guid requirementId);
}
