using KpiV3.Domain.Positions.DataContracts;
using System.Security.Claims;

namespace KpiV3.WebApi.Authentication;

public class HttpContextEmployeeAccessor : IEmployeeAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextEmployeeAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid EmployeeId
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;

            if (context is null)
            {
                throw new InvalidOperationException("HttpContext was null");
            }

            return Guid.Parse(context.User.FindFirstValue("sub"));
        }
    }

    public Position Position
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;

            if (context is null)
            {
                throw new InvalidOperationException("HttpContext was null");
            }

            return new Position
            {
                Id = Guid.Parse(context.User.FindFirstValue("posId")),
                
                Name = context.User.FindFirstValue("posName"),
            };
        }
    }
}
