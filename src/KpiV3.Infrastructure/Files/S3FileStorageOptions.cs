namespace KpiV3.Infrastructure.Files;

public class S3FileStorageOptions
{
    public string Bucket { get; set; } = default!;
    public string AccessKey { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public string Endpoint { get; set; } = default!;
}
