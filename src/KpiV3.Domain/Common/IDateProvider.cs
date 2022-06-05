namespace KpiV3.Domain.Common;

public interface IDateProvider
{
    DateTimeOffset Now();
}
