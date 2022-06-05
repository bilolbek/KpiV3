using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Files;
using KpiV3.Domain.Files.DataContracts;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.DataContracts.Files;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[ApiVersion("3.0")]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;
    private readonly IEmployeeAccessor _employeeAccessor;
    private readonly IMediator _mediator;

    public FileController(
        FileService fileService,
        IEmployeeAccessor employeeAccessor,
        IMediator mediator)
    {
        _fileService = fileService;
        _employeeAccessor = employeeAccessor;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Page<FileMetadata>))]
    public async Task<IActionResult> GetUploadedFilesAsync([FromQuery] GetUploadedFilesRequest request)
    {
        return await _mediator
            .Send(request.ToQuery(_employeeAccessor.EmployeeId))
            .MatchAsync(files => Ok(files), error => error.MapToActionResult());
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
