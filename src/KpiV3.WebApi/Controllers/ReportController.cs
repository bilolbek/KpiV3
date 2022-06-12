using KpiV3.Domain.Reports.Queries;
using KpiV3.WebApi.Misc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "RootOnly")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly PdfGenerator _pdfGenerator;

    public ReportController(
        IMediator mediator,
        PdfGenerator pdfGenerator)
    {
        _mediator = mediator;
        _pdfGenerator = pdfGenerator;
    }

    [HttpGet("{periodId:guid}")]
    public async Task<IActionResult> GetAsync(Guid periodId, [FromQuery] List<Guid> employeeIds)
    {
        var reports = await _mediator.Send(new GetReportsQuery
        {
            EmployeeIds = employeeIds,
            PeriodId = periodId,
        });

        var pdf = _pdfGenerator.GeneratePdf(reports);

        return File(pdf.Content, MediaTypeNames.Application.Pdf, pdf.Name);
    }
}
