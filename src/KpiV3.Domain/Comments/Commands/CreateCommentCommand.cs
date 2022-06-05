using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Comments.Ports;
using KpiV3.Domain.Common;
using KpiV3.Domain.Employees.Ports;
using MediatR;

namespace KpiV3.Domain.Comments.Commands;

public record CreateCommentCommand : IRequest<Result<CommentWithAuthor, IError>>
{
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = default!;
    public Guid BlockId { get; set; }
}

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentWithAuthor, IError>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IDateProvider _dateProvider;
    private readonly IGuidProvider _guidProvider;

    public CreateCommentCommandHandler(
        IEmployeeRepository employeeRepository,
        ICommentRepository commentRepository,
        IDateProvider dateProvider,
        IGuidProvider guidProvider)
    {
        _employeeRepository = employeeRepository;
        _commentRepository = commentRepository;
        _dateProvider = dateProvider;
        _guidProvider = guidProvider;
    }

    public async Task<Result<CommentWithAuthor, IError>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment
        {
            Id = _guidProvider.New(),
            AuthorId = request.AuthorId,
            BlockId = request.BlockId,
            Content = request.Content,
            WrittenDate = _dateProvider.Now(),
        };

        return await _commentRepository
            .InsertAsync(comment)
            .InsertSuccessAsync(() => _employeeRepository
                .FindByIdAsync(comment.AuthorId)
                .MapAsync(author => new CommentWithAuthor(comment, author)));
    }
}
