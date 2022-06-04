using KpiV3.Domain.Ports;

namespace KpiV3.Infrastructure.Common;

public class GuidProvider : IGuidProvider
{
    public Guid New()
    {
        return Guid.NewGuid();
    }
}
