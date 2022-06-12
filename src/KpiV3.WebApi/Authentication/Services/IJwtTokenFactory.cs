using KpiV3.Domain.Employees.DataContracts;
using KpiV3.WebApi.Authentication.DataContracts;

namespace KpiV3.WebApi.Authentication.Services;

public interface IJwtTokenFactory
{
    JwtToken CreateToken(Employee employee);
}
