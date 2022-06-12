namespace KpiV3.Domain.Files.DataContract;

public record DownloadFileResponse
{
    public Guid Id { get; init; }
    public string Name { get; set; } = default!;
    public string ContentType { get; init; } = default!;
    public Stream Content { get; init; } = default!;
    public long Length { get; init; }
}
