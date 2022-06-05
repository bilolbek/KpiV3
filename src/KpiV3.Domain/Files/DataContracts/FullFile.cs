namespace KpiV3.Domain.Files.DataContracts;

public record FullFile
{
    public Guid Id { get; set; }
    public Stream Content { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Length { get; set; }
}
