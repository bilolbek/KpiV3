using KpiV3.Domain.Files.DataContract;
using KpiV3.Domain.Files.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;

    public FileController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(UploadFileResponse))]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        var response = await _fileService.UploadFileAsync(new UploadFileRequest
        {
            Content = file.OpenReadStream(),
            ContentType = file.ContentType,
            Length = file.Length,
            Name = file.Name,
        });

        return Ok(response);
    }

    [HttpGet("{fileId:guid}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DownloadAsync(Guid fileId, CancellationToken cancellationToken)
    {
        var response = await _fileService.DownloadFileAsync(new DownloadFileRequest
        {
            FileId = fileId,
        }, cancellationToken);

        return File(response.Content, response.ContentType, response.Name);
    }
}
