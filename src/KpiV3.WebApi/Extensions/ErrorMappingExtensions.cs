using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Extensions;

public static class ErrorMappingExtensions
{
    public static IActionResult MapToActionResult(this IError error)
    {
        return error switch
        {
            InvalidInput => new BadRequestObjectResult(ToModel(error)),

            NoEntity => new NotFoundResult(),

            BusinessRuleViolation => new BadRequestObjectResult(ToModel(error)),

            UnauthorizedAccess => new UnauthorizedResult(),

            ForbidenAction => new ForbidResult(),

            _ => new ObjectResult(ToModel(error))
            {
                StatusCode = 500,
            }
        };
    }

    private static object ToModel(IError error)
    {
        return new { Errors = new List<string> { error.Message } };
    }
}
