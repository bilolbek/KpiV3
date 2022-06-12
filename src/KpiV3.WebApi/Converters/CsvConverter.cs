using CsvHelper;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace KpiV3.WebApi.Converters;

public static class CsvConverter
{
    public static List<T> Convert<T>(Stream csvStream, Action<T>? onEach = null)
    {
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<T>().ToList();

        foreach (var record in records)
        {
            var context = new ValidationContext(record!);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(record!, context, results, true))
            {
                throw new InvalidOperationException(results.First().ErrorMessage!);
            }

            onEach?.Invoke(record);
        }

        return records;
    }

    public static string Convert<T>(List<T> records)
    {
        var writer = new StringWriter();
        var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteRecords(records);

        return writer.ToString();
    }
}
