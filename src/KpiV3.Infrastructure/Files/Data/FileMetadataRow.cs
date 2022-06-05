using KpiV3.Domain.Files.DataContracts;

namespace KpiV3.Infrastructure.Files.Data;

internal class FileMetadataRow
{
    public FileMetadataRow()
    {
    }

    public FileMetadataRow(FileMetadata file)
    {
        Id = file.Id;
        Name = file.Name;
        ContentType = file.ContentType;
        Length = file.Length;
        UploaderId = file.UploaderId;
    }


    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Length { get; set; }
    public Guid UploaderId { get; set; }

    public FileMetadata ToModel()
    {
        return new FileMetadata
        {
            Id = Id,
            Name = Name,
            ContentType = ContentType,
            Length = Length,
            UploaderId = UploaderId,
        };
    }
}