using Amazon.S3;
using Amazon.S3.Model;
using KpiV3.Domain.Files.Ports;
using Microsoft.Extensions.Options;

namespace KpiV3.Infrastructure.Files.FileStorage;

public class S3ObjectStorage : IFileStorage
{

    private readonly AmazonS3Client _client;
    private readonly S3ObjectStorageOptions _options;

    public S3ObjectStorage(
        AmazonS3Client client,
        IOptions<S3ObjectStorageOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<Result<IError>> InitAsync()
    {
        try
        {
            var buckets = await _client.ListBucketsAsync();

            if (!buckets.Buckets.Any(b => b.BucketName == _options.Bucket))
            {
                await _client.PutBucketAsync(_options.Bucket);
            }

            return Result<IError>.Ok();
        }
        catch (Exception exception)
        {
            return Result<IError>.Fail(new InternalError(exception));
        }
    }

    public async Task<Result<Stream, IError>> DownloadAsync(Guid id)
    {
        try
        {
            var response = await _client.GetObjectAsync(new GetObjectRequest
            {
                Key = id.ToString(),
                BucketName = _options.Bucket,
            });

            return Result<Stream, IError>.Ok(response.ResponseStream);
        }
        catch (Exception exception)
        {
            return Result<Stream, IError>.Fail(new InternalError(exception));
        }
    }

    public async Task<Result<IError>> UploadAsync(Guid id, Stream content)
    {
        try
        {
            await _client.PutObjectAsync(new PutObjectRequest
            {
                Key = id.ToString(),
                BucketName = _options.Bucket,
                InputStream = content,
                AutoCloseStream = false,
            });

            return Result<IError>.Ok();
        }
        catch (Exception exception)
        {
            return Result<IError>.Fail(new InternalError(exception));
        }
    }
}
