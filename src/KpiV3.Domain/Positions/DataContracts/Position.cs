﻿namespace KpiV3.Domain.Positions.DataContracts;

public record Position
{
    public Guid Id { get; init; }
    public string Name { get; set; } = default!;
    public PositionType Type { get; set; }
}