namespace KpiV3.Domain.Files.DataContract;

public record UploadFileRequest
{
    public string Name { get; init; } = default!;
    public string ContentType { get; init; } = default!;
    public long Length { get; init; }
    public Stream Content { get; init; } = default!;
    public Guid OwnerId { get; init; }
}
