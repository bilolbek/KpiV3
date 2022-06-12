using KpiV3.Domain.Positions.DataContracts;
using System.Security.Claims;

namespace KpiV3.WebApi.Authentication.Services;

public class EmployeeAccessor : IEmployeeAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmployeeAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid EmployeeId => Guid.Parse(FindClaimOrThrow("sub"));
    public Guid PositionId => Guid.Parse(FindClaimOrThrow("posId"));
    public string PositionName => FindClaimOrThrow("posName");
    public PositionType PositionType => Enum.Parse<PositionType>(FindClaimOrThrow("posType"));

    public string FindClaimOrThrow(string claimName)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            throw new InvalidOperationException("HttpContext was null");
        }

        var value = httpContext.User.FindFirstValue(claimName);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new UnauthorizedAccessException($"Claim '{claimName}' not found");
        }

        return value;
    }
}
