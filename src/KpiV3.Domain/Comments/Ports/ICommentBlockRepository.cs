using KpiV3.Domain.Comments.DataContracts;

namespace KpiV3.Domain.Comments.Ports;

public interface ICommentBlockRepository
{
    Task<Result<IError>> InsertAsync(CommentBlock block);
}
