using KpiV3.Domain.Indicators.DataContracts;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.Domain.Indicators.Commands;

public record CreateIndicatorCommand : IRequest<Indicator>
{
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string? Comment { get; set; }
}

public class CreateIndicatorCommandHandler : IRequestHandler<CreateIndicatorCommand, Indicator>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;

    public CreateIndicatorCommandHandler(
        KpiContext db,
        IGuidProvider guidProvider)
    {
        _db = db;
        _guidProvider = guidProvider;
    }

    public async Task<Indicator> Handle(CreateIndicatorCommand request, CancellationToken cancellationToken)
    {
        var indicator = new Indicator
        {
            Id = _guidProvider.New(),
            Name = request.Name,
            Description = request.Description,
            Comment = request.Comment,
        };

        _db.Indicators.Add(indicator);

        await _db.SaveChangesAsync(cancellationToken);

        return indicator;
    }
}
