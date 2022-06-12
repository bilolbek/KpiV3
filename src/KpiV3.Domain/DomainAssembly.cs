using System.Reflection;

namespace KpiV3.Domain;

public static class DomainAssembly
{
    public static Assembly Instance { get; } = typeof(DomainAssembly).Assembly;
}
