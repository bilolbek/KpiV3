using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Domain.Files.DataContract;

public class FileMetadata
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Length { get; set; }

    public Guid OwnerId { get; set; }
    public Employee Owner { get; set; } = default!;
}
