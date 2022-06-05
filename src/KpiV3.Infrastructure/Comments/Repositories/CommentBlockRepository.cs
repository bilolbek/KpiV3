using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Comments.Ports;
using KpiV3.Infrastructure.Comments.Data;
using KpiV3.Infrastructure.Data;

namespace KpiV3.Infrastructure.Comments.Repositories;

internal class CommentBlockRepository : ICommentBlockRepository
{
    private readonly Database _db;

    public CommentBlockRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<IError>> InsertAsync(CommentBlock block)
    {
        const string sql = @"INSERT INTO comment_blocks (id, type) VALUES (@Id, @Type)";

        return await _db.ExecuteAsync(new(sql, new CommentBlockRow(block)));
    }
}
