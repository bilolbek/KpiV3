using KpiV3.Domain.Comments.DataContracts;

namespace KpiV3.Domain.Comments.Ports;

public interface ICommentRepository
{
    Task<Result<Comment, IError>> FindByIdAsync(Guid commentId);
    Task<Result<IError>> InsertAsync(Comment comment);
    Task<Result<IError>> UpdateAsync(Comment comment);
    Task<Result<IError>> DeleteAsync(Guid commentId);
}
