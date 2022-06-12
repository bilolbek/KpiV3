using KpiV3.Domain.Reports.DataContracts;

namespace KpiV3.WebApi.Misc;

public class PdfGenerator
{
    public async Task<PdfModel> GenerateAsync(Report report)
    {
        var model = new PdfModel();



        return model;
    }
}

public class PdfModel
{
    public string Name { get; set; } = default!;
    public Stream Content { get; set; } = default!;
}