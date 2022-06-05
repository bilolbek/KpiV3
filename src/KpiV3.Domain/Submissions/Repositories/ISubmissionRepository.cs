using KpiV3.Domain.Submissions.DataContracts;

namespace KpiV3.Domain.Submissions.Repositories;

public interface ISubmissionRepository
{
    Task<Result<Submission, IError>> FindByIdAsync(Guid submissionId);
    Task<Result<IError>> InsertAsync(Submission submission);
    Task<Result<IError>> UpdateAsync(Submission submission);
    Task<Result<IError>> DeleteAsync(Guid submissionId);
}
