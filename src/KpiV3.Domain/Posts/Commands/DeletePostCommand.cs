using MediatR;

namespace KpiV3.Domain.Posts.Commands;

public record DeletePostCommand : IRequest
{
    public Guid PostId { get; init; }
}

public class DeletePostCommandHandler : AsyncRequestHandler<DeletePostCommand>
{
    private readonly KpiContext _db;

    public DeletePostCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _db.Posts
            .FindAsync(new object?[] { request.PostId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        _db.Posts.Remove(post);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
