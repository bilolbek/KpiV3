using KpiV3.Domain.Requirements.DataContracts;

namespace KpiV3.Domain.Requirements.Ports;

public interface IRequirementRepository
{
    Task<Result<Requirement, IError>> FindByIdAsync(Guid requirementId);
    Task<Result<List<Requirement>, IError>> FindBySpecialtyIdAndPeriodPartIdAsync(
        Guid specialtyId,
        Guid periodPartId);
    Task<Result<IError>> InsertAsync(Requirement requirement);
    Task<Result<IError>> UpdateAsync(Requirement requirement);
    Task<Result<IError>> DeleteAsync(Guid requirementId);
}
