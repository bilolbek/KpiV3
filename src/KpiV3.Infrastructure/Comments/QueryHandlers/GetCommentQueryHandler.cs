using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Comments.Queries;
using KpiV3.Infrastructure.Comments.Data;
using KpiV3.Infrastructure.Data;
using MediatR;

namespace KpiV3.Infrastructure.Comments.QueryHandlers;

internal class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, Result<CommentWithAuthor, IError>>
{
    private readonly Database _db;

    public GetCommentQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<CommentWithAuthor, IError>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT 
    c.id,
    c.content,
    c.written_date,
    c.block_id,
    e.id as ""author_id"",
    e.first_name as ""author_first_name"",
    e.last_name as ""author_last_name"",
    e.middle_name as ""author_middle_name"",
    e.avatar_id as ""author_avatar_id""
FROM comments c
INNER JOIN employees e on c.author_id = e.id
WHERE c.id = @CommentId";

        return await _db
            .QueryFirstAsync<CommentWithAuthorRow>(new(sql, new { request.CommentId }))
            .MapAsync(row => row.ToModel());
    }
}
