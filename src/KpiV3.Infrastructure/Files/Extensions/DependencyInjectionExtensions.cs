using Amazon.S3;
using KpiV3.Domain.Files.Ports;
using KpiV3.Infrastructure.Files.FileStorage;
using KpiV3.Infrastructure.Files.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KpiV3.Infrastructure.Files.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFileAdapters(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<S3ObjectStorageOptions>(configuration.GetSection("S3"))
            .AddTransient<IValidateOptions<S3ObjectStorageOptions>, S3ObjectStorageOptionsValidator>()
            .AddTransient<IFileStorage, S3ObjectStorage>()
            .AddTransient<IFileMetadataRepository, FileMetadataRepository>()
            .AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<S3ObjectStorageOptions>>().Value;

                return new AmazonS3Client(
                    options.AccessKey,
                    options.SecretKey,
                    new AmazonS3Config
                    {
                        ServiceURL = options.Endpoint,
                        ForcePathStyle = true,
                    });
            });
    }
}
