using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Comments.Ports;
using KpiV3.Infrastructure.Comments.Data;
using KpiV3.Infrastructure.Data;

namespace KpiV3.Infrastructure.Comments.Repositories;

internal class CommentRepository : ICommentRepository
{
    private readonly Database _db;

    public CommentRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<Comment, IError>> FindByIdAsync(Guid commentId)
    {
        const string sql = @"
SELECT * FROM comments
WHERE id = @commentId";

        return await _db
            .QueryFirstAsync<CommentRow>(new(sql, new { commentId }))
            .MapAsync(row => row.ToModel());
    }

    public async Task<Result<IError>> InsertAsync(Comment comment)
    {
        const string sql = @"
INSERT INTO comments (id, author_id, block_id, content, written_date)
VALUES (@Id, @AuthorId, @BlockId, @Content, @WrittenDate)";

        return await _db.ExecuteAsync(new(sql, new CommentRow(comment)));
    }


    public async Task<Result<IError>> UpdateAsync(Comment comment)
    {
        const string sql = @"
UPDATE comments SET
    author_id = @AuthorId,
    block_id = @BlockId,
    content = @Content,
    written_date = @WrittenDate
WHERE id = @Id";

        return await _db.ExecuteRequiredChangeAsync<Comment>(new(sql, new CommentRow(comment)));
    }

    public async Task<Result<IError>> DeleteAsync(Guid commentId)
    {
        const string sql = @"DELETE FROM comments WHERE id = @commentId";

        return await _db.ExecuteRequiredChangeAsync<Comment>(new(sql, new { commentId }));
    }
}
