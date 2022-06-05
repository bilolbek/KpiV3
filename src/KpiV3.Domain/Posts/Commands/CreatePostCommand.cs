using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Comments.Ports;
using KpiV3.Domain.Common;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Posts.DataContracts;
using KpiV3.Domain.Posts.Ports;
using MediatR;

namespace KpiV3.Domain.Posts.Commands;

public record CreatePostCommand : IRequest<Result<PostWithAuthor, IError>>
{
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
}

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result<PostWithAuthor, IError>>
{
    private readonly IGuidProvider _guidProvider;
    private readonly IDateProvider _dateProvider;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IPostRepository _postRepository;
    private readonly ICommentBlockRepository _commentBlockRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public CreatePostCommandHandler(
        IGuidProvider guidProvider,
        IDateProvider dateProvider,
        ITransactionProvider transactionProvider,
        IPostRepository postRepository,
        ICommentBlockRepository commentBlockRepository,
        IEmployeeRepository employeeRepository)
    {
        _guidProvider = guidProvider;
        _dateProvider = dateProvider;
        _transactionProvider = transactionProvider;
        _postRepository = postRepository;
        _commentBlockRepository = commentBlockRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<PostWithAuthor, IError>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var commentBlock = new CommentBlock
        {
            Id = _guidProvider.New(),
            Type = CommentBlockType.Post,
        };

        var post = new Post
        {
            Id = _guidProvider.New(),
            AuthorId = request.AuthorId,
            Title = request.Title,
            Content = request.Content,
            CommentBlockId = commentBlock.Id,
            WrittenDate = _dateProvider.Now(),
        };

        return await _transactionProvider
            .RunAsync(() =>
                _commentBlockRepository
                    .InsertAsync(commentBlock)
                    .BindAsync(() => _postRepository.InsertAsync(post)))
            .InsertSuccessAsync(() => _employeeRepository
                .FindByIdAsync(post.AuthorId)
                .MapAsync(author => new PostWithAuthor(post, author)));
    }
}
