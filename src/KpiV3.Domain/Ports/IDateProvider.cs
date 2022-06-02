namespace KpiV3.Domain.Ports;

public interface IDateProvider
{
    DateTimeOffset Now();
}
