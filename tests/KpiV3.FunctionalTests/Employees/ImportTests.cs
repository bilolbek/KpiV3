using KpiV3.Domain.Employees.DataContracts;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MediatR;
using KpiV3.Domain.Employees.Commands;
using KpiV3.WebApi.Converters;
using FluentAssertions;

namespace KpiV3.FunctionalTests.Employees;

public class ImportTests : TestBase
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task Returns_ok_on_happy_path()
    {
        // Arrange
        var request = CreateRequest();
        var client = CreateClient();

        // Act
        var response = await SendCsvAsync(client, request);

        // Assert
        response.Should().Be200Ok();
    }

    [Fact]
    public async Task Maps_csv_file_to_request_correctly()
    {
        // Arrange
        var request = CreateRequest();
        var client = CreateClient();

        // Act
        await SendCsvAsync(client, request);

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<BulkRegisterEmployeesCommand>(c =>
                c.Employees.Count == request.Count &&
                c.Employees[0].Email == request[0].Email &&
                c.Employees[0].FirstName == request[0].FirstName &&
                c.Employees[0].LastName == request[0].LastName &&
                c.Employees[0].MiddleName == request[0].MiddleName &&
                c.Employees[0].Position == request[0].Position &&

                c.Employees[1].Email == request[1].Email &&
                c.Employees[1].FirstName == request[1].FirstName &&
                c.Employees[1].LastName == request[1].LastName &&
                c.Employees[1].MiddleName == request[1].MiddleName &&
                c.Employees[1].Position == request[1].Position),
            default));
    }

    private List<BulkRegisterEmployee> CreateRequest()
    {
        return new List<BulkRegisterEmployee>()
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

    private async Task<HttpResponseMessage> SendCsvAsync(HttpClient client, List<BulkRegisterEmployee> records)
    {
        var content = new MultipartFormDataContent();

        var writer = new StreamWriter(new MemoryStream());
        var csv = CsvConverter.Convert(records);
        writer.Write(csv);
        writer.Flush();
        var file = new StreamContent(writer.BaseStream);

        file.Headers.ContentType = new("text/csv");

        content.Add(file, name: "file", fileName: "file");

        return await client.PostAsync("api/v3.0/employee/import", content);
    }

    private HttpClient CreateClient()
    {
        Requestor = new Employee
        {
            Id = new("075bcdf4-0457-4d7b-a9d5-a2b8d3ef7deb")
        };

        RequestorPosition = new Position
        {
            Id = new("96c06b84-f752-4cf3-b729-ce5399af6434"),
            Name = "Admin",
            Type = PositionType.Root,
        };

        return Authorize(CreateClient((env, services) =>
        {
            services.Replace(ServiceDescriptor.Transient(_ => _mediator.Object));
        }));
    }
}
