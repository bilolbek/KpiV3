namespace KpiV3.Infrastructure.Files.FileStorage;

public class S3ObjectStorageOptions
{
    public string Bucket { get; set; } = default!;
    public string AccessKey { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public string Endpoint { get; set; } = default!;
}
