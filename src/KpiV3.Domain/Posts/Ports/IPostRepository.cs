using KpiV3.Domain.Posts.DataContracts;

namespace KpiV3.Domain.Posts.Ports;

public interface IPostRepository
{
    Task<Result<Post, IError>> FindByIdAsync(Guid postId);
    Task<Result<IError>> InsertAsync(Post post);
    Task<Result<IError>> UpdateAsync(Post post);
    Task<Result<IError>> DeleteAsync(Guid postId);
}
