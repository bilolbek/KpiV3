using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using KpiV3.Domain.Reports.DataContracts;

namespace KpiV3.WebApi.Misc;

public class PdfGenerator
{
    public PdfModel GeneratePdf(Report report)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new PdfWriter(memoryStream);
        using var document = new PdfDocument(writer);
        using var pdf = new Document(document);

        var p = new Paragraph($"Report for {report.Period}")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20);

        var date = new Paragraph($"Requested date {report.CreatedDate}");

        pdf
            .Add(p)
            .Add(date);

        foreach (var employee in report.EmployeeReports)
        {
            var block = new Div();

            block
                .SetBorder(new SolidBorder(1))
                .SetPadding(10)
                .SetMarginBottom(20);

            block
                .Add(new Paragraph()
                    .Add(new Text("Employee: ").SetBold())
                    .Add(employee.FullName))
                .Add(new Paragraph()
                    .Add(new Text("Position: ").SetBold())
                    .Add(employee.Position))
                .Add(new Paragraph()
                    .Add(new Text("Specialty: ").SetBold())
                    .Add(employee.Specialty));

            var table = new Table(3, false);

            table
                .AddHeaderCell(Cell("Name"))
                .AddHeaderCell(Cell("Weight"))
                .AddHeaderCell(Cell("Grade"));

            foreach (var item in employee.Items)
            {
                table
                    .AddCell(Cell(item.Indicator))
                    .AddCell(Cell(Math.Round(item.Weight, 2).ToString()))
                    .AddCell(Cell(Math.Round(item.Value ?? 0, 2).ToString()));
            }

            table
                .SetMarginBottom(30)
                .SetWidth(UnitValue.CreatePercentValue(100));

            var kpi = new Paragraph($"{Math.Round(employee.Kpi, 2)}")
                .SetBorder(new SolidBorder(1))
                .SetPadding(3)
                .SetFontSize(12)
                .SetFontColor(employee.Kpi > 0.5 ? ColorConstants.GREEN : ColorConstants.RED);

            block
                .Add(new Paragraph("Indicators: ").SetFontSize(15))
                .Add(table)
                .Add(new Paragraph("KPI: ").Add(kpi).SetFontSize(15));

            pdf.Add(block);

            static Paragraph Cell(string content)
            {
                return new Paragraph(content)
                    .SetPadding(4)
                    .SetFontSize(10);
            }
        }

        pdf.Close();

        return new PdfModel
        {
            Name = $"Report.pdf",
            Content = memoryStream.ToArray()
        };
    }
}

public class PdfModel
{
    public string Name { get; set; } = default!;
    public byte[] Content { get; set; } = default!;
}