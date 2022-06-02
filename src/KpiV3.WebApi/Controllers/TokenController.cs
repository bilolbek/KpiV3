using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.Authentication.DataContracts;
using KpiV3.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class TokenController : ControllerBase
{
    private readonly IJwtTokenFactory _jwtTokenFactory;

    public TokenController(IJwtTokenFactory jwtTokenFactory)
    {
        _jwtTokenFactory = jwtTokenFactory;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] Credentials credentials)
    {
        return await _jwtTokenFactory
            .CreateToken(credentials)
            .MatchAsync(token => Ok(token), error => error.MapToActionResult());
    }
}
