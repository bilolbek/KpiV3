using KpiV3.Domain.Posts.DataContracts;
using KpiV3.Domain.Posts.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Posts.Data;

namespace KpiV3.Infrastructure.Posts.Repositories;

internal class PostRepository : IPostRepository
{
    private readonly Database _db;

    public PostRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<Post, IError>> FindByIdAsync(Guid postId)
    {
        const string sql = @"SELECT * FROM posts WHERE id = @postId";

        return await _db
            .QueryFirstAsync<PostRow>(new(sql, new { postId }))
            .MapAsync(row => row.ToModel());
    }

    public async Task<Result<IError>> InsertAsync(Post post)
    {
        const string sql = @"
INSERT INTO posts (id, author_id, comment_block_id, title, content, written_date)
VALUES (@Id, @AuthorId, @CommentBlockId, @Title, @Content, @WrittenDate)";

        return await _db.ExecuteAsync(new(sql, new PostRow(post)));
    }

    public async Task<Result<IError>> UpdateAsync(Post post)
    {
        const string sql = @"
UPDATE posts SET
    author_id = @AuthorId,
    comment_block_id = @CommentBlockId,
    title = @Title,
    content = @Content,
    written_date = @WrittenDate
WHERE id = @Id";

        return await _db.ExecuteRequiredChangeAsync<Post>(new(sql, new PostRow(post)));
    }

    public async Task<Result<IError>> DeleteAsync(Guid postId)
    {
        const string sql = @"DELETE FROM posts WHERE id = @postId";

        return await _db.ExecuteRequiredChangeAsync<Post>(new(sql, new { postId }));
    }
}
