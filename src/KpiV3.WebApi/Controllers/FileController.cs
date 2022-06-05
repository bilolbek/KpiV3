using KpiV3.Domain.Files;
using KpiV3.Domain.Files.DataContracts;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("3.0")]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;
    private readonly IEmployeeAccessor _employeeAccessor;

    public FileController(
        FileService fileService,
        IEmployeeAccessor employeeAccessor)
    {
        _fileService = fileService;
        _employeeAccessor = employeeAccessor;
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(FileMetadata))]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        return await _fileService
            .UploadAsync(new FileToUpload
            {
                Name = file.Name,
                ContentType = file.ContentType,
                Content = file.OpenReadStream(),
                Length = file.Length,
                UploaderId = _employeeAccessor.EmployeeId,
            })
            .MatchAsync(metadata => Ok(metadata), error => error.MapToActionResult());
    }

    [HttpGet("{fileId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DownloadAsync(Guid fileId)
    {
        return await _fileService
            .DownloadAsync(fileId)
            .MatchAsync(file => File(file.Content, file.ContentType, file.Name), error => error.MapToActionResult());
    }
}
