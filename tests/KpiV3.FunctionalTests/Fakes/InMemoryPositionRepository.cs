using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;

namespace KpiV3.FunctionalTests.Fakes;

public class InMemoryPositionRepository : IPositionRepository
{
    public InMemoryPositionRepository()
    {
        Items = new();
    }

    public Dictionary<Guid, Position> Items { get; }

    public Task<Result<IError>> DeleteAsync(Guid positionId)
    {
        Items.Remove(positionId);
        return Task.FromResult(Result<IError>.Ok());
    }

    public Task<Result<Position, IError>> FindByIdAsync(Guid positionId)
    {
        if (Items.TryGetValue(positionId, out var position))
        {
            return Task.FromResult(Result<Position, IError>.Ok(position));
        }

        return Task.FromResult(Result<Position, IError>.Fail(new NoEntity(typeof(Position))));
    }

    public Task<Result<Position, IError>> FindByNameAsync(string positionName)
    {
        if (Items.Values.FirstOrDefault(p => p.Name == positionName) is { } position)
        {
            return Task.FromResult(Result<Position, IError>.Ok(position));
        }

        return Task.FromResult(Result<Position, IError>.Fail(new NoEntity(typeof(Position))));
    }

    public Task<Result<IError>> InsertAsync(Position position)
    {
        Items[position.Id] = position;
        return Task.FromResult(Result<IError>.Ok());
    }

    public Task<Result<IError>> UpdateAsync(Position position)
    {
        Items[position.Id] = position;
        return Task.FromResult(Result<IError>.Ok());
    }
}
