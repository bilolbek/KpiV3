namespace KpiV3.Domain.Files.DataContracts;

public record FileMetadata
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public Guid UploaderId { get; set; }
    public long Length { get; set; }
}
