namespace KpiV3.Domain.Files.DataContracts;

public record FileToUpload
{
    public string Name { get; set; } = default!;
    public Stream Content { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Length { get; set; }
    public Guid UploaderId { get; set; }
}
