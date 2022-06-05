using KpiV3.Domain.Submissions.DataContracts;
using KpiV3.Domain.Submissions.Repositories;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Submissions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Infrastructure.Submissions.Repositories;

internal class SubmissionRepository : ISubmissionRepository
{
    private readonly Database _db;

    public SubmissionRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<Submission, IError>> FindByIdAsync(Guid submissionId)
    {
        const string sql = @"
SELECT * FROM submissions
WHERE id = @submissionId";

        return await _db
            .QueryFirstAsync<SubmissionRow>(new(sql, new { submissionId }))
            .MapAsync(row => row.ToModel());
    }

    public async Task<Result<IError>> InsertAsync(Submission submission)
    {
        const string sql = @"
INSERT INTO submissions (id, requirement_id, file_id, uploader_id, note, submission_date, status)
VALUES (@Id, @RequirementId, @FileId, @UploaderId, @Note, @SubmissionDate, @Status)";

        return await _db.ExecuteAsync(new(sql, new SubmissionRow(submission)));
    }

    public async Task<Result<IError>> UpdateAsync(Submission submission)
    {
        const string sql = @"
UPDATE submissions SET
    requirement_id = @RequirementId,
    file_id = @FileId,
    uploader_id = @UploaderId,
    note = @Note,
    submission_date = @SubmissionDate,
    status = @Status
WHERE id = @Id";

        return await _db.ExecuteRequiredChangeAsync<Submission>(new(sql, new SubmissionRow(submission)));
    }

    public async Task<Result<IError>> DeleteAsync(Guid submissionId)
    {
        const string sql = @"DELETE FROM submissions WHERE id = @submissionId";

        return await _db.ExecuteRequiredChangeAsync<Submission>(new(sql, new { submissionId }));
    }
}
