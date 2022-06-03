using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KpiV3.WebApi.Authentication;

public class JwtOptions
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public TimeSpan TokenLifetime { get; set; }
    public string Secret { get; set; } = default!;

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
}
