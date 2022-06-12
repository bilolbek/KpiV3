using KpiV3.WebApi.Authentication.DataContracts;
using KpiV3.WebApi.Authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;

    public TokenController(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(JwtToken))]
    [ProducesResponseType(401)]
    public async Task<IActionResult> CreateAsync([FromBody] Credentials credentials)
    {
        var token = await _jwtTokenService.CreateTokenAsync(credentials);

        return Ok(token);
    }
}
