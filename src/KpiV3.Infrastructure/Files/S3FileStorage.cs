using Amazon.S3;
using Amazon.S3.Model;
using KpiV3.Domain.Files.Ports;
using Microsoft.Extensions.Options;

namespace KpiV3.Infrastructure.Files;

public class S3FileStorage : IFileStorage
{
    private readonly AmazonS3Client _client;
    private readonly S3FileStorageOptions _options;

    public S3FileStorage(
        AmazonS3Client client, 
        IOptions<S3FileStorageOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<Stream> DownloadAsync(Guid fileId)
    {
        var response = await _client.GetObjectAsync(new GetObjectRequest
        {
            Key = fileId.ToString(),
            BucketName = _options.Bucket,
        });

        return response.ResponseStream;
    }

    public async Task UploadAsync(Guid fileId, Stream content)
    {
        await _client.PutObjectAsync(new PutObjectRequest
        {
            Key = fileId.ToString(),
            BucketName = _options.Bucket,
            InputStream = content,
            AutoCloseStream = false,
        });
    }
}
