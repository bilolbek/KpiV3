using KpiV3.Domain.Common.Exceptions;
using KpiV3.WebApi.DataContracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace KpiV3.WebApi.Filters;

public class ExceptionToResponseFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ExceptionToResponseFilterAttribute> _logger;

    public ExceptionToResponseFilterAttribute(ILogger<ExceptionToResponseFilterAttribute> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        var result = MapToResult(context);

        _logger.LogError("Unhandled exception occurred while executing request: {ex}", context.Exception);

        context.Result = result;
    }

    private static IActionResult MapToResult(ExceptionContext context)
    {
        return context.Exception switch
        {
            BusinessLogicException => new BadRequestObjectResult(new ErrorDto(context.Exception.Message)),
            InvalidInputException => new BadRequestObjectResult(new ErrorDto(context.Exception.Message)),
            UnauthorizedAccessException => new UnauthorizedObjectResult(new ErrorDto(context.Exception.Message)),
            ForbiddenActionException => new ForbidResult(),
            EntityNotFoundException => new NotFoundResult(),

            _ => new ObjectResult(new
            {
                context.Exception.Message,
                context.Exception.Source,
                ExceptionType = context.Exception.GetType().FullName,
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            }
        };
    }
}
