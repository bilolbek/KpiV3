namespace KpiV3.Domain.Common.Ports;

public interface IDateProvider
{
    DateTimeOffset Now();
}
