using KpiV3.WebApi.Converters;
using FluentAssertions;
using KpiV3.WebApi.DataContracts.Employees;
using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.FunctionalTests.Employees;

public class ImportTests : TestBase
{
    public ImportTests()
    {
        Authentication.Authenticate(PositionType.Root);
    }

    [Fact]
    public async Task Returns_ok_on_happy_path()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        var response = await SendCsvAsync(request);

        // Assert
        response.Should().Be200Ok();
    }

    [Fact]
    public async Task Creates_all_non_existing_positions_on_happy_path()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        var response = await SendCsvAsync(request);

        // Assert
        request.ForEach(employee =>
        {
            Positions.Items.Values.Should().Contain(p => p.Name == employee.Position);
        });
    }

    [Fact]
    public async Task Created_all_employees_on_happy_path()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        var response = await SendCsvAsync(request);

        // Assert
        request.ForEach(employee =>
        {
            Employees.Items.Values.Should().Contain(e =>
                e.Name.FirstName == employee.FirstName &&
                e.Name.LastName == employee.LastName &&
                e.Name.MiddleName == employee.MiddleName &&
                e.Email == employee.Email);
        });
    }

    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Returns_bad_request_if_input_is_invalid(List<CsvImportedEmployee> request)
    {
        // Arrange
        // Act
        var response = await SendCsvAsync(request);

        // Assert
        response.Should().Be400BadRequest();
    }

    private static IEnumerable<object[]> InvalidRequests()
    {
        var employee = new CsvImportedEmployee()
        {
            Email = "employee1@kpi.com",

            FirstName = "Employee1",
            LastName = "Employee1",
            MiddleName = "Employee1",

            Position = "Teacher"
        };

        yield return new object[] { ToRequest(employee with { Email = null! }) };
        yield return new object[] { ToRequest(employee with { FirstName = null! }) };
        yield return new object[] { ToRequest(employee with { LastName = null! }) };
        yield return new object[] { ToRequest(employee with { Position = null! }) };

        static List<CsvImportedEmployee> ToRequest(CsvImportedEmployee employee)
        {
            return new List<CsvImportedEmployee> { employee };
        }
    }

    private List<CsvImportedEmployee> CreateRequest()
    {
        return new List<CsvImportedEmployee>()
        {
            new()
            {
                Email = "employee1@kpi.com",

                FirstName = "Employee1",
                LastName = "Employee1",
                MiddleName = "Employee1",

                Position = "Teacher"
            },

            new()
            {
                Email = "employee2@kpi.com",

                FirstName = "Employee2",
                LastName = "Employee2",
                MiddleName = "",

                Position = "Dean"
            },
        };
    }

    private async Task<HttpResponseMessage> SendCsvAsync(List<CsvImportedEmployee> records)
    {
        var content = new MultipartFormDataContent();

        var writer = new StreamWriter(new MemoryStream());
        var csv = CsvConverter.Convert(records);
        writer.Write(csv);
        writer.Flush();
        var file = new StreamContent(writer.BaseStream);

        file.Headers.ContentType = new("text/csv");

        content.Add(file, name: "file", fileName: "file");

        return await Client.PostAsync("api/v3.0/employee/import", content);
    }
}
