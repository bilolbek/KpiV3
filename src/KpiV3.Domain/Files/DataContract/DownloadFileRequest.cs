namespace KpiV3.Domain.Files.DataContract;

public record DownloadFileRequest
{
    public Guid FileId { get; init; }
}
